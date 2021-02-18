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
		public int currentTile;

		public override void Init()
		{
			grid = new Grid(new Vector2Int(64 * 4, 64 * 2), new GenTest());
		}

		public override void Update()
		{
			Vector2Int mousePos = grid.CamToTile(Input.cameraMousePosition);

			currentTile += Input.scroll;

			if (currentTile < 0) currentTile = Grid.tiles.Count - 1;
			else if (currentTile >= Grid.tiles.Count) currentTile = 0;

			if (Input.GetButton(Inputs.MouseLeft)) Paint(mousePos, currentTile);

			if (Input.GetButton(Inputs.MouseRight)) Paint(mousePos, 0);

			if (!Input.GetButton(Inputs.Space))
				grid.Update();

			var pos = grid.CamToTile(Input.cameraMousePosition);

			GUI.AddElement(new GUIText(Grid.IDToTile(currentTile).name)
			{
				rect = new Rect(Input.windowMousePosition + new Vector2(16, -16), Vector2.zero)
			});
		}

		public override void Draw()
		{
			grid.Draw();
		}

		public void Paint(Vector2Int position, int id)
		{
			for (int y = -5; y <= 5; y++)
			{
				for (int x = -5; x <= 5; x++)
				{
					grid.SetTileID(grid.CamToTile(Input.cameraMousePosition) + new Vector2Int(x, y), id);
				}
			}
		}
	}
}