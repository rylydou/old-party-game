using System.Collections.Generic;
using GAME.UI;
using MGE;
using MGE.Graphics;
using MGE.Physics;

namespace GAME.States
{
	public class StateMainMenu : GameState
	{
		static string[] _backgrounds;
		public static string[] backgrounds
		{
			get
			{
				if (_backgrounds is null)
				{
					_backgrounds = Assets.GetUnloadedAssets("@ Menu Backgrounds");

					foreach (var background in _backgrounds)
						Logger.Log(background);
				}
				return _backgrounds;
			}
		}

		public static Menu mainMenu = new Menu(
			"Main Menu",
			(() => "Play", () => Main.current.ChangeState(new StatePlayerSetup())),
			(() => "Editor", () => Main.current.ChangeState(new StateEditor())),
			(() => "Settings", () => MenuManager.OpenMenu(settingsMenu)),
			(() => "Quit", () => Main.current.Exit())
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
		(() => (Main.current.infiniteTime ? "[X]" : "[ ]") + " Infinite Time", () => Main.current.infiniteTime = !Main.current.infiniteTime)
	);

		public static Menu graphicsSettingsMenu = new Menu(
			"Graphics Settings",
			(() => (GFX.graphics.SynchronizeWithVerticalRetrace ? "[X]" : "[ ]") + " V-Sync", () => GFX.graphics.SynchronizeWithVerticalRetrace = !GFX.graphics.SynchronizeWithVerticalRetrace),
			(() => (GFX.graphics.HardwareModeSwitch ? "[X]" : "[ ]") + " Hardware Fullscreen", () => GFX.graphics.HardwareModeSwitch = !GFX.graphics.HardwareModeSwitch),
			(() => "Apply Changes", () => GFX.graphics.ApplyChanges())
		);

		Texture background;

		public override void Init()
		{
			base.Init();

			background = Assets.LoadAsset<Texture>(backgrounds.Random());
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
				MenuManager.Update();

				if (MenuManager.menus.Count <= 0)
				{
					GameSettings.mainController = null;
				}
			}
		}

		public override void DrawUI()
		{
			base.DrawUI();

			if (background is object)
				GFX.Draw(background, new Rect(0, 0, Window.renderSize));

			if (GameSettings.mainController is null)
			{
				var pos = Window.renderSize.y - 256 + Math.Sin(Time.time * Math.pi) * 4;

				Config.font.DrawText("Press [Select] To Start", new Rect(0, pos + 2, Window.renderSize.x, 64), new Color(0, 0.25f), 1, TextAlignment.Center);
				Config.font.DrawText("Press [Select] To Start", new Rect(0, pos, Window.renderSize.x, 64), Color.white, 1, TextAlignment.Center);
			}
			else
			{
				MenuManager.Draw();
			}
		}
	}
}