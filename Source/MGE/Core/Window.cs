using MGE.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGE
{
	public static class Window
	{
		public static GameWindow gameWindow { get => Engine.game.Window; }

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

		public static bool foucused { get; internal set; }

		public static Action<bool> focusChanged = (x) => { };

		public static Viewport veiwport { get => GFX.graphicsDevice.Viewport; set => GFX.graphicsDevice.Viewport = value; }

		public static Vector2 aspectRatioFrac { get => Config.aspectRatio; }
		public static float aspectRatio { get => (float)(aspectRatioFrac.y / aspectRatioFrac.x); }

		public static Vector2Int renderSize { get => new Vector2Int(GFX.graphics.PreferredBackBufferWidth, GFX.graphics.PreferredBackBufferHeight); }

		public static Vector2Int sceneSize { get => Config.gameRenderSize / Config.pixelsPerUnit; }
		public static Vector2Int gameRenderSize { get => Config.gameRenderSize; }

		public static Action onResize = () => { };

		public static void FixWindow()
		{
			if (GFX.graphics.IsFullScreen) return;

			var horizontalSize = Math.Clamp(GFX.graphics.PreferredBackBufferWidth, gameRenderSize.x, int.MaxValue);
			var size = new Vector2Int(horizontalSize, (int)(horizontalSize * aspectRatio));

			GFX.graphics.PreferredBackBufferWidth = size.x;
			GFX.graphics.PreferredBackBufferHeight = size.y;
		}
	}
}