using GAME.Components;
using MGE;
using MGE.ECS;
using MGE.FileIO;
using MGE.InputSystem;

namespace GAME.States
{
	public class StateEditor : GameState
	{
		public byte selectedTile = 1;

		public override void Init()
		{
			base.Init();

			SceneManager.QueueScene(
				new Scene(
					new Layer(
						false,
						new Entity(new CBackground())
					),
					new Layer(
						false,
						new Entity(new CStage())
					)
				)
			);
		}

		public override void Update()
		{
			base.Update();

			selectedTile = (byte)Math.Clamp(selectedTile - Input.scroll, 1, Stage.tilesets.Count - 1);

			var shift = Input.GetButton(Inputs.LeftShift) | Input.GetButton(Inputs.RightShift);
			var ctrl = Input.GetButton(Inputs.LeftControl) | Input.GetButton(Inputs.RightControl);
			var alt = Input.GetButton(Inputs.LeftAlt) | Input.GetButton(Inputs.RightAlt);

			if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
				GameSettings.current.stage.tiles.Set((Vector2Int)Input.cameraMousePosition, selectedTile);
			else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
				GameSettings.current.stage.tiles.Set((Vector2Int)Input.cameraMousePosition, 0);
		}

		public override void Draw()
		{
			base.Draw();

			Config.font.DrawText(GameSettings.current.stage.name, new Vector2(16 + 2), new Color(0, 0.1f));
			Config.font.DrawText(GameSettings.current.stage.name, new Vector2(16), Color.white);

			Config.font.DrawText(selectedTile.ToString(), Input.windowMousePosition + new Vector2(16 + 2, -32 + 2), new Color(0, 0.1f));
			Config.font.DrawText(selectedTile.ToString(), Input.windowMousePosition + new Vector2(16, -32), new Color("#FB3"));
		}
	}
}