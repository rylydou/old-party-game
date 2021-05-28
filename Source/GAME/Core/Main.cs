using GAME.States;
using MGE;
using MGE.Graphics;
using XNA_Game = Microsoft.Xna.Framework.Game;
using XNA_GameTime = Microsoft.Xna.Framework.GameTime;
using MGE.InputSystem;
using Microsoft.Xna.Framework.Audio;
using MGE.ECS;

namespace GAME
{
	public class Main : XNA_Game
	{
		public readonly System.Version version;

		public static Main current { get; private set; }

		public Engine engine;

		public GameState state;

		public Main()
		{
			current = this;

			engine = new Engine(this);

			version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
		}

		protected override void Initialize()
		{
			engine.Initialize();

			engine.onTick += () => OnTick();
			engine.onAfterRenderGame += () => OnAfterRenderGame();
			engine.onBeforeRenderUI += () => OnBeforeRenderUI();
			engine.onAfterRenderUI += () => OnAfterRenderUI();

			engine.LoadContent();

			GameSettings.current = new GameSettings();

			try
			{
				GameSettings.stage = Assets.LoadAsset<Stage>("@ Stages/Test");
			}
			catch (System.Exception e)
			{
				Logger.Log(e);
				GameSettings.stage = new Stage(new Vector2Int(40, 23));
			}

			SoundEffect.MasterVolume = 0.1f;

			ChangeState(new StateMainMenu());

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
			state?.Tick();
		}

		protected override void Update(XNA_GameTime gameTime)
		{
			Window.Title = $"MGE Party Game | {Math.Round(Stats.fps)}fps {Math.Round(Stats.averageFps)}avg {Math.Round(Stats.minFps)}min | {Stats.memUsedAsMBs.ToString("F1")}MB / {Math.Round(Stats.memAllocatedAsMBs)}MB";

			var shift = Input.GetButton(Inputs.LeftShift) | Input.GetButton(Inputs.RightShift);
			var ctrl = Input.GetButton(Inputs.LeftControl) | Input.GetButton(Inputs.RightControl);
			var alt = Input.GetButton(Inputs.LeftAlt) | Input.GetButton(Inputs.RightAlt);

			if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Tilde))
				ChangeState(new StateMainMenu());

			foreach (var controller in GameSettings.controllers)
				controller.Update();

			if (state is object)
			{
				state.Update();
			}

			engine.Update(gameTime);

			Discord.SetDetails(GameSettings.stage is null ?
				$"Playing with {GameSettings.players.Count} players" :
				$"Playing on {GameSettings.stage.name} with {GameSettings.players.Count} players"
			);

			if (state is StateMainMenu)
				Discord.SetState("In Main Menu");
			else if (state is StatePlayerSetup)
				Discord.SetState("In Setup Menu");
			else if (state is StatePlaying)
				Discord.SetState("Playing");
			else if (state is StateEditor)
				Discord.SetState("In Stage Editor");

			base.Update(gameTime);
		}

		protected override void Draw(XNA_GameTime gameTime)
		{
			engine.Draw(gameTime);

			base.Draw(gameTime);
		}

		void OnAfterRenderGame()
		{
			using (new DrawBatch(false))
			{
				state?.Draw();
			}
		}

		void OnBeforeRenderUI() { }

		void OnAfterRenderUI()
		{
			using (new DrawBatch(true))
			{
				state?.DrawUI();

#if !INDEV
				Config.font.DrawText($"v{version} {System.DateTime.Now.ToString("yyyy-MM-dd")}", new Rect(16, MGE.GUI.canvasSize.y - 38, MGE.GUI.canvasSize.x, 32), new Color(1, 0.33f));
#endif
			}
		}

		public void ChangeState(GameState state)
		{
			this.state?.Exit();
			this.state = null;

			Time.timeScale = 1.0f;

			System.GC.Collect();

			this.state = state;
			this.state?.Init();

			if (SceneManager.activeScene is object)
				SceneManager.activeScene.clearScreen = false;
		}
	}
}