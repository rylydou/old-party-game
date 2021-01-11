using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MGE.Graphics;

namespace MGE
{
	public enum PointerMode
	{
		System,
		Hidden,
		Sprite
	}

	public class Pointer : EssentialVars
	{
		static PointerMode _mode = PointerMode.System;
		public static PointerMode mode { get => _mode; set { _mode = value; Main.current.IsMouseVisible = _mode == PointerMode.System; } }
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
		public static DrawCall sprite;
	}
}