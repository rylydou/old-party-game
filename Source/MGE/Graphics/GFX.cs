using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.Graphics
{
	public static class GFX
	{
		public static GraphicsDevice graphicsDevice { get => Engine.game.GraphicsDevice; }
		public static GraphicsDeviceManager graphics { get => Engine.current.graphics; }
		public static SpriteBatch sb { get => Engine.current.sb; }

		public static ulong drawCalls { get; internal set; }

		public static int pixelsPerUnit { get => Config.pixelsPerUnit; }
		public static int currentPixelsPerUnit { get; internal set; } = 16;

		static readonly Dictionary<int, List<Vector2>> circleCache = new Dictionary<int, List<Vector2>>();

		static Texture _pixel;
		public static Texture pixel
		{
			get
			{
				if (_pixel == null)
				{
					_pixel = new Texture(new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color));
					_pixel.texture.SetData(new[] { Microsoft.Xna.Framework.Color.White });
				}

				return _pixel;
			}
		}

		public static void Draw(Texture texture, Rect source, Vector2 position, Color? color = null)
		{
			if (!color.HasValue) color = Color.white;

			sb.Draw(texture != null ? texture : pixel, new Rect((position * currentPixelsPerUnit).rounded, source.size), source, color.Value);
		}

		public static void Draw(Texture texture, Vector2 position, Color? color = null, bool flippedX = false, bool flippedY = false)
		{
			if (!color.HasValue) color = Color.white;

			sb.Draw(
				texture is object ? texture : pixel,
				(position * currentPixelsPerUnit).rounded, new Rect(0, 0, texture.size),
				color.Value,
				0,
				Vector2.zero,
				Vector2.one,
				(flippedX ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (flippedY ? SpriteEffects.FlipVertically : SpriteEffects.None),
				0
			);
			drawCalls++;
		}

		public static void Draw(Texture texture, Rect destination, Color? color = null, float rotation = 0, Vector2 origin = default)
		{
			if (!color.HasValue) color = Color.white;

			sb.Draw(texture != null ? texture : pixel, new Rect((destination.position * currentPixelsPerUnit).rounded, (destination.size * currentPixelsPerUnit).rounded), null, color.Value, rotation, origin, SpriteEffects.None, 0);
			drawCalls++;
		}

		public static void Draw(Texture texture, RectInt source, Rect destination, Color? color = null, float rotation = 0, Vector2 origin = default)
		{
			if (!color.HasValue) color = Color.white;

			sb.Draw(texture != null ? texture : pixel, new Rect((destination.position * currentPixelsPerUnit).rounded, (destination.size * currentPixelsPerUnit).rounded), source, color.Value, rotation, origin, SpriteEffects.None, 0);
			drawCalls++;
		}

		public static void DrawDirect(Texture texture, Vector2 position, RectInt? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0)
		{
			sb.Draw(texture != null ? texture : pixel, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
			drawCalls++;
		}

		public static void DrawDirect(Texture texture, Rect destinationRectangle, RectInt? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0)
		{
			sb.Draw(texture != null ? texture : pixel, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
			drawCalls++;
		}

		public static void SetupToDrawGame()
		{
			currentPixelsPerUnit = pixelsPerUnit;
		}

		public static void SetupToDrawUI()
		{
			currentPixelsPerUnit = 1;
		}

		static List<Vector2> CreateArc(float radius, int sides, float startingAngle, float radians)
		{
			List<Vector2> points = new List<Vector2>();
			points.AddRange(CreateCircle(radius, sides));
			points.RemoveAt(points.Count - 1);

			var curAngle = 0.0f;
			var anglePerSide = Math.pi2 / sides;

			while ((curAngle + (anglePerSide / 2.0f)) < startingAngle)
			{
				curAngle += anglePerSide;

				points.Add(points[0]);
				points.RemoveAt(0);
			}

			points.Add(points[0]);

			var sidesInArc = (int)((radians / anglePerSide) + 0.5);
			points.RemoveRange(sidesInArc + 1, points.Count - sidesInArc - 1);

			return points;
		}

		static List<Vector2> CreateCircle(float radius, int sides = 0)
		{
			if (sides == 0)
				sides = Math.RoundToInt(Math.Clamp(radius / 16f * 4f, 16, 64));

			var circleKey = ((byte)radius ^ (byte)sides).GetHashCode();
			if (circleCache.ContainsKey(circleKey))
				return circleCache[circleKey];

			var vectors = new List<Vector2>();

			var step = Math.pi2 / sides;

			for (float theta = 0.0f; theta < Math.pi2; theta += step)
				vectors.Add(new Vector2((radius * Math.Cos(theta)), (radius * Math.Sin(theta))));

			vectors.Add(new Vector2((radius * Math.Cos(0.0f)), (radius * Math.Sin(0.0f))));

			circleCache.Add(circleKey, vectors);

			return vectors;
		}

		public static void DrawArc(Vector2 center, float radius, int sides, float startingAngle, float radians, Color? color = null, float thickness = 1.0f)
		{
			List<Vector2> arc = CreateArc(radius, sides, startingAngle, radians);

			DrawPoints(center, arc, color, thickness);
		}

		public static void DrawCircle(Vector2 position, float radius, Color? color = null, float thickness = 1.0f, int sides = 0)
		{
			DrawPoints(position, CreateCircle(radius, sides), color, thickness);
		}

		public static void DrawBox(Rect rect, Color? color = null, float angle = 0.0f)
		{
			Draw(pixel, rect, color, angle);
		}

		public static void DrawRect(Rect rect, Color? color = null, float thickness = 1.0f)
		{
			DrawLine(new Vector2(rect.x - thickness, rect.y - thickness), new Vector2(rect.right, rect.y - thickness), color, thickness); // Top
			DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x, rect.bottom + thickness), color, thickness); // Left
			DrawLine(new Vector2(rect.x, rect.bottom), new Vector2(rect.right, rect.bottom), color, thickness); // Bottom
			DrawLine(new Vector2(rect.right + thickness, rect.y - thickness), new Vector2(rect.right + thickness, rect.bottom + thickness), color, thickness); // Right
		}

		public static void DrawLine(Vector2 from, Vector2 to, Color? color = null, float thickness = 1.0f)
		{
			if (!color.HasValue) color = Color.white;

			var distance = (float)Vector2.Distance(from, to);

			var angle = (float)Math.Atan(to.y - from.y, to.x - from.x);

			DrawLine(from, distance, color.Value, angle, thickness);
		}

		public static void DrawLine(Vector2 position, float length, Color color, float angle = 0.0f, float thickness = 1.0f)
		{
			sb.Draw(pixel, position * currentPixelsPerUnit, null, color, angle, Vector2.zero, new Vector2(length, thickness) * currentPixelsPerUnit, SpriteEffects.None, 0);
		}

		public static void DrawPoints(Vector2 position, List<Vector2> points, Color? color = null, float thickness = 1.0f)
		{
			DrawPoints(position, points.ToArray(), color, thickness);
		}

		public static void DrawPoints(Vector2 position, Vector2[] points, Color? color = null, float thickness = 1.0f)
		{
			if (points.Length < 2) return;

			for (int i = 1; i < points.Length; i++)
				DrawLine(points[i - 1] + position, points[i] + position, color, thickness);
		}

		public static void DrawPoint(Vector2 position, Color? color = null)
		{
			Draw(pixel, position, color);
		}
	}
}