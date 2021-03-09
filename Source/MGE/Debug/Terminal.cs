using System;
using Microsoft.Xna.Framework.Graphics;
using MGE.UI.Layouts;
using MGE.InputSystem;
using MGE.Graphics;
using Microsoft.Xna.Framework;

namespace MGE.Debug
{
	public static class Terminal
	{
		public static bool enabled = true;

		static SpriteFont font { get => Config.defualtFont; }

		static long lastMem = int.MaxValue;

		public static void Draw()
		{
			if (!enabled) return;

			var mem = GC.GetTotalMemory(false);

			using (var layout = new StackLayout(new Vector2Int(8), 20, false))
			{
				GFX.DrawText($"{Util.CleanRound(Stats.fps)} / {Util.CleanRound(Stats.averageFps)} / {Util.CleanRound(Stats.minFps)}", layout.AddElement(), Config.FpsToColor((int)Stats.fps));
				// GFX.DrawText("--- General ---", layout.AddElement());
				GFX.DrawText($"Mem: {mem / 1000000}MB / {Environment.WorkingSet / 1000000}MB" + (mem - lastMem > 0 ? " +" : " -"), layout.AddElement());
				// GFX.DrawText($"Time running: {Math.Round(Time.time, 2)}", layout.AddElement());
				// GFX.DrawText("--- Graphics ---", layout.AddElement());
				// GFX.DrawText($"Window Mode: {Window.windowMode}", layout.AddElement());
				// GFX.DrawText($"Scale Up: {Camera.main.scaleUpFactor}", layout.AddElement());
				// GFX.DrawText($"Scale Down: {Camera.main.scaleDownFactor}", layout.AddElement());
				// GFX.DrawText($"Draw Calls: {GFX.drawCalls}", layout.AddElement());
				// GFX.DrawText("--- Asset ---", layout.AddElement());
				// GFX.DrawText($"Preloaded: {Assets.preloadedAssets.Count}", layout.AddElement());
				// GFX.DrawText($"Unloaded: {Assets.unloadedAssets.Count}", layout.AddElement());
				// GFX.DrawText("--- Scene ---", layout.AddElement());
				// GFX.DrawText("--- Input ---", layout.AddElement());
				// GFX.DrawText($"Win Mouse Pos: {Input.windowMousePosition}" + (Input.isMouseInWindow ? "" : "!"), layout.AddElement());
				// GFX.DrawText($"Cam Mouse Pos: {Input.cameraMousePosition}", layout.AddElement());
				// GFX.DrawText($"Abs Mouse Pos: {Input.absoluteMousePosition}", layout.AddElement());
				// GFX.DrawText($"Keyboard String: {Util.RemoveBadChars(Input.keyboardString)}", layout.AddElement());
				// if (Input.GamepadConnected(0))
				// {
				// 	GFX.DrawText("--- Gamepad ---", layout.AddElement());
				// 	GFX.DrawText($"LStick: {Input.GamepadGetLeftStick(0)}", layout.AddElement());
				// 	GFX.DrawText($"RStick: {Input.GamepadGetRightStick(0)}", layout.AddElement());
				// }
				// else
				// 	GFX.DrawText($"No Gamepad(s) Connected!", layout.AddElement());
			}

			if (Math.Abs(mem - lastMem) > 1000000)
				lastMem = mem;
		}
	}
}