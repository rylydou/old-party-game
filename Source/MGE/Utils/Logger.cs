using System.IO;
using System;
using System.Diagnostics.Tracing;

namespace MGE
{
	public struct Logger
	{
		public const string logPath = @"\Logs\";

		public static bool throwOnError = false;

		static string _logFolderPath;
		public static string logFolderPath
		{
			get
			{
				if (string.IsNullOrEmpty(_logFolderPath))
					_logFolderPath = Environment.CurrentDirectory + logPath;
				return _logFolderPath;
			}
		}

		static StreamWriter _log = null;
		public static StreamWriter log
		{
			get
			{
				if (_log == null)
				{
					if (!Directory.Exists(logFolderPath)) Directory.CreateDirectory(logFolderPath);
					_log = new StreamWriter(logFolderPath + DateTime.Now.ToString(@"yyyy\-MM\-dd hh\.mm\.ss") + ".log", true);
					_log.AutoFlush = true;
				}
				return _log;
			}
		}

		public static void Log(object obj)
		{
			WriteToLog(obj.ToString());
		}

		public static void LogWarning(object obj)
		{
			WriteToLog(obj.ToString());
		}

		public static void LogError(object obj)
		{
			var text = obj.ToString();

			WriteToLog(text);

			if (throwOnError)
				throw new Exception(text);
		}

		public static void ClearLog()
		{
			Console.Clear();

			WriteToLogRaw("");
			WriteToLogRaw("");
		}

		public static void WriteToLog(string text)
		{
			Console.WriteLine(text);

			WriteToLogRaw($"[ {DateTime.Now.ToString(@"hh\:mm\:ss\.fff")} ]");
			WriteToLogRaw("> " + text);
			WriteToLogRaw("");
		}

		public static void WriteToLogRaw(string text)
		{
			// log.WriteLine(text);
		}
	}
}