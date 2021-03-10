using System.Collections.Generic;
using MGE.Graphics;
using Newtonsoft.Json;

namespace MGE
{
	public class SpriteSheet
	{
		[JsonIgnore] public readonly Texture texture;

		public Dictionary<string, RectInt> regions;

		public SpriteSheet() { }

		public SpriteSheet(Texture texture, Dictionary<string, RectInt> regions)
		{
			this.texture = texture;
			this.regions = regions;
		}

		public void Draw(string name, Rect rect, Color color)
		{
			GFX.Draw(texture, regions[name], rect, color);
		}
	}
}