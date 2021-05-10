using System;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MGE.Graphics;
using MGE.UI;
using MGE.InputSystem;
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

		public Action onTick = () => { };

		float timeSinceLastTick = 0.0f;
		float statsUpdateCooldown = -1.0f;

		bool shouldScreenshot = false;

		public Engine(Game game)
		{
			if (game == null)
				throw new Exception("Game cannot be null!");

			using (Timmer.Start("Constructing"))
			{
				_current = this;
				_game = game;

				graphics = new GraphicsDeviceManager(game);

				Logger.throwOnError = Args.HasFlag("--throw-on-error");

				GCSettings.LatencyMode = GCLatencyMode.LowLatency;

				game.IsFixedTimeStep = false;
				game.InactiveSleepTime = TimeSpan.Zero;
				game.Window.Title = Config.gameName;
				game.Window.AllowUserResizing = Config.allowWindowResizing;
				game.Window.ClientSizeChanged += (sender, args) => OnResize();
				game.Window.TextInput += (sender, args) => Input.TextInput(args);
				game.Activated += (sender, args) => Window.isFocused = true;
				game.Deactivated += (sender, args) => Window.isFocused = false;

				Pointer.mode = PointerMode.System;
				Pointer.mouseCursor = MouseCursor.Wait;
			}
		}

		public void Initialize()
		{
			using (Timmer.Start("Initialize"))
			{
				graphics.SynchronizeWithVerticalRetrace = false;
				graphics.PreferMultiSampling = true;
				graphics.GraphicsProfile = GraphicsProfile.HiDef;
				graphics.ApplyChanges();

				MGE.Window.aspectRatioFrac = Config.aspectRatio;
				MGE.Window.windowedSize = Config.defaultWindowSize;
				MGE.Window.windowedPosition = (MGE.Window.monitorSize - Config.defaultWindowSize) / 2;
				MGE.Window.Apply();

				OnResize();

				Camera.Init();

				Input.GamepadInit();

				Pointer.mode = PointerMode.Texture;
				Pointer.hotspot = new Vector2(0);
				Pointer.size = new Vector2(16);
				Pointer.color = Colors.accent;
				Pointer.shadowColor = new Color(0.0f, 0.1f);
				Pointer.shadowOffset = new Vector2(2);
			}
		}

		public void LoadContent()
		{
			using (Timmer.Start("Load Content"))
			{
				sb = new SpriteBatch(game.GraphicsDevice);

				Assets.ReloadAssets();

				Pointer.texture = Assets.GetAsset<Texture>("Sprites/Pointer");

				Pointer.mouseCursor = MouseCursor.Arrow;
			}
		}

		public void UnloadContent() => Assets.UnloadAssets();

		public void Update(GameTime gameTime)
		{
			Time.Update(gameTime);
			Input.Update();

			if (Input.GetButtonPress(Inputs.F2))
			{
				shouldScreenshot = true;
			}

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

			statsUpdateCooldown -= Time.deltaTime;
			if (statsUpdateCooldown < 0.0f)
			{
				statsUpdateCooldown = Config.timeBtwStatsUpdate;
				Stats.Update();
			}

			GUI.Update();

			GFX.SetupToDrawGame();

			timeSinceLastTick += Time.deltaTime;
			while (timeSinceLastTick > Config.timeBtwTicks)
			{
				Time.ticks++;
				timeSinceLastTick -= Config.timeBtwTicks;
				SceneManager.Tick();
				onTick.Invoke();
			}
			SceneManager.Update();

			GFX.SetupToDrawUI();
		}

		public void Draw(GameTime gameTime)
		{
			GFX.SetupToDrawGame();
			GFX.drawCalls = 0;

			using (var rt = new RenderTarget2D(game.GraphicsDevice, Window.gameRenderSize.x, Window.gameRenderSize.y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents, false, 1))
			{
				game.GraphicsDevice.SetRenderTarget(rt);

				SceneManager.Draw();

				game.GraphicsDevice.SetRenderTarget(null);

				using (new DrawBatch(transform: null, effect: Camera.postEffect, blend: BlendState.Opaque))
				{
					sb.Draw(rt, new Rect(0, 0, Window.windowedSize), Color.white);
				}

				if (shouldScreenshot)
				{
					var path = $"Screenshots/{DateTime.Now.ToString(@"yyyy-mm-dd hh-mm-ss")}.png";

					try
					{
						using (var png = FileIO.IO.FileOpen(path))
						{
							rt.SaveAsPng(png, rt.Width, rt.Height);
						}

						Logger.Log("Screenshot saved!");
					}
					catch (System.Exception e)
					{
						try
						{
							Logger.LogWarning($"Could not save screenshot!\n{e}");

							FileIO.IO.FileDelete(path);
						}
						catch
						{
							Logger.LogError($"Could not delete screenshot! It is most likely corrupted :(\n{e}");
						}
					}
				}
				shouldScreenshot = false;
			}

			GFX.SetupToDrawUI();

			SceneManager.DrawUI();

			GUI.gui.Draw();

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