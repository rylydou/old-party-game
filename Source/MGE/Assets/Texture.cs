using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MGE
{
	public class Texture
	{
		#region Static
		public static List<Texture2D> textures = new List<Texture2D>();
		#endregion

		#region Object
		public readonly int id;
		public Texture2D texture
		{
			get => textures[id];
		}

		public int width { get => texture.Width; }
		public int height { get => texture.Height; }

		public Texture(Texture2D texture)
		{
			var id = textures.FindIndex((x) => x == texture);

			if (id < 0)
			{
				this.id = textures.Count;
				textures.Add(texture);
			}
			else
				this.id = id;
		}

		public static implicit operator Texture(Texture2D texture) => new Texture(texture);
		public static implicit operator Texture2D(Texture texture) => texture.texture;
		#endregion
	}
}