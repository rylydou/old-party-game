using System;
using System.Collections.Generic;
using MGE.Debug;
using MGE.Debug.Menus;
using MGE.Graphics;
using MGE.UI;
using MGE.UI.Layouts;

namespace MGE.StageSystem.Layers
{
	[System.Serializable]
	public class IntLayer : StageLayer
	{
		public bool showInGame = false;

		public Grid<ushort> tiles;
		public List<Color> colors;

		[NonSerialized] public ushort selectedColor = 0;
		[NonSerialized] public float lastChanged = -1;

		protected override void Editor_Create()
		{
			name = "Int Grid";

			tiles = new Grid<ushort>(stage.size.x, stage.size.y);
			colors = new List<Color>() { Color.clear, Color.red, Color.green, Color.blue };
		}

		public override void Editor_Update(ref GUI gui)
		{
			using (var layout = new StackLayout(new Vector2(0, offset), itemSize, false))
			{
				gui.Toggle("Show In Game?", new Rect(layout.newElement, gui.rect.width, layout.currentSize), ref showInGame);

				ushort index = 0;
				var colorToRemove = -1;
				foreach (var color in colors)
				{
					var rect = new Rect(layout.newElement, gui.rect.width, layout.currentSize);

					switch (gui.ColoredButton(index.ToString(), rect, color, null, index == selectedColor))
					{
						case PointerInteraction.LClick:
							if (selectedColor == index && index > 0)
								Menuing.OpenMenu(new DMenuTextInput("Enter Color Hex Code...", colors[selectedColor].ToHex(6), (x) => colors[selectedColor] = new Color(x), null, TextFeildRule.colorCodeNoAlpha));
							else
								selectedColor = index;
							break;
						case PointerInteraction.MClick:
							colorToRemove = index;
							break;
					}

					index++;
				}

				if (colorToRemove > 0)
				{
					colors.RemoveAt(colorToRemove);
					tiles.ForEach((color) =>
					{
						if (color > colorToRemove)
							color--;
						else if (color == colorToRemove)
							color = 0;

						return color;
					});
				}

				if (gui.ButtonClicked("Add New Index", new Rect(layout.newElement, new Vector2(gui.rect.width, layout.currentSize))))
				{
					colors.Add(Random.Color());
				}
			}
		}

		public override void Editor_Draw(Vector2 pan, float zoom)
		{
			tiles.For((x, y, tile) =>
			{
				GFX.DrawBox(Scale(new Rect(x, y, 1, 1)), colors[tile]);
			});
		}

		public override void Game_Draw()
		{
			if (!showInGame) return;

			tiles.For((x, y, tile) =>
			{
				GFX.DrawBox(new Rect(x * stage.tileSize, y * stage.tileSize, stage.tileSize, stage.tileSize), colors[tile]);
			});
		}
	}
}