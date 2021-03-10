using System.Collections.Generic;
using MGE.Graphics;
using Newtonsoft.Json;

namespace MGE
{
	public class SpriteSheet
	{
		[JsonIgnore] public readonly Texture texture;

		public Dictionary<string, Rect> regions;

		// public Sprite(Texture texture, Vector2 size, params string[] names)
		// {
		// 	this.texture = texture;

		// 	for (int i = 0; i < size.x; i++)
		// 	{

		// 	}
		// }

		public SpriteSheet()
		{

		}

		public SpriteSheet(Texture texture, Dictionary<string, Rect> regions)
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