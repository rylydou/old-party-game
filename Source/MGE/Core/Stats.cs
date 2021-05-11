using System.Linq;
using System.Collections.Generic;
using System;

namespace MGE
{
	public static class Stats
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

		public static long memUsed;
		public static float memUsedAsMBs { get => (float)((double)memUsed / 1048576); }

		public static long memAllocated;
		public static float memAllocatedAsMBs { get => (float)((double)memAllocated / 1048576); }

		internal static void Update()
		{
			fps = 1.0f / Time.deltaTime;

			fpsHistory.Enqueue((int)fps);
			if (fpsHistory.Count > Config.fpsHistorySize)
				fpsHistory.Dequeue();

			memUsed = GC.GetTotalMemory(false);
			memAllocated = Environment.WorkingSet;
		}
	}
}