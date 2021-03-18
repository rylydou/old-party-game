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
			colors = new List<Color>() { Color.clear, Colors.accent, Colors.accent.inverted };
		}

		public override void Update(ref GUI gui)
		{
			using (var layout = new StackLayout(new Vector2Int(0, 8), 32, false))
			{
				ushort index = 0;
				foreach (var color in colors)
				{
					var rect = new Rect(layout.newElement, new Vector2(gui.rect.width, layout.currentSize));

					gui.Image(rect, color);

					var interaction = gui.MouseInteraction(rect);

					switch (interaction)
					{
						case PointerInteraction.Hover:
							gui.Image(rect, new Color(0, 0.1f));
							break;
						case PointerInteraction.LClick:
							colorIndex = index;
							break;
					}

					if (index == colorIndex)
						gui.Rect(rect, color.inverted, 2);

					gui.Text(index.ToString(), rect, Colors.text, 1, TextAlignment.Center);

					index++;
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