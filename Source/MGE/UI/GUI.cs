using Microsoft.Xna.Framework;

namespace MGE.UI
{
	public static class GUI
	{
		public static bool dirty { get; private set; } = true;

		static Matrix _transform;
		public static Matrix transform
		{
			get
			{
				if (dirty)
				{
					dirty = false;
					_transform = Matrix.CreateScale(new Vector3(scaleUpFactor.y, scaleUpFactor.y, 0));
				}
				return _transform;
			}
		}

		public static GUIElement root { get; private set; }

		public static Vector2 scaleUpFactor { get => ((Vector2)Window.renderSize / root.rect.size); }
		public static Vector2 scaleDownFactor { get => (root.rect.size / (Vector2)Window.renderSize); }

		public static Vector2 canvasSize { get => Config.canvasSize; }

		internal static void Init()
		{
			root = new GUIElement(new Rect(0, canvasSize.x, canvasSize.y));

			Window.onResize += () => dirty = true;
		}

		internal static void Update()
		{
			root.Update();
		}

		internal static void Draw()
		{
			root.Draw();
		}

		public static Vector2 WindowToCanvas(Vector2 position) => position * scaleUpFactor;

		public static Vector2 CanvasToWindow(Vector2 position) => position * scaleDownFactor;
	}
}