using GAME.States;
using MGE;
using MGE.Graphics;
using MGE.FileIO;
using XNA_Game = Microsoft.Xna.Framework.Game;
using XNA_GameTime = Microsoft.Xna.Framework.GameTime;
using MGE.InputSystem;

namespace GAME
{
	public class Main : XNA_Game
	{
		public static Main current { get; private set; }

		public Engine engine;

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

			Pointer.mode = PointerMode.System;

			Microsoft.Xna.Framework.Audio.SoundEffect.MasterVolume = 0.33f;

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
			Window.Title = $"MGE Party Game | {Math.Round(Stats.fps)}fps {Math.Round(Stats.averageFps)}avg {Math.Round(Stats.minFps)}min | {Math.Round(Stats.memUsedAsMBs, 1)}MB / {Math.Round(Stats.memAllocatedAsMBs)}MB";

			if (Input.GetButtonPress(Inputs.Tilde))
				ChangeState(null);
			else if (Input.GetButton(Inputs.LeftControl) && Input.GetButtonPress(Inputs.S))
				GameSettings.current.stage.Save();
			else if (Input.GetButton(Inputs.LeftControl) && Input.GetButtonPress(Inputs.L))
				GameSettings.current.stage = Stage.Load(GameSettings.current.stage.name);

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
						switch (key)
						{
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
								GameSettings.current.stage.name += key;
								break;
						}
					}
				}
				else
				{
					if (Input.GetButtonPress(Inputs.D1))
						ChangeState(new StatePlaying());
					else if (Input.GetButtonPress(Inputs.D2))
						ChangeState(new StateEditor());
				}

				if (Input.GetButtonPress(Inputs.F2))
					changingStage = true;
			}

			engine.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(XNA_GameTime gameTime)
		{
			engine.Draw(gameTime);

			using (new DrawBatch(transform: null))
			{
				if (state is object)
				{
					state.Draw();
				}
				else
				{
					GFX.DrawBox(new Rect(0, 0, MGE.Window.renderSize), new Color(0, 0.9f));

					using (var layout = new MGE.UI.Layouts.StackLayout(new Vector2(16), 24, false))
					{
						Config.font.DrawText($"Current Stage: {GameSettings.current.stage.name}" + (changingStage ? string.Empty : " (F2 to change stage)"), layout.newElement, changingStage ? new Color("#FB2") : Color.white);
						layout.AddElement();
						Config.font.DrawText("--- MODES ---", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText("1 - Enter Play Mode", layout.newElement, Color.white);
						Config.font.DrawText("2 - Enter Edit Mode", layout.newElement, Color.white);
						Config.font.DrawText("~ - This Menu", layout.newElement, Color.white);
						layout.AddElement();
						Config.font.DrawText("--- AVAILABLE STAGES ---", layout.newElement, Color.white);
						foreach (var file in IO.FolderGetFiles("Assets/Stages"))
						{
							Config.font.DrawText("- " + file.Replace("Assets/Stages/", string.Empty).Replace(".stage", string.Empty), layout.newElement, Color.white);
						}
					}
				}
			}

			base.Draw(gameTime);
		}

		public void ChangeState(GameState state)
		{
			this.state?.Exit();
			this.state = null;

			System.GC.Collect();

			this.state = state;
			this.state?.Init();
		}
	}
}