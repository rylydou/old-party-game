using System;
using System.Diagnostics.Tracing;

namespace MGE
{
	public struct Logger
	{
		public static void Log(object obj)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {obj.ToString()}");
		}

		public static void LogWarning(object obj)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] > {obj.ToString()} <");
		}

		public static void LogError(object obj)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ! > {obj.ToString()} < !");
		}

		public static void ClearLog() => Console.Clear();
	}
}