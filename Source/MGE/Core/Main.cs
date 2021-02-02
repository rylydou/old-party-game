using System;
using System.Collections.Generic;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using MonoGame.Framework;
using MonoGame.Framework.Utilities;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.FileIO;
using MGE.ECS;

namespace MGE
{
	public partial class Engine
	{
		static Engine _current;
		public static Engine current { get => _current; }

		static Game _game;
		public static Game game { get => _game; }

		public GraphicsDeviceManager graphics;
		public SpriteBatch sb;
		public Camera camera;

		float statsUpdateCooldown;

		public Engine(Game game)
		{
			if (game == null)
				throw new Exception("Game cannot be null!");

			Logger.Log("Constructing...");

			using (Timmer.Create("Constructing"))
			{
				_current = this;
				_game = game;

				Logger.throwOnError = Args.HasFlag("--throw-on-error");

				GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

				game.InactiveSleepTime = TimeSpan.Zero;
				game.IsFixedTimeStep = false;
				game.Window.Title = MGEConfig.gameName;
				game.Window.AllowUserResizing = MGEConfig.allowWindowResizing;
				game.Window.ClientSizeChanged += (sender, args) => OnResize();
				game.Window.TextInput += (sender, args) => Input.TextInput(args);

				graphics = new GraphicsDeviceManager(game);

				Pointer.mode = PointerMode.System;
				Pointer.mouseCursor = MouseCursor.Wait;
			}
		}

		public void Initialize()
		{
			Logger.Log("Initializing...");

			using (Timmer.Create("Initialize"))
			{
				App.exePath = IO.CleanPath(Environment.CurrentDirectory);

				graphics.SynchronizeWithVerticalRetrace = Args.HasFlag("--enable-v-sync");
				graphics.ApplyChanges();

				MGE.Window.fullAspectRatio = MGEConfig.aspectRatio;
				MGE.Window.windowedSize = MGEConfig.defaultWindowSize;
				MGE.Window.windowedPosition = (MGE.Window.monitorSize - MGEConfig.defaultWindowSize) / 2;
				MGE.Window.Apply();

				OnResize();

				camera = new Camera();

				Input.GamepadInit();

				Pointer.mode = PointerMode.Texture;
				Pointer.hotspot = new Vector2(0);
				Pointer.size = new Vector2(16);
				Pointer.color = Color.red;
				Pointer.shadowColor = new Color(0.0f, 0.1f);
				Pointer.shadowOffset = new Vector2(2);

				// IO.Save("/Data/settings.json", new TestStruct(true));
			}
		}

		public void LoadContent()
		{
			Logger.Log("Loading Content...");

			using (Timmer.Create("Load Content"))
			{
				sb = new SpriteBatch(game.GraphicsDevice);

				Assets.ReloadAssets();

				Pointer.texture = Assets.GetAsset<Texture2D>("Sprites/Pointer");

				new SceneManager(
					new Scene(new List<Layer>()
						{
							new Layer(new List<Entity>()
								{
									new Entity(new List<Component>(){new GAME.Components.CBackground()}),
									new Entity(new List<Component>(){new GAME.Components.CPlayer()})
								}
							)
						}
					)
				);
			}
		}

		public void UnloadContent() => Assets.UnloadAssets();

		public void Update(GameTime gameTime)
		{
			Input.Update();

			if (Input.CheckButtonPress(Inputs.F11))
			{
				switch (MGE.Window.windowMode)
				{
					case WindowMode.Windowed: MGE.Window.windowMode = WindowMode.BorderlessWindowed; break;
					case WindowMode.BorderlessWindowed: MGE.Window.windowMode = WindowMode.Fullscreen; break;
					case WindowMode.Fullscreen: MGE.Window.windowMode = WindowMode.Windowed; break;
				}

				// TODO: Don't be dumb
				MGE.Window.Apply();
				MGE.Window.Apply();
			}

			Time.time = gameTime.TotalGameTime.TotalSeconds;
			Time.deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

			if (statsUpdateCooldown < 0.0f)
			{
				statsUpdateCooldown = MGEConfig.timeBtwStatsUpdate;
				Stats.Update();
			}
			statsUpdateCooldown -= (float)Time.deltaTime;

			SceneManager.current.Update();
		}

		public void Draw(GameTime gameTime)
		{
			Graphics.Graphics.drawCalls = 0;

			game.GraphicsDevice.Clear(Color.nullColor);

			SceneManager.current.Draw();

			SceneManager.current.DrawUI();

			Terminal.Draw();

			DrawPointer();
		}

		void DrawPointer()
		{
			if (Pointer.mode == PointerMode.Texture)
			{
				using (new DrawBatch(transform: null))
				{
					Graphics.Graphics.Draw(Pointer.texture, new Rect((Vector2)Mouse.GetState().Position - Pointer.size * Pointer.hotspot + Pointer.shadowOffset, Pointer.size), Pointer.shadowColor);
					Graphics.Graphics.Draw(Pointer.texture, new Rect((Vector2)Mouse.GetState().Position - Pointer.size * Pointer.hotspot, Pointer.size), Pointer.color);
				}
			}
		}

		public void OnResize()
		{
			MGE.Window.FixWindow();

			UpdateWindowSizes();

			MGE.Window.onResize.Invoke();
		}

		public void UpdateWindowSizes()
		{
			MGE.Window.renderSize = new Vector2Int(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
		}
	}
}