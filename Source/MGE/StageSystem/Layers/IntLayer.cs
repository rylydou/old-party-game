using System.Collections.Generic;
using MGE.Components;
using MGE.Graphics;

namespace MGE.StageSystem.Layers
{
	[System.Serializable]
	public class IntLayer : StageLayer
	{
		public Grid<ushort> tiles;
		public List<Color> colors;

		public IntLayer(Vector2Int size)
		{
			tiles = new Grid<ushort>(size.x, size.y);
			colors = new List<Color>() { Color.clear };
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