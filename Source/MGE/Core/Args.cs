using System.Linq;
using System.Collections.Generic;
using System;

namespace MGE
{
	public static class Args
	{
		static List<string> _args;
		public static List<string> args
		{
			get
			{
				if (_args == null)
				{
					_args = Environment.GetCommandLineArgs().ToList();
					if (_args == null) _args = new List<string>();
				}
				return _args;
			}
		}

		public static bool HasFlag(string flag)
		{
			return args.Any((x) => GetFlagName(x) == flag);
		}

		public static string GetFlag(string flagName)
		{
			return args.Find((x) => GetFlagName(x) == flagName);
		}

		public static string[] GetFlagPerams(string flag)
		{
			if (HasFlag(flag)) return null;

			return GetFlagValue(GetFlag(flag)).Split(' ');
		}

		public static string GetFlagName(string flag)
		{
			int index = flag.IndexOf(':');

			if (index < 0)
				return flag;
			else
				return flag.Remove(index);
		}

		public static string GetFlagValue(string flag)
		{
			int index = flag.IndexOf(':');

			if (index < 0)
				throw new Exception("Flag has no name!");

			return flag.Substring(index + 2);
		}
	}
}