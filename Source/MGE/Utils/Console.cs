using System;
using Microsoft.Xna.Framework.Graphics;
using MGE.UI.Layouts;
using MGE.InputSystem;
using MGE.Graphics;

namespace MGE
{
	public class Terminal
	{
		public static SpriteFont font { get => MGEConfig.defualtFont; }

		public static void Draw()
		{
			using (var layout = new StackLayout(new Vector2Int(8), 20, false))
			{
				GFX.Text($"{Util.CleanRound(Stats.fps)} / {Util.CleanRound(Stats.averageFps)} / {Util.CleanRound(Stats.minFps)}", layout.AddElement(), MGEConfig.FpsToColor((int)Stats.fps));
				// GFX.Text("--- General ---", layout.AddElement());
				GFX.Text($"Mem: {GC.GetTotalMemory(false) / 1000000}MB / {Environment.WorkingSet / 1000000}MB", layout.AddElement());
				// GFX.Text($"Time running: {Math.Round(Time.time, 2)}", layout.AddElement());
				// GFX.Text("--- Graphics ---", layout.AddElement());
				// GFX.Text($"Window Mode: {Window.windowMode}", layout.AddElement());
				// GFX.Text($"Scale Up: {Camera.main.scaleUpFactor}", layout.AddElement());
				// GFX.Text($"Scale Down: {Camera.main.scaleDownFactor}", layout.AddElement());
				// GFX.Text($"Draw Calls: {GFX.drawCalls}", layout.AddElement());
				// GFX.Text("--- Asset ---", layout.AddElement());
				// GFX.Text($"Preloaded: {Assets.preloadedAssets.Count}", layout.AddElement());
				// GFX.Text($"Unloaded: {Assets.unloadedAssets.Count}", layout.AddElement());
				GFX.Text("--- Scene ---", layout.AddElement());
				GFX.Text($"Entities: {SceneManager.activeScene.entityCount}", layout.AddElement());
				GFX.Text($"Components: {SceneManager.activeScene.componentCount}", layout.AddElement());
				// GFX.Text("--- Input ---", layout.AddElement());
				// GFX.Text($"Win Mouse Pos: {Input.windowMousePosition}" + (Input.isMouseInWindow ? "" : "!"), layout.AddElement());
				// GFX.Text($"Cam Mouse Pos: {Input.cameraMousePosition}", layout.AddElement());
				// GFX.Text($"Abs Mouse Pos: {Input.absoluteMousePosition}", layout.AddElement());
				// GFX.Text($"Keyboard String: {Util.RemoveBadChars(Input.keyboardString)}", layout.AddElement());
				// if (Input.GamepadConnected(0))
				// 	GFX.Text($"Gamepad: {Input.GamepadGetLeftStick(0)}", layout.AddElement());
				// else
				// 	GFX.Text($"No Gamepad(s) Connected!", layout.AddElement());
			}
		}
	}
}