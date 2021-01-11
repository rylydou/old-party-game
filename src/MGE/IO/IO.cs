using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using MGE;

namespace MGE.IO
{
	public struct IO
	{
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
			Util.ParsePath(ref path, true);

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
			Util.ParsePath(ref path, true);

			if (!File.Exists(path)) return default(T);

			return JsonSerializer.Deserialize<T>(File.ReadAllText(path), jsonOptions);
		}
	}
}