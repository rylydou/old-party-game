using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.Graphics
{
	public class Camera : EssentialVars
	{
		static Camera _main;
		public static Camera main { get => _main; }

		protected bool _isDirty = false;

		protected Vector2 _pos;
		public Vector2 pos { get => _pos; set { _pos = value; _isDirty = true; } }
		protected float _rotation;
		public float rotation { get => _rotation * (float)Math.rad2Deg; set { _rotation = value * (float)Math.deg2Rad; _isDirty = true; } }
		protected float _zoom;
		public float zoom { get => _zoom; set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; _isDirty = true; } }

		protected Matrix _transform;

		public Camera()
		{
			_isDirty = true;

			_pos = Vector2.zero;
			_rotation = 0;
			_zoom = 1.0f;

			Window.onResize += () => { _isDirty = true; };

			if (_main == null)
				_main = this;
		}

		public void Move(Vector2 amount)
		{
			_pos += amount;
		}

		public Matrix Transformation()
		{
			if (_isDirty)
			{
				_transform =
					Matrix.CreateTranslation(new Vector3(-(float)_pos.x - Window.veiwport.Width / 2, -(float)_pos.y - Window.veiwport.Height / 2, 0.0f)) *
					Matrix.CreateRotationZ(rotation) *
					Matrix.CreateScale(new Vector3(zoom, zoom, 1.0f)) *
					Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2, 0)) *
					Matrix.CreateScale((float)Window.renderSize.x / Window.gameSize.x, (float)Window.renderSize.y / Window.gameSize.y, 1f);

				_isDirty = false;
			}

			return _transform;
		}

		public Vector2 MouseToScreenPos(Vector2 position)
		{
			// TODO: Don't be bad
			return position * (Vector2)(Window.renderSize / Window.gameSize);
		}
	}
}