using GAME.States;
using MGE;
using MGE.Graphics;
using MGE.FileIO;
using XNA_Game = Microsoft.Xna.Framework.Game;
using XNA_GameTime = Microsoft.Xna.Framework.GameTime;

namespace GAME
{
	public class Main : XNA_Game
	{
		public static Main current { get; private set; }

		public Engine engine;

		public GameState state;

		public Main()
		{
			current = this;

			engine = new Engine(this);
		}

		protected override void Initialize()
		{
			engine.Initialize();

			engine.onTick += () => OnTick();

			engine.LoadContent();

			GameSettings.current = new GameSettings();

			try
			{
				GameSettings.current.stage = IO.Load<Stage>($"Assets/Stages/Untitled Stage.stage", false);
			}
			catch (System.Exception)
			{
				GameSettings.current.stage = new Stage((Vector2)MGE.Window.sceneSize);

				var noise = new Noise();
				noise.noiseType = Noise.NoiseType.OpenSimplex2S;
				noise.fractalType = Noise.FractalType.FBm;
				noise.octaves = 8;
				noise.gain = 0.5f;
				noise.frequency = 0.001f;

				GameSettings.current.stage.tiles.For((x, y) =>
				{
					var pos = new Vector2(x, y) / (Vector2)GameSettings.current.stage.tiles.size;

					return
						(byte)(x == 0 || y == 0 || x == GameSettings.current.stage.tiles.size.x - 1 || y == GameSettings.current.stage.tiles.size.y - 1 ? 3 :
						(noise.GetNoise(x * 8, 0).Abs() * 1.25f + 0.33f < pos.y ? 2 : 0));
				});
			}

			Pointer.mode = PointerMode.System;

			Microsoft.Xna.Framework.Audio.SoundEffect.MasterVolume = 0.33f;

			ChangeState(new StatePlaying());

			base.Initialize();
		}

		protected override void LoadContent()
		{
			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			engine.UnloadContent();

			base.UnloadContent();
		}

		public void OnTick()
		{
			state.Tick();
		}

		protected override void Update(XNA_GameTime gameTime)
		{
			Window.Title = $"MGE Party Game | {Math.Round(Stats.fps)}fps {Math.Round(Stats.averageFps)}avg {Math.Round(Stats.minFps)}min | {Math.Round(Stats.memUsedAsMBs, 1)}MB / {Math.Round(Stats.memAllocatedAsMBs)}MB";

			foreach (var controller in GameSettings.current.controllers.Values)
			{
				controller.Update();
			}

			state.Update();

			engine.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(XNA_GameTime gameTime)
		{
			engine.Draw(gameTime);

			using (new DrawBatch(transform: null))
				state.Draw();

			base.Draw(gameTime);
		}

		public void ChangeState(GameState state)
		{
			this.state?.Exit();
			this.state = null;

			System.GC.Collect();

			this.state = state;
			this.state.Init();
		}
	}
}