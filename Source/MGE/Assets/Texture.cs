using System.Collections.Generic;
using System.IO;
using System.Linq;
using MGE.FileIO;
using MGE.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace MGE
{
	public class Texture : Asset
	{
		public override string extension => ".psd";

		public Texture2D texture;

		public int width { get => texture.Width; }
		public int height { get => texture.Height; }

		public Vector2Int size { get => new Vector2Int(texture.Width, texture.Height); }

		public Texture() { }

		public Texture(Texture2D texture)
		{
			this.texture = texture;
		}

		public override void Save(string fullPath)
		{
			base.Save(fullPath);

			using (var stream = IO.FileOpenWrite(fullPath))
			{
				SaveAsPNG(stream, width, height);
			}
		}

		public void SaveAsPNG(Stream stream, int width, int height) => texture.SaveAsPng(stream, width, height);
		public void SaveAsJPEG(Stream stream, int width, int height) => texture.SaveAsJpeg(stream, width, height);

		public override void Load(string fullPath, string localPath = null)
		{
			base.Load(fullPath, localPath);

			texture = Texture2D.FromFile(GFX.graphicsDevice, fullPath);
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

		public static implicit operator Texture(Texture2D texture) => new Texture(texture);
		public static implicit operator Texture2D(Texture texture) => texture.texture;
	}
}