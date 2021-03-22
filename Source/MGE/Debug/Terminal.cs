using System;
using Microsoft.Xna.Framework.Graphics;
using MGE.UI.Layouts;
using MGE.InputSystem;
using MGE.Graphics;

namespace MGE.Debug
{
	public static class Terminal
	{
		public static bool enabled = true;

		static Font font { get => Config.font; }

		static long lastMem = int.MaxValue;

		public static void Draw()
		{
			if (!enabled) return;

			var mem = GC.GetTotalMemory(false);

			using (var layout = new StackLayout(new Vector2Int(8, 64), 20, false))
			{
				font.DrawText($"{Util.CleanRound(Stats.fps)} / {Util.CleanRound(Stats.averageFps)} / {Util.CleanRound(Stats.minFps)}", layout.AddElement(), Config.FpsToColor((int)Stats.fps));
				font.DrawText($"Mem: {mem / 1000000}MB / {Environment.WorkingSet / 1000000}MB" + (mem - lastMem > 0 ? " +" : " -"), layout.AddElement(), Colors.text);
			}

			if (Math.Abs(mem - lastMem) > 1000000)
				lastMem = mem;
		}
	}
}