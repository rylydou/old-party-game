using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MGE.UI;
using MGE.UI.Elements;
using MGE.UI.Layouts;
using MGE.InputSystem;

namespace MGE
{
	public class Terminal : EssentialVars
	{
		public static void Draw()
		{
			GUI.AddElement(new GUIStackLayout(new List<GUIElement>()
			{
				new GUIImage(null) { color = Color.red },
				new GUIImage(null) { color = Color.green },
				new GUIImage(null) { color = Color.blue },
			}, 64));

			using (var layout = new StackLayout(new Vector2Int(8), 20, false))
			{
				sb.DrawString(font, $"{Util.CleanRound(Stats.fps)} / {Util.CleanRound(Stats.averageFps)} / {Util.CleanRound(Stats.minFps)}", layout.AddElement(), MGEConfig.FpsToColor((int)Stats.fps));
				sb.DrawString(font, "--- General ---", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Mem: {GC.GetTotalMemory(false) / 1000000}MB / {Environment.WorkingSet / 1000000}MB", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Time running: {Math.Round(Time.time, 2)}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, "--- Graphics ---", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Window Mode: {Window.windowMode}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Scale Up: {camera.scaleUpFactor}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Scale Down: {camera.scaleDownFactor}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Draw Calls: {Graphics.Graphics.drawCalls}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, "--- Asset ---", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Preloaded: {Assets.preloadedAssets.Count}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Unloaded: {Assets.unloadedAssets.Count}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, "--- Scene ---", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Entities: {SceneManager.current.activeScene.entityCount}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Components: {SceneManager.current.activeScene.componentCount}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, "--- Input ---", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Abs Mouse Pos: {Input.absoluteMousePosition}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Win Mouse Pos: {Input.windowMousePosition}" + (Input.isMouseInWindow ? "" : "!"), layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Cam Mouse Pos: {Input.cameraMousePosition}", layout.AddElement(), MGEConfig.statsColor);
				sb.DrawString(font, $"Keyboard String: {Util.RemoveBadChars(Input.keyboardString)}", layout.AddElement(), MGEConfig.statsColor);
				if (Input.GamepadConnected(0))
					sb.DrawString(font, $"Gamepad: {Input.GamepadGetLeftStick(0)}", layout.AddElement(), MGEConfig.statsColor);
				else
					sb.DrawString(font, $"No Gamepad Connected!", layout.AddElement(), MGEConfig.statsColor);
			}
		}
	}
}