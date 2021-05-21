using System.Collections.Generic;
using MGE.FileIO;

namespace MGE
{
	public static class Settings
	{
		static Dictionary<string, object> data = new Dictionary<string, object>();

		public static bool dirty { get; private set; }

		public static T Get<T>(string name, T defualtValue = default)
		{
			try
			{
				return (T)data.TryGetValue(name, defualtValue);
			}
			catch
			{
				return defualtValue;
			}
		}

		public static bool Set(string name, object value)
		{
			dirty = true;

			if (!data.TryAdd(name, value))
			{
				data[name] = value;

				return false;
			}

			return true;
		}

		public static bool Toggle(string name, bool defualtState = true)
		{
			return Set(name, !Get<bool>(name, defualtState));
		}

		public static bool Load()
		{
			if (IO.FileExists("settings.json"))
			{
				data = IO.LoadJson<Dictionary<string, object>>("settings.json");

				if (data is null)
					data = new Dictionary<string, object>();

				return true;
			}
			else
			{
				data = new Dictionary<string, object>();

				Save();

				return false;
			}
		}

		public static void Save()
		{
			IO.SaveJson("settings.json", data);
		}
	}
}