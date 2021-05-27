using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MGE.Graphics;
using MGE.InputSystem;

namespace MGE
{
	public enum PointerMode
	{
		System,
		Hidden,
		Texture
	}

	public static class Pointer
	{
		public static PointerMode mode = PointerMode.System;
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
		public static float hideAfter = 15.0f;
		public static float minMoveToUnhide = 4.0f;

		public static Texture texture;
		public static Color color;
		public static Vector2 hotspot;
		public static Vector2 size;
		public static Color shadowColor;
		public static Vector2 shadowOffset;

		static float timeNotMoving = 0.0f;
		static Vector2 lastPos;

		internal static void Draw()
		{
			if (
				Vector2.DistanceGT(lastPos, Input.windowMousePosition, minMoveToUnhide) |
				!Math.Approximately(Input.scroll, 0) |
				Input.GetButton(Inputs.MouseLeft) |
				Input.GetButton(Inputs.MouseMiddle) |
				Input.GetButton(Inputs.MouseRight)
			)
				timeNotMoving = 0;

			timeNotMoving += Time.deltaTime;
			if (timeNotMoving < hideAfter)
			{
				Engine.game.IsMouseVisible = mode == PointerMode.System;

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
			else
			{
				Engine.game.IsMouseVisible = false;
			}

			lastPos = Input.windowMousePosition;
		}
	}
}