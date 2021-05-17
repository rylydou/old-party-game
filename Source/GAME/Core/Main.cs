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

		public bool NO_TIMMER = true;

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
				GameSettings.current.stage = Stage.Load("Test");
			}
			catch (System.Exception e)
			{
				Logger.Log(e);
				GameSettings.current.stage = new Stage(new Vector2Int(40, 23));
			}

			SoundEffect.MasterVolume = 0.1f;

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
				GameSettings.current.stage.Save();
			else if (!shift && ctrl && !alt && Input.GetButtonPress(Inputs.L))
				GameSettings.current.stage = Stage.Load(GameSettings.current.stage.name);
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Tilde))
				ChangeState(null);
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Plus))
				SoundEffect.MasterVolume = Math.Clamp01(SoundEffect.MasterVolume + 0.1f);
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Minus))
				SoundEffect.MasterVolume = Math.Clamp01(SoundEffect.MasterVolume - 0.1f);

			if (state is object)
			{
				foreach (var controller in GameSettings.current.controllers.Values)
				{
					controller.Update();
				}

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
									GameSettings.current.stage.name += ' ';
									break;
								case (char)13:
									changingStage = false;
									break;
								case '\n':
									changingStage = false;
									break;
								case '\b':
									if (GameSettings.current.stage.name.Length > 0)
										GameSettings.current.stage.name = GameSettings.current.stage.name.Remove(GameSettings.current.stage.name.Length - 1, 1);
									break;
								default:
									break;
							}
						}
						else
						{
							GameSettings.current.stage.name += key;
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
						ChangeState(new StatePlayerSetup());
					else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D4))
						ChangeState(new StateTileEditor());
					else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.T))
						NO_TIMMER = !NO_TIMMER;
					else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.L))
						Logger.collectErrors = !Logger.collectErrors;
					else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.P))
						Physics.DEBUG = !Physics.DEBUG;
				}

				if (Input.GetButtonPress(Inputs.F2))
					changingStage = !changingStage;
			}

			engine.Update(gameTime);

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
						Config.font.DrawText($"Current Stage: {GameSettings.current.stage.name}" + (changingStage ? string.Empty : " (F2 to change stage)"), layout.newElement, changingStage ? new Color("#FB2") : Color.white);
						layout.AddElement();
						Config.font.DrawText("--- MODES ---", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText("1 - Play Mode", layout.newElement, Color.white);
						Config.font.DrawText("2 - Edit Mode", layout.newElement, Color.white);
						Config.font.DrawText("3 - Player Setup", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText("~ - This Menu", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText("--- OPTIONS ---", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText($"Volume {SoundEffect.MasterVolume.ToString("P")} (- +)", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText((NO_TIMMER ? "[X]" : "[ ]") + " No Timmer? (T)", layout.newElement, Color.white);
						Config.font.DrawText((Logger.collectErrors ? "[X]" : "[ ]") + " Collect Errors? (L)", layout.newElement, Color.white);
						Config.font.DrawText((Physics.DEBUG ? "[X]" : "[ ]") + " Debug Physics? (P)", layout.newElement, Color.white);
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
			}
		}

		public void ChangeState(GameState state)
		{
			this.state?.Exit();
			this.state = null;

			System.GC.Collect();

			this.state = state;
			this.state?.Init();

			if (SceneManager.activeScene is object)
				SceneManager.activeScene.clearScreen = false;
		}
	}
}