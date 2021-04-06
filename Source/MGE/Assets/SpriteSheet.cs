using System.Collections.Generic;
using MGE.Graphics;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public class SpriteSheet
	{
		public Texture texture;

		[JsonProperty] public Dictionary<string, RectInt> regions;

		public SpriteSheet() { }

		public SpriteSheet(Texture texture, Dictionary<string, RectInt> regions)
		{
			this.texture = texture;
			this.regions = regions;
		}

		public void Draw(string name, Rect rect, Color color)
		{
			GFX.DrawDirect(texture, regions[name], rect, color);
		}
	}
}