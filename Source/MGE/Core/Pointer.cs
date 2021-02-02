using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MGE.Graphics;

namespace MGE
{
	public enum PointerMode
	{
		System,
		Hidden,
		Texture
	}

	public class Pointer : EssentialVars
	{
		static PointerMode _mode = PointerMode.System;
		public static PointerMode mode { get => _mode; set { _mode = value; Engine.game.IsMouseVisible = _mode == PointerMode.System; } }
		static MouseCursor _mouseCursor = MouseCursor.Arrow;
		public static MouseCursor mouseCursor
		{
			get => _mouseCursor;
			set
			{
				_mouseCursor = value;

				Mouse.SetCursor(_mouseCursor);
			}
		}

		public static Texture2D texture;
		public static Color color;
		public static Vector2 hotspot;
		public static Vector2 size;
		public static Color shadowColor;
		public static Vector2 shadowOffset;
	}
}