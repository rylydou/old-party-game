using System;
using System.Collections.Generic;
using MGE.Graphics;
using MGE.UI;
using MGE.UI.Layouts;

namespace MGE.StageSystem.Layers
{
	[System.Serializable]
	public class IntLayer : StageLayer
	{
		public Grid<ushort> tiles;
		public List<Color> colors;

		[NonSerialized] public ushort selectedColor = 0;

		protected override void Editor_Init()
		{
			name = "Int Grid";

			tiles = new Grid<ushort>(stage.size.x, stage.size.y);
			colors = new List<Color>() { Color.clear, Color.red, Color.green, Color.blue };
		}

		public override void Editor_Update(ref GUI gui)
		{
			using (var layout = new StackLayout(new Vector2(0, offset), itemSize, false))
			{
				ushort index = 0;
				var colorToRemove = -1;
				foreach (var color in colors)
				{
					var rect = new Rect(layout.newElement, new Vector2(gui.rect.width, layout.currentSize));

					switch (gui.ColoredButton(index.ToString(), rect, color, null, index == selectedColor))
					{
						case PointerInteraction.LClick:
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
	}
}