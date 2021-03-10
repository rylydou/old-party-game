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

	public class Pointer
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

		public static Texture texture;
		public static Color color;
		public static Vector2 hotspot;
		public static Vector2 size;
		public static Color shadowColor;
		public static Vector2 shadowOffset;

		public static void Draw()
		{
			if (Pointer.mode == PointerMode.Texture)
			{
				using (new DrawBatch(transform: null))
				{
					GFX.Draw(
						Pointer.texture,
						new Rect((Vector2)Mouse.GetState().Position - Pointer.size * Pointer.hotspot + Pointer.shadowOffset, Pointer.size),
						Pointer.shadowColor);

					GFX.Draw(
						Pointer.texture,
						new Rect((Vector2)Mouse.GetState().Position - Pointer.size * Pointer.hotspot, Pointer.size),
						Pointer.color);
				}
			}
		}
	}
}