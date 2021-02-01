using System;
using System.Diagnostics.Tracing;

namespace MGE
{
	public struct Logger
	{
		public static bool throwOnError = false;

		public static void Log(object obj)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] - > {obj.ToString()}");
		}

		public static void LogWarning(object obj)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ? > {obj.ToString()}");
		}

		public static void LogError(object obj)
		{
			var text = $"[{DateTime.Now.ToString("HH:mm:ss")}] ! > {obj.ToString()}";
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(text);

			if (throwOnError)
				throw new Exception(text);
		}

		public static void ClearLog() => Console.Clear();
	}
}