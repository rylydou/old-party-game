using MGE;
using MGE.ECS;
using MGE.InputSystem;
using GAME.World;
using GAME.World.Generation;
using MGE.UI;
using MGE.UI.Elements;

namespace GAME.Components
{
	public class CWorld : Component
	{
		public static Grid grid;
		public int currentTile = 1;
		public int brushSize = 1;
		public int brushFuzz = 3;

		public override void Init()
		{
			grid = new Grid(Window.gameSize, new GenTest());
		}

		public override void Update()
		{
			Vector2Int mousePos = grid.CamToTile(Input.cameraMousePosition);

			if (Input.GetButton(Inputs.LeftShift))
			{
				brushSize -= Input.scroll;
				brushSize = Math.Clamp(brushSize, 0, 50);
			}
			else if (Input.GetButton(Inputs.LeftAlt))
			{
				brushFuzz -= Input.scroll;
				brushFuzz = Math.Clamp(brushFuzz, 1, 10);
			}
			else
			{
				currentTile -= Input.scroll;

				if (currentTile < 1) currentTile = Grid.tiles.Count - 1;
				else if (currentTile >= Grid.tiles.Count) currentTile = 1;
			}

			if (Input.GetButton(Inputs.MouseLeft)) Paint(mousePos, currentTile);

			if (Input.GetButton(Inputs.MouseRight)) Paint(mousePos, 0);

			if (!Input.GetButton(Inputs.Escape))
				grid.Update();

			var pos = grid.CamToTile(Input.cameraMousePosition);

			GUI.AddElement(new GUIText(Grid.IDToTile(currentTile).name)
			{
				rect = new Rect(Input.windowMousePosition + new Vector2(16, -32), Vector2.zero)
			});
			GUI.AddElement(new GUIText((brushSize * 2 + 1).ToString())
			{
				rect = new Rect(Input.windowMousePosition + new Vector2(-32 - 16, -32), Vector2.zero)
			});
			GUI.AddElement(new GUIText(brushFuzz + "/10")
			{
				rect = new Rect(Input.windowMousePosition + new Vector2(-64 - 16, 0), Vector2.zero)
			});
		}

		public override void Draw()
		{
			grid.Draw();
		}

		public void Paint(Vector2Int position, int id)
		{
			for (int y = -brushSize; y <= brushSize; y++)
			{
				for (int x = -brushSize; x <= brushSize; x++)
				{
					if (Random.Bool(1.0 / (double)brushFuzz))
					{
						var pos = grid.CamToTile(Input.cameraMousePosition) + new Vector2Int(x, y);
						grid.SetTile(pos, Grid.IDToTile(id));
					}
				}
			}
		}
	}
}