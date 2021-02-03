using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MGE.FileIO;

namespace MGE
{
	public static class App
	{
		public static string _exePath;
		public static string exePath
		{
			get
			{
				if (string.IsNullOrEmpty(_exePath))
					_exePath = IO.CleanPath(Environment.CurrentDirectory);
				return _exePath;
			}
		}

		public static string dataPath { get => MGEConfig.saveDataPath; }
	}
}