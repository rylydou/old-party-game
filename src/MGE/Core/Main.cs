using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using MonoGame.Framework;
using MonoGame.Framework.Utilities;
using MGE.Graphics;
using MGE.FileIO;

namespace MGE
{
	public partial class Main : Game
	{
		static Main _current;
		public static Main current { get => _current; }

		public GraphicsDeviceManager graphics;
		public SpriteBatch sb;
		public Camera camera;

		public Vector2 mousePos;

		public Main()
		{
			Logger.Log("Constructing...");

			using (var timmer = Timmer.Create("Constructing"))
			{
				_current = this;

				graphics = new GraphicsDeviceManager(this);

				Pointer.mode = PointerMode.System;
				Pointer.mouseCursor = MouseCursor.Wait;
			}
		}

		protected override void Initialize()
		{
			Logger.Log("Initializing...");

			using (var timmer = Timmer.Create("Initialize"))
			{
				App.exePath = IO.CleanPath(Environment.CurrentDirectory);

				Window.ClientSizeChanged += (obj, args) => OnResize();
				MGE.Window.fullAspectRatio = new Vector2(16.0, 9.0);
				MGE.Window.windowedSize = MGE.Window.monitorSize / 2;
				MGE.Window.windowedPosition = MGE.Window.monitorSize / 4;
				MGE.Window.Apply();

				OnResize();

				camera = new Camera();

				graphics.SynchronizeWithVerticalRetrace = false;
				graphics.ApplyChanges();

				IsFixedTimeStep = false;

				Window.Title = "MGE Game";
				Window.AllowUserResizing = true;

				// Pointer.sprite = new DrawCall(Assets.GetAsset<Texture2D>("Sprites/Pointer.png"), Color.red, new Rect(Vector2.zero, new Vector2(16)), Vector2.zero);
			}

			base.Initialize();

			Pointer.mode = PointerMode.Sprite;
			Pointer.sprite = new DrawCall(Assets.GetAsset<Texture2D>("Sprites/Pointer.png"), Color.red, new Rect(Vector2.zero, new Vector2(16)), Vector2.zero);
		}

		protected override void LoadContent()
		{
			Logger.Log("Loading Content...");

			using (var timmer = Timmer.Create("Load Content"))
			{
				sb = new SpriteBatch(GraphicsDevice);

				// > Reload Assets
				Assets.ReloadAssets();

				ScreenManager._current = new ScreenManager();
				ScreenManager.current.QueueScreen(new GAME.Screens.MenuScreen());
			}
		}

		protected override void UnloadContent() => Assets.UnloadAssets();

		protected override void Update(GameTime gameTime)
		{
			// > Engine <
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (Keyboard.GetState().IsKeyDown(Keys.Q)) MGE.Window.windowMode = WindowMode.Windowed;
			if (Keyboard.GetState().IsKeyDown(Keys.W)) MGE.Window.windowMode = WindowMode.BorderlessWindowed;
			if (Keyboard.GetState().IsKeyDown(Keys.E)) MGE.Window.windowMode = WindowMode.Fullscreen;
			if (Keyboard.GetState().IsKeyDown(Keys.Space)) MGE.Window.Apply();

			Time.time = gameTime.TotalGameTime.TotalSeconds;
			Time.deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

			// double fps = 1.0 / Time.deltaTime;
			// if (fps > 1) Logger.Log($"{Math.Round(fps)} fps");
			// else Logger.Log($"{Math.Round(fps * 100.0) / 100.0} fps");

			// > Game <
			ScreenManager.current.activeScreen.Update();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.nullColor);

			ScreenManager.current.activeScreen.Draw();

			ScreenManager.current.activeScreen.DrawUI();

			DrawPointer();

			base.Draw(gameTime);
		}

		void DrawPointer()
		{
			if (Pointer.mode == PointerMode.Sprite)
			{
				sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp);
				sb.Draw(Pointer.sprite.sprite, new Rect((Vector2)Mouse.GetState().Position - Pointer.sprite.rect.size * Pointer.sprite.center + new Vector2(0, 4), Pointer.sprite.rect.size), new Color(0, 0, 0, 0.1f));
				sb.Draw(Pointer.sprite.sprite, new Rect((Vector2)Mouse.GetState().Position - Pointer.sprite.rect.size * Pointer.sprite.center, Pointer.sprite.rect.size), Pointer.sprite.color);
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