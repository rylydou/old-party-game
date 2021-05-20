using System.Collections.Generic;
using GAME.UI;
using MGE;
using MGE.Graphics;
using MGE.Physics;

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
			(() => (Settings.Get<bool>("V-Sync", true) ? "[X]" : "[ ]") + " V-Sync", () => Settings.Toggle("V-Sync", true)),
			(() => (Settings.Get<bool>("Hardware Fullscreen", false) ? "[X]" : "[ ]") + " Hardware Fullscreen", () => Settings.Toggle("Hardware Fullscreen", false)),
			(() => (Settings.Get<bool>("SMAA", true) ? "[X]" : "[ ]") + " SMAA", () => Settings.Toggle("SMAA", true)),
			(() => "Apply", () => GFX.Apply())
		);

		float backgroundTimeLeft = backgroundLifetime;

		Texture prevBackground;
		Texture background;
		Texture gradHor;

		public override void Init()
		{
			base.Init();

			gradHor = Assets.GetAsset<Texture>("UI/Gradient Horizontal");
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

			var opacity = Math.Clamp(backgroundTimeLeft, 0, backgroundTimeLeftToStartFading) / backgroundTimeLeftToStartFading;

			if (background is object)
				GFX.Draw(background, new Rect(0, 0, Window.renderSize), new Color(1, Math.Clamp01(opacity)));

			if (GameSettings.mainController is null)
			{
				var pos = Window.renderSize.y - 256 + Math.Sin(Time.time * Math.pi2) * 4;

				const string text = "Press [Select] To Start";

				for (int y = -4; y <= 4; y++)
					for (int x = -4; x <= 4; x++)
						Config.font.DrawText(text, new Rect(x, pos + y, Window.renderSize.x, 64), new Color(0, 0.025f), 1.5f, TextAlignment.Center);

				Config.font.DrawText(text, new Rect(0, pos, Window.renderSize.x, 64), Color.white, 1.5f, TextAlignment.Center);
			}
			else
			{
				GFX.Draw(gradHor, new Rect(0, 0, Window.renderSize.x / 2, Window.renderSize.y), new Color(0, 0.5f));

				MenuManager.Draw();
			}
		}
	}
}