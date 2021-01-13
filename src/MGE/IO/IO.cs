using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using MGE;

namespace MGE.FileIO
{
	public struct IO
	{
		#region Saving & Loading
		static JsonSerializerOptions _jsonOptions;
		static JsonSerializerOptions jsonOptions
		{
			get
			{
				if (_jsonOptions == null)
					_jsonOptions = new JsonSerializerOptions()
					{
						WriteIndented = true,
						ReadCommentHandling = JsonCommentHandling.Skip
					};
				return _jsonOptions;
			}
		}

		public static bool SaveAsJson<T>(string path, T value)
		{
			bool createdNewFile = false;
			IO.ParsePath(ref path, true);

			if (File.Exists(path))
			{
				createdNewFile = true;
				File.Create(path);
			}

			var json = JsonSerializer.Serialize<T>(value, jsonOptions);

			Logger.Log(json);

			File.WriteAllText(path, json);

			return createdNewFile;
		}

		public static T LoadFromJson<T>(string path)
		{
			IO.ParsePath(ref path, true);

			if (!File.Exists(path)) return default(T);

			return JsonSerializer.Deserialize<T>(File.ReadAllText(path), jsonOptions);
		}
		#endregion

		#region Utils
		public static string ParsePath(string path, bool full = false)
		{
			ParsePath(ref path, full);
			return path;
		}

		public static void ParsePath(ref string path, bool full = false)
		{
			if (full)
			{
				if (path.StartsWith("//"))
					path = App.exePath + "/" + path.Replace("//", "Content/Assets/");
				else if (path.StartsWith('/'))
					path = App.exePath + path;
			}
			else
			{
				if (path.StartsWith("//"))
					path = path.Replace("//", "Assets/");
				else if (path.StartsWith('/'))
					path = path.Remove(0, 1);
			}

			CleanPath(path);
		}

		public static string CleanPath(string path)
		{
			CleanPath(ref path);
			return path;
		}

		public static void CleanPath(ref string path)
		{
			path = path.Replace('\\', '/');
		}

		public static string GetFullExt(string file)
		{
			return file.Substring(file.IndexOf('.'));
		}
		#endregion
	}
}