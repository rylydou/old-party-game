using System.Collections.Generic;
using System.Linq;
using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.UI.Layouts;

namespace GAME.States
{
	public class StatePlayerSetup : GameState
	{
		Palette palette;

		Dictionary<EController, Texture> controllerToTex;

		float timeAllReadyToContinue = 1.0f;

		float timeAllReady;

		public override void Init()
		{
			base.Init();

			SceneManager.QueueScene(new Scene());

			if (GameSettings.current is null)
				GameSettings.current = new GameSettings();

			controllerToTex = new Dictionary<EController, Texture>();
			controllerToTex.Add(EController.ArrowKeys, Assets.GetAsset<Texture>("UI/Controllers/Arrow Keys"));
			controllerToTex.Add(EController.WASD, Assets.GetAsset<Texture>("UI/Controllers/WASD"));
			controllerToTex.Add(EController.Gamepad0, Assets.GetAsset<Texture>("UI/Controllers/Gamepad 0"));
			controllerToTex.Add(EController.Gamepad1, Assets.GetAsset<Texture>("UI/Controllers/Gamepad 1"));
			controllerToTex.Add(EController.Gamepad2, Assets.GetAsset<Texture>("UI/Controllers/Gamepad 2"));
			controllerToTex.Add(EController.Gamepad3, Assets.GetAsset<Texture>("UI/Controllers/Gamepad 3"));

			palette = Setup.palettes.Random();

			foreach (var player in GameSettings.current.players)
			{
				player.Reset();
			}
		}

		public override void Update()
		{
			base.Update();

			var readyPlayers = 0;
			foreach (var player in GameSettings.current.players.ToArray())
			{
				if (player.READY)
				{
					if (player.controls.back)
					{
						player.READY = false;
						PlaySound("UI/Sounds/Back");
					}

					readyPlayers++;
				}
				else
				{
					if (player.controls.select)
					{
						player.READY = true;

						PlaySound("UI/Sounds/Ready");
					}
					else if (player.controls.back)
					{
						GameSettings.current.players.Remove(player);

						PlaySound("UI/Sounds/Leave");
					}
					else if (player.controls.left)
					{
						var index = Setup.skins.FindIndex(x => x == player.skin) - 1;
						if (index < 0) index = Setup.skins.Count - 1;
						player.skin = Setup.skins[index];

						PlaySound("UI/Sounds/Change");
					}
					else if (player.controls.right)
					{
						var index = Setup.skins.FindIndex(x => x == player.skin) + 1;
						if (index >= Setup.skins.Count) index = 0;
						player.skin = Setup.skins[index];

						PlaySound("UI/Sounds/Change");
					}
				}
			}

			foreach (var controller in GameSettings.current.controllers)
			{
				if (controller.Value.select)
				{
					if (!GameSettings.current.players.Any(x => (EController)x.index == controller.Key))
					{
						GameSettings.current.players.Add(new Player((sbyte)controller.Key, Setup.skins.Random()));

						PlaySound("UI/Sounds/Join");
					}
				}
			}

			if (GameSettings.current.players.Count > 0 && readyPlayers >= GameSettings.current.players.Count)
			{
				timeAllReady += Time.deltaTime;
			}
			else
				timeAllReady = 0;

			if (timeAllReady > timeAllReadyToContinue)
				Main.current.ChangeState(new StatePlaying());
		}

		public override void DrawUI()
		{
			base.DrawUI();

			GFX.DrawBox(new Rect(0, 0, Window.renderSize), palette.backgroundA);

			const int res = 2;

			var lastPos = Vector2.zero;
			for (int x = -32; x < Window.renderSize.x + 32; x += res)
			{
				var pos = new Vector2(x, Math.Sin((float)x / Window.renderSize.x * 2 + Time.time * 0.5f) * Window.renderSize.y / 3 + Window.renderSize.y / 2);
				GFX.DrawLine(lastPos, pos, palette.backgroundB, 64);
				lastPos = pos;
			}

			lastPos = Vector2.zero;
			for (int x = -32; x < Window.renderSize.x + 32; x += res)
			{
				var pos = new Vector2(x, Math.Sin((float)x / Window.renderSize.x * 2 + Time.time * 0.5f + Math.pi) * Window.renderSize.y / 3 + Window.renderSize.y / 2);
				GFX.DrawLine(lastPos, pos, palette.backgroundB, 64);
				lastPos = pos;
			}

			const int barSize = 64;
			for (int i = -1; i < Window.renderSize.x / barSize + 4; i++)
			{
				if ((i + 1) % 3 == 0)
					GFX.DrawBox(new Rect(i * barSize + Math.Wrap(Time.time * 32, -barSize * 1.5f, barSize * 1.5f), -barSize / 4, barSize, Window.renderSize.y + barSize / 2), palette.backgroundB, Math.pi / 16);
			}

			GFX.DrawBox(new Rect(0, 0, Window.renderSize.x, Window.renderSize.y * (timeAllReady / timeAllReadyToContinue)), palette.backgroundA);

			using (var layout = new StackLayout(new Vector2(32), 256, true))
			{
				foreach (var player in GameSettings.current.players)
				{
					if (player is null) continue;

					layout.AddElement();

					GFX.DrawBox(new Rect(layout.currentElement + 4, layout.currentSize + 32), new Color(0, 0.25f));

					GFX.DrawBox(new Rect(layout.currentElement - 16, layout.currentSize + 32), player.color);

					if (!player.READY)
						GFX.DrawBox(new Rect(layout.currentElement, layout.currentSize), new Color(0, 0.75f));

					for (int y = -8; y <= 8; y++)
						for (int x = -8; x <= 8; x++)
							GFX.Draw(player.icon, new Rect(layout.currentElement + 32 + new Vector2(x, y), layout.currentSize - 64), Color.black);

					GFX.Draw(player.icon, new Rect(layout.currentElement + 32, layout.currentSize - 64), Color.white);

					if (!player.READY)
						GFX.Draw(controllerToTex[(EController)player.index], new Rect(layout.currentElement + 16, 64), player.color);

					layout.AddElement(64);
				}
			}
		}
	}
}