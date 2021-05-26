using System.Collections.Generic;
using GAME.UI;
using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.Physics;
using MGE.FileIO;
using MGE.InputSystem;
using System;
using Math = MGE.Math;

namespace GAME.States
{
	public class StateMainMenu : GameState
	{
		const float backgroundLifetime = 6.0f;
		const float backgroundTimeLeftToStartFading = 2.0f;

		static string[] _backgrounds;
		public static string[] backgrounds
		{
			get
			{
				if (_backgrounds is null)
					_backgrounds = Assets.GetUnloadedAssets<Texture>("@ Menu Backgrounds");
				return _backgrounds;
			}
		}

		public static Menu mainMenu = new Menu(
			string.Empty,
			(() => "Play", () => Main.current.ChangeState(new StatePlayerSetup())),
			(() => "Editor", () => MenuManager.OpenMenu(editorMenu)),
			(() => "Settings", () => MenuManager.OpenMenu(settingsMenu)),
			(() => "Quit", () => Main.current.Exit())
		);

		public static Menu editorMenu = new Menu(
			"Editor",
			(() => "Edit", () => Main.current.ChangeState(new StateEditor())),
			(() => "Save", () => GameSettings.stage.Save()),
			(() => "Load", () => MenuManager.OpenMenu(MakeMenuOnStages(x => GameSettings.stage = Assets.LoadAsset<Stage>(x)))),
			(() => "Rename", () => isEnteringInput = true)
		);

		public static Menu settingsMenu = new Menu(
			"Settings",
			(() => "Game Settings", () => MenuManager.OpenMenu(gameSettingsMenu)),
			(() => "Graphics Settings", () => MenuManager.OpenMenu(graphicsSettingsMenu)),
			(() => (Logger.collectErrors ? "[X]" : "[ ]") + " Collect Errors", () => Logger.collectErrors = !Logger.collectErrors),
			(() => (Physics.DEBUG ? "[X]" : "[ ]") + " Debug Physics", () => Physics.DEBUG = !Physics.DEBUG)
		);

		public static Menu gameSettingsMenu = new Menu(
		"Game Settings",
		(() => (GameSettings.current.infiniteTime ? "[X]" : "[ ]") + " Infinite Time", () => GameSettings.current.infiniteTime = !GameSettings.current.infiniteTime)
	);

		public static Menu graphicsSettingsMenu = new Menu(
			"Graphics Settings",
			(() => (Settings.Get<bool>("V-Sync", true) ? "[X]" : "[ ]") + " V-Sync", () => Settings.Toggle("V-Sync", true)),
			(() => (Settings.Get<bool>("Hardware Fullscreen", false) ? "[X]" : "[ ]") + " Hardware Fullscreen", () => Settings.Toggle("Hardware Fullscreen", false)),
			(() => (Settings.Get<bool>("SMAA", true) ? "[X]" : "[ ]") + " SMAA", () => Settings.Toggle("SMAA", true)),
			(() => "Apply", () => GFX.Apply())
		);

		static bool isEnteringInput = false;

		float backgroundTimeLeft = backgroundLifetime;

		Texture prevBackground;
		Texture background;

		public static Menu MakeMenuOnStages(Action<string> onStageSelected)
		{
			var items = new List<(Func<string>, Action)>();

			foreach (var item in Assets.GetUnloadedAssets<Stage>("@ Stages"))
			{
				var text = item.Replace("@ Stages/", string.Empty);
				var path = item;
				items.Add((() => text, () => { onStageSelected.Invoke(path); MenuManager.GoBack(); }));
			}

			return new Menu("Select A Stage", items.ToArray());
		}

		public override void Init()
		{
			base.Init();

			SceneManager.QueueScene(new Scene());

			MenuManager.Init();
		}

		public override void Update()
		{
			base.Update();

			if (GameSettings.mainController is null)
			{
				foreach (var controller in GameSettings.controllers)
				{
					if (controller.select)
					{
						GameSettings.mainController = controller;
						MenuManager.menus = new List<Menu>() { mainMenu };
					}
				}
			}
			else
			{
				if (isEnteringInput)
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
									isEnteringInput = false;
									break;
								case '\n':
									isEnteringInput = false;
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
					MenuManager.Update();

					if (MenuManager.menus.Count <= 0)
					{
						GameSettings.mainController = null;
					}
				}
			}

			backgroundTimeLeft += Time.deltaTime;

			if (backgroundTimeLeft > backgroundLifetime)
			{
				prevBackground?.texture?.Dispose();
				prevBackground = background;
				background = Assets.LoadAsset<Texture>(backgrounds.Random());
				backgroundTimeLeft = 0;
			}
		}

		public override void DrawUI()
		{
			base.DrawUI();

			if (prevBackground is object)
				GFX.Draw(prevBackground, new Rect(0, 0, Window.renderSize));

			if (background is object)
				GFX.Draw(background, new Rect(0, 0, Window.renderSize), new Color(1, Math.Clamp01(Math.Clamp(backgroundTimeLeft, 0, backgroundTimeLeftToStartFading) / backgroundTimeLeftToStartFading)));

			if (GameSettings.mainController is null)
			{
				var pos = Window.renderSize.y - 256 + Math.Sin(Time.time * Math.pi2) * 4;

				const string text = "Press [Select] To Start";

				for (int y = -4; y <= 4; y++)
					for (int x = -4; x <= 4; x++)
						Config.font.DrawText(text, new Rect(x, pos + y, Window.renderSize.x, 64), new Color(0, 0.0125f), 1.5f, TextAlignment.Center);

				Config.font.DrawText(text, new Rect(0, pos, Window.renderSize.x, 64), Color.white, 1.5f, TextAlignment.Center);
			}
			else
			{
				if (isEnteringInput)
				{
					var text = $"> {GameSettings.stage.name} <";

					Config.font.DrawText(text, new Rect(4, Window.renderSize.y / 2 - 32 + 4, Window.renderSize.x, 64), new Color(0, 0.25f), 1.5f, TextAlignment.Center);
					Config.font.DrawText(text, new Rect(0, Window.renderSize.y / 2 - 32, Window.renderSize.x, 64), Color.white, 1.5f, TextAlignment.Center);
				}
				else
				{
					MenuManager.Draw();
				}
			}
		}
	}
}