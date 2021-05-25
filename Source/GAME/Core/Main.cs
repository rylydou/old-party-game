using GAME.States;
using MGE;
using MGE.Graphics;
using MGE.FileIO;
using XNA_Game = Microsoft.Xna.Framework.Game;
using XNA_GameTime = Microsoft.Xna.Framework.GameTime;
using MGE.InputSystem;
using MGE.Physics;
using Microsoft.Xna.Framework.Audio;
using MGE.ECS;

namespace GAME
{
	public class Main : XNA_Game
	{
		public static Main current { get; private set; }

		public Engine engine;

		public bool infiniteTime = true;

		public GameState state;

		public bool changingStage;

		public Main()
		{
			current = this;

			engine = new Engine(this);
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
				GameSettings.stage = Stage.Load("Test");
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

			if (!shift && ctrl && !alt && Input.GetButtonPress(Inputs.S))
				GameSettings.stage.Save();
			else if (!shift && ctrl && !alt && Input.GetButtonPress(Inputs.L))
				GameSettings.stage = Stage.Load(GameSettings.stage.name);
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Tilde))
				ChangeState(null);
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Plus))
				SoundEffect.MasterVolume = Math.Clamp01(SoundEffect.MasterVolume + 0.1f);
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Minus))
				SoundEffect.MasterVolume = Math.Clamp01(SoundEffect.MasterVolume - 0.1f);

			if (state is object)
			{
				foreach (var controller in GameSettings.controllers)
					controller.Update();

				state.Update();
			}
			else
			{
				if (changingStage)
				{
					foreach (var key in Input.keyboardString)
					{
						if ((int)key <= 32)
						{
							switch (key)
							{
								case ' ':
									GameSettings.stage.name += ' ';
									break;
								case (char)13:
									changingStage = false;
									break;
								case '\n':
									changingStage = false;
									break;
								case '\b':
									if (GameSettings.stage.name.Length > 0)
										GameSettings.stage.name = GameSettings.stage.name.Remove(GameSettings.stage.name.Length - 1, 1);
									break;
							}
						}
						else
						{
							GameSettings.stage.name += key;
						}
					}
				}
				else
				{
					if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D1))
						ChangeState(new StatePlaying());
					else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D2))
						ChangeState(new StateEditor());
					else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D3))
						ChangeState(new StateMainMenu());
				}

				if (Input.GetButtonPress(Inputs.F2))
					changingStage = !changingStage;
			}

			engine.Update(gameTime);

			Discord.SetDetails($"Playing on {GameSettings.stage.name} with {GameSettings.players.Count} players");

			if (state is StateMainMenu)
				Discord.SetState("Main Menu");
			else if (state is StatePlayerSetup)
				Discord.SetState("Setup");
			else if (state is StatePlaying)
				Discord.SetState("Playing");
			else if (state is StateEditor)
				Discord.SetState("Stage Editor");

			base.Update(gameTime);
		}

		protected override void Draw(XNA_GameTime gameTime)
		{
			engine.Draw(gameTime);

			base.Draw(gameTime);
		}

		void OnAfterRenderGame()
		{
			using (new DrawBatch(transform: null))
			{
				state?.Draw();
			}
		}

		void OnBeforeRenderUI() { }

		void OnAfterRenderUI()
		{
			using (new DrawBatch(transform: null))
			{
				if (state is object)
				{
					state.DrawUI();
				}
				else
				{
					GFX.DrawBox(new Rect(8, 8, MGE.Window.renderSize.x / 2 - 16, MGE.Window.renderSize.y - 16), new Color(0, 0.75f));

					using (var layout = new MGE.UI.Layouts.StackLayout(new Vector2(24), 24, false))
					{
						Config.font.DrawText("--- MGE PARTY GAME ---", layout.newElement, Color.white);
						Config.font.DrawText($"Version: indev {System.DateTime.Now.ToString(@"yyyy-MM-dd")}", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText($"Current Stage: {GameSettings.stage.name}" + (changingStage ? string.Empty : " (F2 to change stage)"), layout.newElement, changingStage ? new Color("#FB2") : Color.white);
						layout.AddElement();
						Config.font.DrawText("--- MODES ---", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText("1 - Play Mode", layout.newElement, Color.white);
						Config.font.DrawText("2 - Edit Mode", layout.newElement, Color.white);
						Config.font.DrawText("3 - Main Menu", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText("~ - This Menu", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText("--- OPTIONS ---", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText($"Volume {SoundEffect.MasterVolume.ToString("P")} (- +)", layout.newElement, Color.white);
					}

					GFX.DrawBox(new Rect(MGE.Window.renderSize.x / 2 + 8, 8, MGE.Window.renderSize.x / 2 - 16, MGE.Window.renderSize.y - 16), new Color(0, 0.75f));

					using (var layout = new MGE.UI.Layouts.StackLayout(new Vector2(MGE.Window.renderSize.x / 2 + 24, 24), 24, false))
					{
						Config.font.DrawText("--- STAGES ---", layout.newElement, Color.white);
						layout.AddElement();
						foreach (var file in IO.FolderGetFiles("Assets/Stages"))
						{
							Config.font.DrawText("- " + file.Replace("Assets/Stages/", string.Empty).Replace(".stage", string.Empty), layout.newElement, Color.white);
						}
					}
				}
#if !INDEV
				Config.font.DrawText($"Version {System.Environment.Version} {System.DateTime.Now.ToString("yyyy-MM-dd")}", new Rect(16, MGE.Window.renderSize.y - 38, MGE.Window.renderSize.x, 32), new Color(1, 0.5f));
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