using System;
using System.Diagnostics;

namespace MGE
{
	public static class Test
	{
		public static void Time(string name, Action<int> code, int times = 100)
		{
			var sw = new Stopwatch();

			sw.Start();

			for (int i = 0; i < times; i++)
			{
				code.Invoke(i);
			}

			sw.Stop();

			Logger.Log($"Test Time {name} took {sw.ElapsedMilliseconds}ms about ~{(decimal)sw.ElapsedMilliseconds / times}ms per call, done {times} times");
		}
	}
}