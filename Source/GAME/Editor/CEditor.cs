using MGE;
using MGE.ECS;
using MGE.FileIO;
using MGE.Graphics;
using MGE.InputSystem;

namespace GAME.Editor
{
	public class CEditor : Component
	{
		byte selectedTile;
		Vector2Int cursorPos;

		public override void Init()
		{
			base.Init();
		}

		public override void Update()
		{
			base.Update();

			cursorPos = (Vector2Int)Input.cameraMousePosition;

			selectedTile = (byte)Math.Clamp(selectedTile - Input.scroll, 1, Stage.tilesets.Count - 1);

			if (Input.GetButton(Inputs.LeftControl))
			{
				if (Input.GetButtonPress(Inputs.S))
				{
					IO.Save($"Assets/Stages/{GameSettings.current.stage.name}.stage", GameSettings.current.stage, false);
				}
				else if (Input.GetButtonPress(Inputs.L))
				{
					GameSettings.current.stage = IO.Load<Stage>($"Assets/Stages/Untitled Stage.stage", false);
				}
			}
			else
			{
				if (Input.GetButton(Inputs.MouseLeft))
					GameSettings.current.stage.tiles.Set(cursorPos, selectedTile);
				else if (Input.GetButton(Inputs.MouseRight))
					GameSettings.current.stage.tiles.Set(cursorPos, 0);
			}
		}

		public override void Draw()
		{
			base.Draw();

			GFX.DrawRect(new Rect(cursorPos, 1, 1), Color.red);
			Config.font.DrawText(selectedTile.ToString(), (Vector2)cursorPos + new Vector2(1, -1), Color.red, 1f / 32);
		}
	}
}