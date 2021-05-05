using System;
using System.Collections.Generic;
using MGE.Debug;
using MGE.Debug.Menus;

namespace MGE
{
	public static class Config
	{
		// # General
		public static readonly string gameName = "MGE Game";
		static readonly string defualtFontPath = @"Fonts/Basic";
		// C:/Users/{current-user}/AppData/Local/Company/Game/
		public static string saveDataPath = @"%LocalAppData%/MGE/MGEGame/";

		// # Game Loop
		public static int tps = 60;

		// # Graphics
		public static readonly int pixelsPerUnit = 16;
		public static readonly Vector2Int sceneSize = new Vector2Int(16 * 2, 8 * 2);
		public static readonly Vector2Int gameRenderSize = new Vector2Int(320 * 2, 180 * 2);

		// #> Windowing
		public static readonly bool allowWindowResizing = true;
		public static readonly Vector2 aspectRatio = new Vector2(16.0f, 9.0f);
		public static readonly Vector2Int defaultWindowSize = new Vector2Int(1280, 720);

		// # Asset Management
		public static readonly Dictionary<Type, string> typeToExtention = new Dictionary<Type, string>()
		{
			{typeof(Texture), ".psd"},
			{typeof(SpriteSheet), ".spritesheet.psd"},
			{typeof(Tileset), ".tileset.psd"},
			{typeof(Microsoft.Xna.Framework.Audio.SoundEffect), ".wav"},
			{typeof(Sound), ".sound"},
			{typeof(Font), ".font.psd"},
			{typeof(Feilds), ".feilds"},
		};
		public static readonly string infoFileExt = ".info";

		// # Debuging
		public static DebugMenu[] availableMenus = new DebugMenu[]
		{
			new DMenuTest(),
			new DMenuHierarchy(),
			new DMenuAssets(),
			new DMenuInput(),
		};

		// # Editor

		// # Debugging
		public static readonly Color statsColor = new Color("#EEE5");

		public static readonly int statsUpdatesPerSec = 15;
		public static readonly int fpsHistorySize = 60;
		public static readonly Dictionary<int, Color> fpsColors = new Dictionary<int, Color>()
		{
			{31, new Color("#E22C")},
			{56, new Color("#EE2C")},
			{int.MaxValue, new Color("#EEEC")}
		};

		////////////////////////////////////////////////////////////////////////////////

		#region Config Utils
		public static float timeBtwTicks { get => 1.0f / tps; }
		static Font _defualtFont;
		public static Font font
		{
			get
			{
				if (_defualtFont == null)
					_defualtFont = Assets.GetAsset<Font>(defualtFontPath);
				return _defualtFont;
			}
		}
		public static float timeBtwStatsUpdate { get => 1.0f / statsUpdatesPerSec; }
		public static Color FpsToColor(int fps)
		{
			foreach (var color in fpsColors)
			{
				if (fps < color.Key)
				{
					return color.Value;
				}
			}

			return Color.white;
		}
		#endregion
	}
}