using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MGE
{
	public abstract class MGEConfig
	{
		// > General
		public static readonly string gameName = "MGE Game";
		static readonly string defualtFontPath = @"Fonts/Basic";
		// C:/Users/{current-user}AppData/Local/Company/Game/
		public static string saveDataPath = @"%LocalAppData%/MGE/MGEGame/";

		// > Graphics
		// >> Windowing
		public static readonly bool allowWindowResizing = true;
		public static readonly Vector2 aspectRatio = new Vector2(16.0, 9.0);
		public static readonly Vector2Int defaultWindowSize = new Vector2Int(1280, 720);
		public static readonly int minWindowHorizontalSize = 1920 / 4;
		public static readonly Vector2Int gameSize = new Vector2Int(1920 / 6, 1080 / 6);

		// > Asset Management
		public static readonly Dictionary<Type, string> typeToExtention = new Dictionary<Type, string>()
		{
			{typeof(Texture2D), ".psd"},
			{typeof(SoundEffect), ".wav"},
			{typeof(SpriteFont), ".font.psd"},
		};
		public static readonly string infoFileExt = ".info";

		// > Debuging
		public static readonly Color statsColor = new Color("#EEE5");

		public static readonly int statsUpdatesPerSec = 15;
		public static readonly int fpsHistorySize = 60;
		public static readonly Dictionary<int, Color> fpsColors = new Dictionary<int, Color>()
		{
			{31, new Color("#E22C")},
			{56, new Color("#EE2C")},
			{int.MaxValue, new Color("#EEEC")}
		};

		////////////////////////////////////////
		#region Config Utils
		static SpriteFont _defualtFont;
		public static SpriteFont defualtFont
		{
			get
			{
				if (_defualtFont == null)
					_defualtFont = Assets.GetAsset<SpriteFont>(defualtFontPath);
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