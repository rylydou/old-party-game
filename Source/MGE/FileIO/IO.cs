using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using MGE;

namespace MGE.FileIO
{
	public struct IO
	{
		public static string GetInfoFileText(string file)
		{
			return File.ReadAllText(file + Config.infoFileExt);
		}

		public static string[] GetInfoFileLines(string file)
		{
			return File.ReadAllLines(file + Config.infoFileExt);
		}

		#region Saving & Loading
		public static void Save(string path, object obj)
		{
			using (var fs = File.Open(IO.ParsePath(path), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				// var bf = new BinaryFormatter();
				// bf.Serialize(fs, obj);

				var text = Serializer.Serialize(obj);
				fs.Write(Encoding.ASCII.GetBytes(text), 0, text.Length);
			}
			File.WriteAllText(IO.ParsePath(path), Serializer.Serialize(obj));
		}

		public static T Load<T>(string path, T obj) where T : ISerializable
		{
			using (var fs = File.Open(IO.ParsePath(path), FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
			{
				return Serializer.Deserialize<T>(File.ReadAllText(IO.ParsePath(path)));
			}
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
					path = App.exePath + "/" + path.Replace("//", "Assets/");
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