using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MGE.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace MGE
{
	[System.Serializable]
	public class Texture
	{
		#region Static
		public static List<Texture2D> textures = new List<Texture2D>();

		public static Texture FromFile(string path)
		{
			if (File.Exists(path))
				return new Texture(Texture2D.FromFile(GFX.graphicsDevice, path)) { sourcePath = path };
			else return null;
		}

		public static Texture FromStream(Stream stream) =>
			new Texture(Texture2D.FromStream(GFX.graphicsDevice, stream));
		#endregion

		#region Object
		// [NonSerialized] Just for now...
		public readonly int id;
		public string sourcePath { get; internal set; } = string.Empty;

		public Texture2D texture { get => textures[id]; }

		public int width { get => texture.Width; }
		public int height { get => texture.Height; }

		public Vector2Int size { get => new Vector2Int(texture.Width, texture.Height); }

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

		public Color GetPixel(Vector2Int position) => GetPixel(position.x, position.y);
		public Color GetPixel(int x, int y)
		{
			if (x < 0 || x >= width || y < 0 || y >= height) return Color.clear;

			var rect = new Rect(x, y, 1, 1);
			var colors = new Microsoft.Xna.Framework.Color[1];

			texture.GetData(0, rect, colors, 0, 1);

			return colors[0];
		}

		public bool SetPixel(Vector2Int position, Color color) => SetPixel(position.x, position.y, color);
		public bool SetPixel(int x, int y, Color color)
		{
			if (x < 0 || x >= width || y < 0 || y >= height) return false;

			var rect = new Rect(x, y, 1, 1);
			var colors = new Microsoft.Xna.Framework.Color[1] { color };

			texture.SetData<Microsoft.Xna.Framework.Color>(0, rect, colors, 0, 1);

			return true;
		}

		public Color[] GetPixels() => GetPixels(new RectInt(0, 0, width, height));
		public Color[] GetPixels(RectInt rect)
		{
			var amount = (int)rect.width * (int)rect.height;
			var colors = new Microsoft.Xna.Framework.Color[amount];

			texture.GetData(0, rect, colors, 0, amount);

			return colors.Select((x) => (Color)x).ToArray();
		}

		public void SetPixels(Color[] colors) => texture.SetData(colors.Select((x) => (Microsoft.Xna.Framework.Color)x).ToArray());
		public void SetPixels(RectInt rect, Color[] colors)
		{
			var amount = (int)rect.width * (int)rect.height;

			texture.SetData(0, 0, rect, colors.Select((x) => (Microsoft.Xna.Framework.Color)x).ToArray(), 0, amount);
		}

		public void SaveAsPNG(Stream stream, int width, int height) => texture.SaveAsPng(stream, width, height);
		public void SaveAsJPEG(Stream stream, int width, int height) => texture.SaveAsJpeg(stream, width, height);

		public static implicit operator Texture(Texture2D texture) => new Texture(texture);
		public static implicit operator Texture2D(Texture texture) => texture.texture;
		#endregion
	}
}