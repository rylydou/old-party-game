using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MGE.FileIO;

namespace MGE
{
	public class Assets : EssentialVars
	{
		public static string[] activeRP = new string[] { };

		static Dictionary<string, object> preloadedAssets = new Dictionary<string, object>();
		static Dictionary<string, string> unloadedAssets = new Dictionary<string, string>();

		public static void ReloadAssets()
		{
			UnloadAssets();

			var index = new Dictionary<string, string>();

			ScanAssetsInRP(new Folder(IO.ParsePath("//", true)), ref index);

			foreach (var rp in activeRP)
				ScanAssetsInRP(new Folder(IO.ParsePath($"/Resource Packs/{rp}/Assets", true)), ref index);

			LoadAssetsFromIndex(in index);
		}

		public static void UnloadAssets()
		{
			foreach (var asset in preloadedAssets)
				if (asset.Value is System.IDisposable disposable)
					disposable.Dispose();

			preloadedAssets = new Dictionary<string, object>();
			unloadedAssets = new Dictionary<string, string>();
		}

		static void ScanAssetsInRP(Folder folder, ref Dictionary<string, string> index)
		{
			ScanDir(folder.path, ref index, ref folder);
		}

		static void LoadAssetsFromIndex(in Dictionary<string, string> index)
		{
			foreach (var file in index)
			{
				var ext = new FileInfo(file.Value).Extension;
				switch (ext)
				{
					case ".info": break;
					case ".gitkeep": break;
					default:
						object asset = LoadAsset(file.Value);

						if (asset == null)
							Logger.LogError($"Asset {file.Key} is null!");
						else
						{
							if (preloadedAssets.ContainsKey(file.Key))
							{
								Logger.Log($"Replacing: {file.Key}");
								preloadedAssets[file.Key] = asset;
							}
							else
							{
								Logger.Log($"Adding: {file.Key}");
								preloadedAssets.Add(file.Key, asset);
							}
						}
						break;
				}
			}
		}

		static object LoadAsset(string path)
		{
			object asset = null;

			switch (IO.GetFullExt(path))
			{
				// > Image
				case ".png":
					using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
					{
						asset = Texture2D.FromStream(graphicsDevice, fs);
					}
					break;
				// > Audio
				case ".wav":
					using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
					{
						asset = SoundEffect.FromStream(fs);
					}
					break;
				// > Font
				case ".font.png":
					using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
					{
						var fontTex = Texture2D.FromStream(graphicsDevice, fs);
						var info = IO.GetInfoFileLines(path);

						var bounds = new List<Rectangle>();
						var croppings = new List<Rectangle>();
						var chars = new List<char>();
						var kernings = new List<Vector3>();

						chars = info.Select((x) => x[0]).ToList();

						for (int i = 0; i < info.Length; i++)
						{
							bounds.Add(new Rectangle(0, i * 10, 10, 16));
							croppings.Add(new Rectangle(0, 0, 0, 0));
							kernings.Add(new Vector3(0, 16, 0));
						}

						Logger.Log($"Info: {info.Length}, Rects: {bounds.Count}, Kernings: {kernings.Count}, Chars: {chars.Count}");

						asset = new SpriteFont(fontTex, bounds, croppings, chars, 16, 0, kernings, null);
					}
					break;
				default:
					Logger.LogWarning($"Cannot read file {path}");
					break;
			}

			return asset;
		}

		#region Dir Scaning
		static void ScanDir(string path, ref Dictionary<string, string> filesIndex, ref Folder folder)
		{
			foreach (string file in Directory.GetFiles(path))
			{
				string relitivePath = folder.GetRelitivePath(file);

				if (filesIndex.ContainsKey(relitivePath))
					filesIndex[relitivePath] = file;
				else
					filesIndex.Add(folder.GetRelitivePath(file), file);
			}

			foreach (string directory in Directory.GetDirectories(path))
			{
				if (directory.Contains("No Preload"))
					ScanDirAddToUnloaded(directory, ref folder);

				ScanDir(directory, ref filesIndex, ref folder);
			}
		}

		static void ScanDirAddToUnloaded(string path, ref Folder folder)
		{
			foreach (string file in Directory.GetFiles(path))
			{
				string relitivePath = folder.GetRelitivePath(file);

				if (unloadedAssets.ContainsKey(relitivePath))
					unloadedAssets[relitivePath] = file;
				else
					unloadedAssets.Add(folder.GetRelitivePath(file), file);
			}

			foreach (string directory in Directory.GetDirectories(path))
			{
				ScanDirAddToUnloaded(directory, ref folder);
			}
		}
		#endregion

		#region Asset Getting
		public static T GetAsset<T>(string path) where T : class
		{
			if (preloadedAssets.ContainsKey(path))
				return preloadedAssets[path] as T;

			Logger.LogError($"Can't Find Texture \"{path}\"!");
			return null;
		}

		public static T LoadAsset<T>(string path) where T : class
		{
			if (unloadedAssets.ContainsKey(path))
				return LoadAsset(unloadedAssets[path]) as T;

			Logger.LogError($"Can't Find Texture \"{path}\"!");
			return null;
		}
		#endregion
	}
}