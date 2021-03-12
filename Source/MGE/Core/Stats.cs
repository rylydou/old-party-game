using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace MGE
{
	public struct Stats
	{
		public static float fps { get; internal set; } = 0.0f;
		public static Queue<int> fpsHistory { get; internal set; } = new Queue<int>();
		public static float averageFps
		{
			get
			{
				if (fpsHistory.Count == 0)
					return 0.0f;
				else
					return (float)fpsHistory.Average();
			}
		}
		public static float minFps
		{
			get
			{
				if (fpsHistory.Count == 0)
					return 0.0f;
				else
					return fpsHistory.Min();
			}
		}

		internal static void Update()
		{
			fps = 1.0f / Time.deltaTime;

			fpsHistory.Enqueue((int)fps);
			if (fpsHistory.Count > Config.fpsHistorySize)
				fpsHistory.Dequeue();
		}
	}
}