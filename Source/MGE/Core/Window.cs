using MGE.Graphics;
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

	public class Window
	{
		public static GameWindow gameWindow { get => Engine.game.Window; }

		public static WindowMode windowMode;

		static bool _isFocused;
		public static bool isFocused
		{
			get => _isFocused;
			internal set
			{
				_isFocused = value;
				focusChanged.Invoke(_isFocused);
			}
		}
		public static Action<bool> focusChanged = (x) => { };

		public static Vector2Int _windowedSize;
		public static Vector2Int windowedSize
		{
			get
			{
				if (windowMode == WindowMode.Windowed)
					return _windowedSize;
				return monitorSize;
			}
			set => _windowedSize = value;
		}
		public static Vector2Int windowedPosition;
		static Vector2Int _monitorSize;
		public static Vector2Int monitorSize
		{
			get
			{
				if (_monitorSize == Vector2Int.zero)
					_monitorSize = new Vector2Int(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

				return _monitorSize;
			}
		}
		public static Viewport veiwport { get => Engine.game.GraphicsDevice.Viewport; }

		public static Vector2 aspectRatioFrac;
		public static float aspectRatio { get => (float)(aspectRatioFrac.y / aspectRatioFrac.x); }

		public static Vector2Int renderSize;
		public static Vector2Int gameSize = Config.gameSize;
		// public static Vector2Int gameRenderSize = Config.gameSize * Config.pixelsPerUnit;
		public static Vector2Int gameRenderSize = Config.gameRenderSize;

		public static Action onResize = () => { };

		public static void Apply()
		{
			switch (windowMode)
			{
				case WindowMode.Windowed:
					gameWindow.Position = windowedPosition;
					GFX.graphics.PreferredBackBufferWidth = windowedSize.x;
					GFX.graphics.PreferredBackBufferHeight = windowedSize.y;
					GFX.graphics.IsFullScreen = false;
					gameWindow.IsBorderless = false;
					GFX.graphics.ApplyChanges();
					break;
				case WindowMode.BorderlessWindowed:
					// windowedPosition = gameWindow.Position;
					gameWindow.Position = Vector2Int.zero;
					GFX.graphics.PreferredBackBufferWidth = monitorSize.x;
					GFX.graphics.PreferredBackBufferHeight = monitorSize.y;
					GFX.graphics.IsFullScreen = false;
					gameWindow.IsBorderless = true;
					GFX.graphics.ApplyChanges();
					break;
				case WindowMode.Fullscreen:
					GFX.graphics.PreferredBackBufferWidth = monitorSize.x;
					GFX.graphics.PreferredBackBufferHeight = monitorSize.y;
					GFX.graphics.IsFullScreen = true;
					gameWindow.IsBorderless = false;
					GFX.graphics.ApplyChanges();
					break;
			}

			Engine.current.OnResize();
		}

		public static void FixWindow()
		{
			if (windowMode != WindowMode.Windowed) return;

			int horizontalSize = Math.Clamp(GFX.graphics.PreferredBackBufferWidth, Config.minWindowHorizontalSize, int.MaxValue);
			Vector2Int size = new Vector2Int(horizontalSize, (int)(horizontalSize * aspectRatio));

			GFX.graphics.PreferredBackBufferWidth = size.x;
			GFX.graphics.PreferredBackBufferHeight = size.y;

			windowedSize = size;
			windowedPosition = Engine.game.Window.Position;

			GFX.graphics.ApplyChanges();
		}
	}
}