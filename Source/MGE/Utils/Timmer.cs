using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace MGE
{
	public struct Timmer : IDisposable
	{
		#region Static

		#region Variables
		static List<Timmer> timmers = new List<Timmer>();
		#endregion

		#region Creation
		public static Timmer Create() => new Timmer(string.Empty);

		public static Timmer Create(string name) => new Timmer(name);
		#endregion

		#region Management
		public static void Start(string name)
		{
			if (timmers.Exists((x) => x.name == name))
			{
				Logger.LogError($"Timmer \"{name}\" already exists!");
				return;
			}

			timmers.Add(Create(name));
		}

		public static long Stop(string name)
		{
			var timmerIndex = timmers.FindIndex((x) => x.name == name);
			if (timmerIndex == -1)
			{
				Logger.LogError($"Can't find timmer \"{name}\"");
				return -1;
			}
			else
			{
				var timmer = timmers[timmerIndex];
				timmers.RemoveAt(timmerIndex);
				return timmer.elapsedTime;
			}
		}

		public static void Log(string name)
		{
			var timmerIndex = timmers.FindIndex((x) => x.name == name);
			if (timmerIndex == -1)
				Logger.LogError($"Can't find timmer \"{name}\"");
			else
			{
				var timmer = timmers[timmerIndex];
				timmers.RemoveAt(timmerIndex);
				timmer.LogTime();
			}
		}
		#endregion

		#endregion

		#region Object

		#region Variables
		string _name;
		internal string name { get => _name; set => _name = value; }

		Stopwatch _timmer;
		#endregion

		#region Properties
		public long time { get { _timmer.Stop(); return _timmer.ElapsedMilliseconds; } }

		public long elapsedTime { get { _timmer.Stop(); return _timmer.ElapsedMilliseconds; } }
		#endregion

		#region Constructors
		public Timmer(string name = "")
		{
			this._name = name;
			this._timmer = new Stopwatch();
			this._timmer.Start();
		}
		#endregion

		#region Methods
		public void Start() => _timmer.Start();
		public void Stop() => _timmer.Stop();
		public void Restart() => _timmer.Restart();

		public void LogTime()
		{
			Logger.Log($"⏰ {name} Timmer: {elapsedTime}ms");
		}
		#endregion

		#region Inherited Methods
		public void Dispose() => LogTime();
		#endregion

		#endregion
	}
}