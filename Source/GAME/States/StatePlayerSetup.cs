using System.Collections.Generic;
using MGE;
using MGE.Graphics;

namespace GAME.States
{
	public class StatePlayerSetup : GameState
	{
		public Dictionary<EController, Texture> controllerToTex;

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
		}

		public override void Update()
		{
			base.Update();
		}

		public override void Draw()
		{
			base.Draw();

			GFX.DrawBox(new Rect(0, 0, Window.renderSize), new Color("#3D405B"));

			var index = 0;

			const float iconSize = 256;
			const float iconToIconMargin = 256 + 32;
			const float iconMargin = 128;

			foreach (var player in GameSettings.current.players)
			{
				GFX.Draw(player.icon, new Rect(index * iconToIconMargin + iconMargin, iconMargin, iconSize, iconSize));
				GFX.Draw(controllerToTex[(EController)player.controls.index], new Rect(index * iconToIconMargin + iconSize / 2, iconMargin + iconSize / 2, iconSize / 2, iconSize / 2), player.color);

				index++;
			}

			const float controllerCursorSize = 64;

			foreach (var controller in GameSettings.current.controllers)
			{
				GFX.Draw(controllerToTex[controller.Key], new Rect(controller.Value.cursorPos, controllerCursorSize, controllerCursorSize));

				index++;
			}
		}
	}
}