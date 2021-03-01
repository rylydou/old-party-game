using MGE;
using MGE.ECS;
using MGE.InputSystem;
using GAME.World;
using GAME.World.Generation;
using MGE.UI;
using MGE.UI.Elements;
using MGE.Graphics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GAME.Components
{
	public class CWorld : Component
	{
		const int uiItemSize = 32;
		const int uiItemPadding = 8;
		const int maxBrushFuzz = 10;

		public static Grid grid;

		int currentTile = 1;
		int brushSize = 1;
		int brushFuzz = 3;

		Vector2Int mousePos = Vector2.zero;

		public override void Init()
		{
			grid = new Grid(Window.gameSize, new GenTest());
		}

		public override void Update()
		{
			mousePos = grid.CamToTile(Input.cameraMousePosition);
			bool inMenu = Input.windowMousePosition.y < 32;

			if (Input.GetButton(Inputs.LeftShift))
			{
				brushSize -= Input.scroll;
				brushSize = Math.Clamp(brushSize, 0, 50);
			}
			else if (Input.GetButton(Inputs.LeftAlt))
			{
				brushFuzz += Input.scroll;
				brushFuzz = Math.Clamp(brushFuzz, 1, maxBrushFuzz);
			}
			else
			{
				currentTile -= Input.scroll;

				if (currentTile < 1) currentTile = Grid.tiles.Count - 1;
				else if (currentTile >= Grid.tiles.Count) currentTile = 1;
			}

			if (Input.GetButtonPress(Inputs.F3))
				grid.debugEnabled = !grid.debugEnabled;

			if (!inMenu)
			{
				if (Input.GetButton(Inputs.MouseLeft)) Paint(mousePos, currentTile);
				else if (Input.GetButton(Inputs.MouseRight)) Paint(mousePos, 0);
				else if (Input.GetButton(Inputs.MouseMiddle))
				{
					for (int y = -brushSize; y <= brushSize; y++)
					{
						for (int x = -brushSize; x <= brushSize; x++)
						{
							var pos = mousePos + new Vector2Int(x, y);
							grid.UpdateArea(pos);
						}
					}
				}
			}

			if (!Input.GetButton(Inputs.Escape))
				grid.Update();

			int invOffset = Window.windowedSize.x / 2 - (Grid.tiles.Count) * uiItemSize / 2;
			var block = Grid.IDToTile(currentTile);

			for (int i = 1; i < Grid.tiles.Count; i++)
			{
				var rect = new Rect((i - 1) * uiItemSize + invOffset, 0, uiItemSize, uiItemSize);

				if (GUI.MouseInside(rect))
				{
					block = Grid.IDToTile(i);
					GUI.Image(rect, new Color("#FF7504"));
				}

				if (currentTile == i)
					GUI.Image(rect, new Color("#FF7504"));

				GUI.Image(new Rect((i - 1) * uiItemSize + uiItemPadding / 2 + invOffset, uiItemPadding / 2, uiItemSize - uiItemPadding, uiItemSize - uiItemPadding), Grid.IDToTile(i).color.opaque);

				if (GUI.MouseClick(rect))
					currentTile = i;
			}

			var textOffset = new Vector2(Window.windowedSize.x / 2 - block.name.Length * 16 / 2, 42);

			GUI.Text(block.name, textOffset + Vector2.one * 2, new Color(0, 0.1f));
			GUI.Text(block.name, textOffset, block.color.opaque);
			// GUI.Text(grid.GetTile(mousePos).velocity.ToString(), Input.windowMousePosition, block.color.opaque);
		}

		public override void Draw()
		{
			grid.Draw();

			var actualBrushSize = new Vector2Int(brushSize * 2 + 1);
			var color = Color.Lerp(new Color(1, 0, 0, 0.1f), new Color(1, 0, 0, 0.25f), 1 - (float)brushFuzz / maxBrushFuzz);

			using (new DrawBatch())
			{
				var rect = new Rect(mousePos - actualBrushSize / 2, actualBrushSize);

				// GFX.DrawBox(rect, color);
				GFX.DrawRectangle(rect, Color.red, 1);
			}
		}

		public void Paint(Vector2Int position, int id)
		{
			for (int y = -brushSize; y <= brushSize; y++)
			{
				for (int x = -brushSize; x <= brushSize; x++)
				{
					if (Random.Bool(1.0 / (double)brushFuzz))
					{
						var pos = mousePos + new Vector2Int(x, y);
						grid.SetTile(pos, (Tile)System.Activator.CreateInstance(Grid.IDToTile(id).GetType()));
					}
				}
			}
		}
	}
}