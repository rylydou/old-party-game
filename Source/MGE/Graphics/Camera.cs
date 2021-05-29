using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.Graphics
{
	public static class Camera
	{
		public static bool dirty { get; private set; } = true;

		static Vector2 _position = Vector2.zero;
		public static Vector2 position { get => _position * GFX.currentUnitsPerPixel; set { if (_position != value) { _position = value * GFX.currentPixelsPerUnit; dirty = true; } } }
		static float _rotation = 0.0f;
		public static float rotation { get => _rotation; set { if (_rotation != value) { _rotation = value; dirty = true; } } }
		static float _zoom = 1.0f;
		public static float zoom { get => _zoom; set { if (_zoom != value) { _zoom = value; dirty = true; } } }

		public static Vector2 scaleUpFactor { get => ((Vector2)Window.renderSize / (Vector2)Window.gameRenderSize); }
		public static Vector2 scaleDownFactor { get => ((Vector2)Window.gameRenderSize / (Vector2)Window.renderSize); }

		public static Effect postEffect;

		static Matrix _transform;
		public static Matrix transform
		{
			get
			{
				if (dirty)
				{
					dirty = false;

					_transform =
						Matrix.CreateTranslation(new Vector3(-_position.x, -_position.y, 0.0f)) *
						Matrix.CreateRotationZ(rotation) *
						Matrix.CreateScale(zoom);
				}

				return _transform;
			}
		}

		internal static void Init()
		{
			Window.onResize += () => dirty = true;
		}

		public static void Move(Vector2 amount)
		{
			_position += amount;
		}

		public static Vector2 WinToCam(Vector2 position)
		{
			return (position * scaleDownFactor + _position) / Config.pixelsPerUnit;
		}

		public static Vector2 CamToWin(Vector2 position)
		{
			return (position * scaleUpFactor - _position) * Config.pixelsPerUnit;
		}
	}
}