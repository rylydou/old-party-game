using System.Collections.Generic;
using MGE.FileIO;
using MGE.Graphics;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public class SpriteSheet : Asset
	{
		public override string extension => ".spritesheet.psd";

		public Texture texture;

		[JsonProperty] public Dictionary<string, RectInt> regions;

		public SpriteSheet() { }

		public SpriteSheet(Texture texture, Dictionary<string, RectInt> regions)
		{
			this.texture = texture;
			this.regions = regions;
		}

		public override void Load(string fullPath, string localPath = null)
		{
			base.Load(fullPath, localPath);

			regions = IO.LoadJson<SpriteSheet>(fullPath + ".info").regions;

			texture = new Texture();
			texture.Load(fullPath, localPath);
		}

		public void Draw(string name, Rect rect, Color color)
		{
			GFX.Draw(texture, regions[name], rect, color);
		}
	}
}