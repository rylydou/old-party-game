using System.Collections.Generic;
using System.Linq;
using MGE;
using MGE.Graphics;
using MGE.UI.Layouts;

namespace GAME.States
{
	public class StatePlayerSetup : GameState
	{
		Dictionary<EController, Texture> controllerToTex;

		float timeAllReadyToContinue = 3.0f;

		float timeAllReady;

		public override void Init()
		{
			base.Init();

			if (GameSettings.current is null)
				GameSettings.current = new GameSettings();

			controllerToTex = new Dictionary<EController, Texture>();
			controllerToTex.Add(EController.ArrowKeys, Assets.GetAsset<Texture>("UI/Controllers/Arrow Keys"));
			controllerToTex.Add(EController.WASD, Assets.GetAsset<Texture>("UI/Controllers/WASD"));
			controllerToTex.Add(EController.Gamepad0, Assets.GetAsset<Texture>("UI/Controllers/Gamepad 0"));
			controllerToTex.Add(EController.Gamepad1, Assets.GetAsset<Texture>("UI/Controllers/Gamepad 1"));
			controllerToTex.Add(EController.Gamepad2, Assets.GetAsset<Texture>("UI/Controllers/Gamepad 2"));
			controllerToTex.Add(EController.Gamepad3, Assets.GetAsset<Texture>("UI/Controllers/Gamepad 3"));

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
					if (player.controls.leave)
					{
						player.READY = false;
					}

					readyPlayers++;
				}
				else
				{
					if (player.controls.select)
					{
						player.READY = true;
					}
					else if (player.controls.leave)
					{
						GameSettings.current.players.Remove(player);
					}
					else if (player.controls.left)
					{
						var index = Setup.skins.FindIndex(x => x == player.skin) - 1;
						if (index < 0) index = Setup.skins.Count - 1;
						player.skin = Setup.skins[index];
					}
					else if (player.controls.right)
					{
						var index = Setup.skins.FindIndex(x => x == player.skin) + 1;
						if (index >= Setup.skins.Count - 1) index = 0;
						player.skin = Setup.skins[index];
					}
				}
			}

			foreach (var controller in GameSettings.current.controllers)
			{
				if (controller.Value.select)
				{
					if (!GameSettings.current.players.Any(x => (EController)x.index == controller.Key))
					{
						GameSettings.current.players.Add(new Player((sbyte)controller.Key));
					}
				}
			}

			if (readyPlayers >= GameSettings.current.players.Count)
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

			GFX.DrawBox(new Rect(0, 0, Window.renderSize), Color.Lerp(new Color("#222"), Color.green, timeAllReady / timeAllReadyToContinue));

			using (var layout = new StackLayout(new Vector2(32), 256, true))
			{
				foreach (var player in GameSettings.current.players)
				{
					if (player is null) continue;

					GFX.DrawBox(new Rect(layout.newElement, layout.currentSize), player.READY ? Color.green : Color.black);

					GFX.Draw(player.icon, new Rect(layout.currentElement + 32, layout.currentSize - 64), Color.white);

					GFX.Draw(controllerToTex[(EController)player.index], new Rect(layout.currentElement + 16, 64), player.color);

					layout.AddElement(64);
				}
			}
		}
	}
}