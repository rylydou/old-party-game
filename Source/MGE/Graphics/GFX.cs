using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.Graphics
{
	public class GFX
	{
		public static GraphicsDevice graphicsDevice { get => Engine.game.GraphicsDevice; }
		public static GraphicsDeviceManager graphics { get => Engine.current.graphics; }
		public static SpriteBatch sb { get => Engine.current.sb; }

		public static ulong drawCalls { get; internal set; }

		#region Normal Drawing
		public static void Draw(Texture texture, Vector2 position, Color color)
		{
			sb.Draw(texture != null ? texture : pixel, position, color);
			drawCalls++;
		}

		public static void Draw(Texture texture, Rect rect, Color color)
		{
			sb.Draw(texture != null ? texture : pixel, rect, color);
			drawCalls++;
		}

		public static void Draw(Texture texture, RectInt source, Rect destination, Color color)
		{
			sb.Draw(texture != null ? texture : pixel, destination, source, color);
			drawCalls++;
		}

		public static void Draw(Texture texture, Vector2 position, RectInt? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
		{
			sb.Draw(texture != null ? texture : pixel, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
			drawCalls++;
		}

		public static void Draw(Texture texture, Rect destinationRectangle, RectInt? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
		{
			sb.Draw(texture != null ? texture : pixel, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
			drawCalls++;
		}
		#endregion

		#region Primitive Drawing
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

		static List<Vector2> CreateArc(float radius, int sides, float startingAngle, float radians)
		{
			List<Vector2> points = new List<Vector2>();
			points.AddRange(CreateCircle(radius, sides));
			points.RemoveAt(points.Count - 1);

			var curAngle = 0.0;
			var anglePerSide = Math.pi2 / sides;

			while ((curAngle + (anglePerSide / 2.0)) < startingAngle)
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

		static List<Vector2> CreateCircle(double radius, int sides = 0)
		{
			if (sides == 0)
				sides = Math.RoundToInt(Math.Clamp(radius / 16f * 4f, 16, 64));

			var circleKey = ((byte)radius ^ (byte)sides).GetHashCode();
			if (circleCache.ContainsKey(circleKey))
				return circleCache[circleKey];

			var vectors = new List<Vector2>();

			var step = Math.pi2 / sides;

			for (double theta = 0.0; theta < Math.pi2; theta += step)
				vectors.Add(new Vector2((radius * Math.Cos(theta)), (radius * Math.Sin(theta))));

			vectors.Add(new Vector2((radius * Math.Cos(0.0)), (radius * Math.Sin(0.0))));

			circleCache.Add(circleKey, vectors);

			return vectors;
		}

		public static void DrawArc(Vector2 center, float radius, int sides, float startingAngle, float radians, Color color, float thickness = 1.0f)
		{
			List<Vector2> arc = CreateArc(radius, sides, startingAngle, radians);

			DrawPoints(center, arc, color, thickness);
		}

		public static void DrawCircle(Vector2 position, float radius, Color color, float thickness = 1.0f, int sides = 0)
		{
			DrawPoints(position, CreateCircle(radius, sides), color, thickness);
		}

		public static void DrawBox(Rect rect, Color color, float angle = 0.0f)
		{
			GFX.Draw(pixel, rect, null, color, angle, Vector2.zero, SpriteEffects.None, 0);
		}

		public static void DrawRectangle(Rect rect, Color color, float thickness = 1.0f)
		{
			DrawLine(new Vector2(rect.x - thickness, rect.y - thickness), new Vector2(rect.right, rect.y - thickness), color, thickness); // Top
			DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x, rect.bottom + thickness), color, thickness); // Left
			DrawLine(new Vector2(rect.x, rect.bottom), new Vector2(rect.right, rect.bottom), color, thickness); // Bottom
			DrawLine(new Vector2(rect.right + thickness, rect.y - thickness), new Vector2(rect.right + thickness, rect.bottom + thickness), color, thickness); // Right
		}

		public static void DrawLine(Vector2 from, Vector2 to, Color color, float thickness = 1.0f)
		{
			var distance = (float)Vector2.Distance(from, to);

			var angle = (float)Math.Atan2(to.y - from.y, to.x - from.x);

			DrawLine(from, distance, color, angle, thickness);
		}

		public static void DrawLine(Vector2 position, float length, Color color, float angle = 0.0f, float thickness = 1.0f)
		{
			sb.Draw(pixel, position, null, color, angle, Vector2.zero, new Vector2(length, thickness), SpriteEffects.None, 0);
		}

		public static void DrawPoints(Vector2 position, List<Vector2> points, Color color, float thickness = 1.0f)
		{
			DrawPoints(position, points.ToArray(), color, thickness);
		}

		public static void DrawPoints(Vector2 position, Vector2[] points, Color color, float thickness = 1.0f)
		{
			if (points.Length < 2) return;

			for (int i = 1; i < points.Length; i++)
				DrawLine(points[i - 1] + position, points[i] + position, color, thickness);
		}

		public static void DrawPoint(Vector2 position, Color color)
		{
			GFX.Draw(pixel, position, color);
		}
		#endregion
	}
}