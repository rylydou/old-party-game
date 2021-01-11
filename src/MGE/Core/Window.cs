using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGE
{
	public enum WindowMode
	{
		Windowed,
		BorderlessWindowed,
		Fullscreen
	}

	public class Window : EssentialVars
	{
		public static WindowMode windowMode;

		public static Vector2 fullAspectRatio;
		public static float aspectRatio { get => (float)(fullAspectRatio.y / fullAspectRatio.x); }
		public static Vector2Int windowedSize;
		public static Vector2Int windowedPosition;
		public static Action onResize = () => { };

		public static Vector2Int fullRenderSize;
		public static Vector2Int renderSize = new Vector2Int(1920 / 2, 1080 / 2);
		public static Viewport veiwport { get => graphicsDevice.Viewport; }

		static Vector2Int _maxScreenSize;
		public static Vector2Int maxScreenSize
		{
			get
			{
				if (_maxScreenSize == Vector2Int.zero)
					_maxScreenSize = new Vector2Int(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

				return _maxScreenSize;
			}
		}

		public static void Apply()
		{
			switch (windowMode)
			{
				case WindowMode.Windowed:
					window.Position = windowedPosition;
					graphics.PreferredBackBufferWidth = windowedSize.x;
					graphics.PreferredBackBufferHeight = windowedSize.y;
					graphics.IsFullScreen = false;
					window.IsBorderless = false;
					graphics.ApplyChanges();
					break;
				case WindowMode.BorderlessWindowed:
					// windowedPosition = window.Position;
					window.Position = Vector2Int.zero;
					graphics.PreferredBackBufferWidth = maxScreenSize.x;
					graphics.PreferredBackBufferHeight = maxScreenSize.y;
					graphics.IsFullScreen = false;
					window.IsBorderless = true;
					graphics.ApplyChanges();
					break;
				case WindowMode.Fullscreen:
					graphics.PreferredBackBufferWidth = maxScreenSize.x;
					graphics.PreferredBackBufferHeight = maxScreenSize.y;
					graphics.IsFullScreen = true;
					window.IsBorderless = false;
					graphics.ApplyChanges();
					break;
			}
		}

		public static void FixWindow()
		{
			if (windowMode != WindowMode.Windowed) return;

			int horizontalSize = Math.Clamp(graphics.PreferredBackBufferWidth, 64 * 4, int.MaxValue);
			Vector2Int size = new Vector2Int(horizontalSize, (int)(horizontalSize * aspectRatio));

			graphics.PreferredBackBufferWidth = size.x;
			graphics.PreferredBackBufferHeight = size.y;

			// windowedSize = size;

			graphics.ApplyChanges();
		}
	}
}