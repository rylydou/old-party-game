using Microsoft.Xna.Framework;

namespace MGE.Graphics
{
	public class Camera
	{
		static Camera _main;
		public static Camera main { get => _main; }

		protected bool _isDirty = false;

		protected Vector2 _position;
		public Vector2 position { get => _position; set { _position = value; _isDirty = true; } }
		protected float _rotation;
		public float rotation { get => _rotation * (float)Math.rad2Deg; set { _rotation = value * (float)Math.deg2Rad; _isDirty = true; } }
		protected float _zoom;
		public float zoom { get => _zoom; set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; _isDirty = true; } }

		public Vector2 scaleUpFactor { get => ((Vector2)Window.renderSize / (Vector2)Window.sceneSize); }
		public Vector2 scaleDownFactor { get => ((Vector2)Window.sceneSize / (Vector2)Window.renderSize); }

		protected Matrix _transform;

		public Camera()
		{
			_isDirty = true;

			_position = Vector2.zero;
			_rotation = 0;
			_zoom = 1.0f;

			Window.onResize += () => { _isDirty = true; };

			if (_main == null)
				_main = this;
		}

		public void Move(Vector2 amount)
		{
			_position += amount;
		}

		public Matrix Transformation()
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

		public Vector2 WinToCam(Vector2 position)
		{
			return position * scaleUpFactor / GFX.pixelsPerUnit + this.position;
		}

		public Vector2 CamToWin(Vector2 position)
		{
			return position / GFX.pixelsPerUnit + this.position;
		}
	}
}