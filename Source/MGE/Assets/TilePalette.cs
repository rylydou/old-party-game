using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public class TilePalette
	{
		public Texture texture;

		public Tileset[] tilesets;
	}
}