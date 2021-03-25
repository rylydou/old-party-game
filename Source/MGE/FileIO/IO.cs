using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace MGE.FileIO
{
	public static class IO
	{
		static BinaryFormatter _bf;
		public static BinaryFormatter bf
		{
			get
			{
				if (_bf == null)
					_bf = new BinaryFormatter() { AssemblyFormat = FormatterAssemblyStyle.Simple, TypeFormat = FormatterTypeStyle.TypesWhenNeeded, FilterLevel = TypeFilterLevel.Full };
				return _bf;
			}
		}

		public static string basePath { get; } = App.exePath + "/";

		#region File
		public static FileStream FileOpen(string path, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.Read)
		{
			path = basePath + path;
			return File.Open(path, mode, access, share);
		}

		public static FileStream FileOpenRead(string path, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.Read, FileShare share = FileShare.Read)
		{
			return FileOpen(path, mode, access, share);
		}

		public static FileStream FileOpenWrite(string path, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.Read)
		{
			return FileOpen(path, mode, access, share);
		}

		public static FileStream FileCreate(string path)
		{
			path = basePath + path;
			return File.Create(path);
		}

		public static FileStream FileCreate(string path, int bufferSize, FileOptions options = FileOptions.None)
		{
			path = basePath + path;
			return File.Create(path, bufferSize, options);
		}

		public static void FileDelete(string path)
		{
			File.Delete(path);
		}

		public static bool FileExists(string path)
		{
			path = basePath + path;
			return File.Exists(path);
		}

		public static void FileMove(string from, string to, bool overwrite = true)
		{
			from = basePath + from;
			to = basePath + to;

			File.Move(from, to, overwrite);
		}

		public static void FileCopy(string from, string to, bool overwrite = true)
		{
			from = basePath + from;
			to = basePath + to;

			File.Copy(from, to, overwrite);
		}

		public static string FileGetName(string path)
		{
			return new FileInfo(path).Name;
		}

		public static string FileGetParent(string path)
		{
			return CleanPath(new FileInfo(path).Directory.FullName);
		}
		#endregion

		#region Folder
		public static string FolderGetParent(string path)
		{
			return CleanPath(new DirectoryInfo(path).Parent.FullName);
		}
		#endregion

		#region Saving & Loading
		public static void Save(string path, object obj, bool isFullPath = true)
		{
			if (!isFullPath)
				path = basePath + path;

			using (var fs = File.Open(IO.ParsePath(path), FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
			{
				bf.Serialize(fs, obj);
			}
		}

		public static T Load<T>(string path, bool isFullPath = true)
		{
			if (!isFullPath)
				path = basePath + path;

			using (var fs = File.Open(IO.ParsePath(path), FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
			{
				var obj = (T)bf.Deserialize(fs);

				return obj;
			}
		}

		public static void SaveJson(string path, object obj)
		{
			using (var fs = File.Open(IO.ParsePath(path), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
			{
				var text = Serializer.SerializeJson(obj);
				fs.Write(Encoding.ASCII.GetBytes(text), 0, text.Length);
			}
		}

		public static T LoadJson<T>(string path)
		{
			using (var fs = File.Open(IO.ParsePath(path), FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
			{
				var obj = Serializer.DeserializeJson<T>(File.ReadAllText(IO.ParsePath(path)));

				return obj;
			}
		}
		#endregion

		#region Utils
		public static FieldInfo[] GetAllFields(Type type, int maxDepth = 8)
		{
			var fields = new List<FieldInfo>();
			GetFields(ref fields, type, maxDepth, 0);
			return fields.ToArray();
		}

		public static void GetFields(ref List<FieldInfo> fields, Type type, int maxDepth = 8, int depth = 0)
		{
			var fieldsToAdd = type.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (var field in fieldsToAdd)
				fields.Add(field);

			foreach (var field in fields)
				GetFields(ref fields, field.GetType(), maxDepth, depth);

			if (depth++ > maxDepth)
				return;
		}

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