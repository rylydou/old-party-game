using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;
using BinarySerialization;

namespace MGE.FileIO
{
	public static class IO
	{
		static BinarySerializer _bs;
		public static BinarySerializer bs
		{
			get
			{
				if (_bs == null)
					_bs = new BinarySerializer();
				return _bs;
			}
		}

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
			if (obj is ISerializable sb) sb.OnBeforeSerilize();

			using (var fs = File.Open(IO.ParsePath(path), FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
			{
				// bs.Serialize(fs, obj);
				bf.Serialize(fs, obj);
			}

			if (obj is ISerializable sa) sa.OnAfterSerilize();
		}

		public static T Load<T>(string path)
		{
			using (var fs = File.Open(IO.ParsePath(path), FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
			{
				var obj = (T)bf.Deserialize(fs);

				// var obj = bs.Deserialize<T>(fs);

				if (obj is ISerializable da) da.OnAfterDeserilize();

				return obj;
			}
		}

		public static void SaveJson(string path, object obj)
		{
			if (obj is ISerializable sb) sb.OnBeforeSerilize();

			using (var fs = File.Open(IO.ParsePath(path), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
			{
				var text = Serializer.SerializeJson(obj);
				fs.Write(Encoding.ASCII.GetBytes(text), 0, text.Length);
			}

			if (obj is ISerializable sa) sa.OnBeforeSerilize();
		}

		public static T LoadJson<T>(string path)
		{
			using (var fs = File.Open(IO.ParsePath(path), FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
			{
				var obj = Serializer.DeserializeJson<T>(File.ReadAllText(IO.ParsePath(path)));

				if (obj is ISerializable da) da.OnAfterDeserilize();

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