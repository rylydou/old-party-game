using Microsoft.Xna.Framework;

namespace MGE.Graphics
{
	public static class Camera
	{
		static bool _isDirty = true;

		static Vector2 _position = Vector2.zero;
		public static Vector2 position { get => _position; set { _position = value; _isDirty = true; } }
		static float _rotation = 0.0f;
		public static float rotation { get => _rotation * (float)Math.rad2Deg; set { _rotation = value * (float)Math.deg2Rad; _isDirty = true; } }
		static float _zoom = 1.0f;
		public static float zoom { get => _zoom; set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; _isDirty = true; } }

		public static Vector2 scaleUpFactor { get => ((Vector2)Window.renderSize / (Vector2)Window.sceneSize); }
		public static Vector2 scaleDownFactor { get => ((Vector2)Window.sceneSize / (Vector2)Window.renderSize); }

		static Matrix _transform;

		internal static void Init()
		{
			Window.onResize += () => { _isDirty = true; };
		}

		public static void Move(Vector2 amount)
		{
			_position += amount;
		}

		public static Matrix Transformation()
		{
			if (_isDirty)
			{
				_transform =
					Matrix.CreateTranslation(new Vector3(-_position.x, -_position.y, 0.0f)) *
					Matrix.CreateRotationZ(rotation) *
					Matrix.CreateScale(zoom);

				_isDirty = false;
			}

			return _transform;
		}

		public static Vector2 WinToCam(Vector2 position)
		{
			return position * scaleUpFactor / GFX.pixelsPerUnit + position;
		}

		public static Vector2 CamToWin(Vector2 position)
		{
			return position / GFX.pixelsPerUnit + position;
		}
	}
}