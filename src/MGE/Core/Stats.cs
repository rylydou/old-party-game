using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace MGE
{
	public struct Stats
	{
		public static double fps = 0.0;
		public static Queue<int> fpsHistory = new Queue<int>();
		public static double averageFps
		{
			get
			{
				if (fpsHistory.Count == 0)
					return 0.0;
				else
					return fpsHistory.Average();
			}
		}
		public static double minFps
		{
			get
			{
				if (fpsHistory.Count == 0)
					return 0.0;
				else
					return fpsHistory.Min();
			}
		}

		public static void Update()
		{
			fps = 1.0 / Time.deltaTime;

			fpsHistory.Enqueue((int)fps);
			if (fpsHistory.Count > MGEConfig.fpsHistorySize)
				fpsHistory.Dequeue();
		}
	}
}