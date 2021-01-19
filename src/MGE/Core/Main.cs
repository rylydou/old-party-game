using System;
using System.Collections.Generic;
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
	public partial class Main : Game
	{
		static Main _current;
		public static Main current { get => _current; }

		public GraphicsDeviceManager graphics;
		public SpriteBatch sb;
		public Camera camera;

		float statsUpdateCooldown;

		public Main()
		{
			Logger.Log("Constructing...");

			using (Timmer.Create("Constructing"))
			{
				_current = this;

				graphics = new GraphicsDeviceManager(this);

				Pointer.mode = PointerMode.System;
				Pointer.mouseCursor = MouseCursor.Wait;

				Window.ClientSizeChanged += (sender, args) => OnResize();
				Window.TextInput += (sender, args) => Input.TextInput(args);
			}
		}

		protected override void Initialize()
		{
			Logger.Log("Initializing...");

			using (Timmer.Create("Initialize"))
			{
				App.exePath = IO.CleanPath(Environment.CurrentDirectory);

				MGE.Window.fullAspectRatio = MGEConfig.aspectRatio;
				MGE.Window.windowedSize = MGEConfig.defaultWindowSize;
				MGE.Window.windowedPosition = (MGE.Window.monitorSize - MGEConfig.defaultWindowSize) / 2;
				MGE.Window.Apply();

				OnResize();

				camera = new Camera();

				graphics.SynchronizeWithVerticalRetrace = true;
				graphics.ApplyChanges();

				IsFixedTimeStep = false;

				Window.Title = MGEConfig.gameName;
				Window.AllowUserResizing = MGEConfig.allowWindowResizing;

				Input.GamepadInit();

				// IO.Save("/Data/settings.json", new TestStruct(true));
			}

			base.Initialize();

			Pointer.mode = PointerMode.Texture;
			Pointer.texture = Assets.GetAsset<Texture2D>("Sprites/Pointer.psd");
			Pointer.hotspot = new Vector2(0);
			Pointer.size = new Vector2(16);
			Pointer.color = Color.red;
			Pointer.shadowColor = new Color(0.0f, 0.1f);
			Pointer.shadowOffset = new Vector2(2);
		}

		protected override void LoadContent()
		{
			Logger.Log("Loading Content...");

			using (Timmer.Create("Load Content"))
			{
				sb = new SpriteBatch(GraphicsDevice);

				// > Reload Assets
				Assets.ReloadAssets();

				new SceneManager(
					new Scene(new List<Layer>()
						{
							new Layer(new List<Entity>()
								{
									new Entity(new List<Component>(){new GAME.Components.Background()})
								}
							)
						}
					)
				);
			}
		}

		protected override void UnloadContent() => Assets.UnloadAssets();

		protected override void Update(GameTime gameTime)
		{
			// > Engine <
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

			// > Game <
			SceneManager.current.Update();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.nullColor);

			SceneManager.current.Draw();

			SceneManager.current.DrawUI();

			Terminal.Draw();

			DrawPointer();

			base.Draw(gameTime);
		}

		void DrawPointer()
		{
			if (Pointer.mode == PointerMode.Texture)
			{
				sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp);
				sb.Draw(Pointer.texture, new Rect((Vector2)Mouse.GetState().Position - Pointer.size * Pointer.hotspot + Pointer.shadowOffset, Pointer.size), Pointer.shadowColor);
				sb.Draw(Pointer.texture, new Rect((Vector2)Mouse.GetState().Position - Pointer.size * Pointer.hotspot, Pointer.size), Pointer.color);
				sb.End();
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