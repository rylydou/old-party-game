using System.Collections.Generic;
using System.Linq;
using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.UI;
using MGE.UI.Layouts;

namespace GAME.States
{
	public class StatePlayerSetup : GameState
	{
		static readonly Color noPlayerA = new Color("#111");
		static readonly Color noPlayerB = new Color("#1A1A1A");

		float timeAllReadyToContinue = 1.0f;

		float timeAllReady;

		Texture playerShadow;
		Dictionary<EController, Texture> controllerToTex;

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

			playerShadow = Assets.GetAsset<Texture>("UI/Player Shadow");

			foreach (var player in GameSettings.players)
			{
				player.Reset();
			}
		}

		public override void Update()
		{
			base.Update();

			var readyPlayers = 0;
			foreach (var player in GameSettings.players.ToArray())
			{
				if (player.READY)
				{
					if (player.controls.back)
					{
						player.READY = false;
						PlaySound("UI/Sounds/Unready");
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
						GameSettings.players.Remove(player);

						foreach (var playerToUpdate in GameSettings.players)
						{
							playerToUpdate._color = null;
						}

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

			foreach (var controller in GameSettings.controllers)
			{
				if (controller.select)
				{
					if (!GameSettings.players.Any(x => x.controls.id == controller.id))
					{
						GameSettings.players.Add(new Player(controller.id, Setup.skins.Random()));

						PlaySound("UI/Sounds/Join");
					}
				}
			}

			if (GameSettings.players.Count > 0 && readyPlayers >= GameSettings.players.Count)
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

			var index = 0;

			using (var layout = new StackLayout(Vector2.zero, GUI.canvasSize.x / 4, true))
			{
				for (int i = 0; i < 4; i++)
				{
					index++;

					layout.AddElement();

					GFX.DrawBox(new Rect(layout.currentElement, layout.currentSize, GUI.canvasSize.y), index % 2 == 0 ? noPlayerA : noPlayerB);
				}
			}

			using (var layout = new StackLayout(Vector2.zero, GUI.canvasSize.x / 4, true))
			{
				foreach (var player in GameSettings.players)
				{
					layout.AddElement();

					GFX.DrawBox(new Rect(layout.currentElement, layout.currentSize, GUI.canvasSize.y), player.color);
					if (!player.READY)
						GFX.DrawBox(new Rect(layout.currentElement + 16, layout.currentSize - 32, GUI.canvasSize.y - 32), new Color(0, 0.5f));

					GFX.Draw(playerShadow, new Rect(layout.currentElement + 128 + new Vector2(0, 128), layout.currentSize - 256), new Color(0, 0.25f));
					GFX.Draw(player.icon, new Rect(layout.currentElement + 128, layout.currentSize - 256), Color.white);

					if (!player.READY)
						GFX.Draw(controllerToTex[player.controls.id], new Rect(layout.currentElement + 32, 128), player.color);
				}
			}
		}
	}
}