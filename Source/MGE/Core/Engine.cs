using System;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MGE.Graphics;
using MGE.UI;
using MGE.InputSystem;
using MGE.UI.Elements;
using System.Collections.Generic;

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

				graphics = new GraphicsDeviceManager(game);

				Logger.throwOnError = Args.HasFlag("--throw-on-error");

				GCSettings.LatencyMode = GCLatencyMode.LowLatency;

				game.InactiveSleepTime = TimeSpan.Zero;
				game.IsFixedTimeStep = false;
				game.Window.Title = MGEConfig.gameName;
				game.Window.AllowUserResizing = MGEConfig.allowWindowResizing;
				game.Window.ClientSizeChanged += (sender, args) => OnResize();
				game.Window.TextInput += (sender, args) => Input.TextInput(args);
				game.Activated += (sender, args) => Window.isFocused = true;
				game.Deactivated += (sender, args) => Window.isFocused = false;

				Pointer.mode = PointerMode.System;
				Pointer.mouseCursor = MouseCursor.Wait;

				// IntPtr hWnd = game.Window.Handle;
				// System.Windows.Forms.Control ctrl = System.Windows.Forms.Control.FromHandle(hWnd);
				// System.Windows.Forms.Form form = ctrl.FindForm();
				// form.TransparencyKey = Color.clear;
			}
		}

		public void Initialize()
		{
			Logger.Log("Initializing...");

			using (Timmer.Create("Initialize"))
			{
				graphics.SynchronizeWithVerticalRetrace = Args.HasFlag("--enable-v-sync");
				graphics.ApplyChanges();

				MGE.Window.aspectRatioFrac = MGEConfig.aspectRatio;
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
			}
		}

		public void UnloadContent() => Assets.UnloadAssets();

		public void Update(GameTime gameTime)
		{
			Time.Update(gameTime);
			Input.Update();

			if (Input.GetButtonPress(Inputs.F11))
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

			if (statsUpdateCooldown < 0.0f)
			{
				statsUpdateCooldown = MGEConfig.timeBtwStatsUpdate;
				Stats.Update();
			}
			statsUpdateCooldown -= (float)Time.deltaTime;

			GUI.Update();

			SceneManager.current.Update();

			// GUI.AddElement(new GUIStackLayout(new List<GUIElement>()
			// {
			// 	new GUIImage() { color = new Color(1f, 0.25f, 0f, 0.5f) },
			// 	new GUIImage() { color = new Color(0f, 1f, 0.25f, 0.5f) },
			// 	new GUIImage() { color = new Color(0.25f, 0f, 1f, 0.5f) },
			// }, 64));
		}

		public void Draw(GameTime gameTime)
		{
			GFX.drawCalls = 0;

			game.GraphicsDevice.Clear(Color.nullColor);

			SceneManager.current.Draw();

			SceneManager.current.DrawUI();

			GUI.Draw();

			Terminal.Draw();

			Pointer.Draw();
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