using System;
using System.Collections.Generic;
using MGE.Components;
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

		[NonSerialized] public ushort colorIndex = 0;

		protected override void OnInit(Vector2Int size)
		{
			name = "Int Grid";

			tiles = new Grid<ushort>(size.x, size.y);
			colors = new List<Color>() { Color.clear, Color.red, Color.green, Color.blue };
		}

		public override void Update(ref GUI gui)
		{
			using (var layout = new StackLayout(new Vector2Int(0, 8), 32, false))
			{
				ushort index = 0;
				var colorToRemove = -1;
				foreach (var color in colors)
				{
					var rect = new Rect(layout.newElement, new Vector2(gui.rect.width, layout.currentSize));

					gui.Image(rect, color);

					var interaction = gui.MouseInteraction(rect);

					switch (interaction)
					{
						case PointerInteraction.Hover:
							gui.Image(rect, index == 0 ? Colors.highlight : new Color(0, 0.25f));
							break;
						case PointerInteraction.LClick:
							colorIndex = index;
							break;
						case PointerInteraction.MClick:
							colorToRemove = index;
							break;
					}

					if (index == colorIndex)
						gui.Image(new Rect(rect.center - new Vector2(layout.currentSize / 2), new Vector2(layout.currentSize)), color.inverted.opaque);
					gui.Text(index.ToString(), rect, index == colorIndex ? color.inverted.readableColor : color.readableColor, 1, TextAlignment.Center);

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

		public override void Draw(Vector2 pan, float zoom)
		{
			tiles.For((x, y, tile) =>
			{
				GFX.DrawBox(Scale(new Rect(x, y, 1, 1)), colors[tile]);
			});
		}
	}
}