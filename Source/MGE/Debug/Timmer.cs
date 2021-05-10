using System;

namespace MGE
{
	public class Timmer : IDisposable
	{
		public static Timmer Start(string name = "Timmer") => new Timmer(name);

		public readonly string name;

		public readonly DateTime startTime;
		public DateTime stopTime { get; private set; }

		public TimeSpan elapsedTime
		{
			get
			{
				Stop();
				return TimeSpan.FromTicks(stopTime.Ticks - startTime.Ticks);
			}
		}

		public Timmer(string name = "Timmer")
		{
			Logger.Log($"⏳ Starting {name}...");
			this.name = name;
			this.startTime = DateTime.Now;
		}

		public void Stop() => stopTime = DateTime.Now;

		public void LogTime() => Logger.Log($"⌛ {name} Timmer: {elapsedTime.ToString(@"s\.ffff")}s");

		public void Dispose() => LogTime();
	}
}