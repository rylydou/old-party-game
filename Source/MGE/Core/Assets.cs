using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MGE.FileIO;

namespace MGE
{
	public class Assets
	{
		public static string[] activeRP = new string[] { };

		public static Dictionary<string, object> preloadedAssets = new Dictionary<string, object>();
		public static Dictionary<string, string> unloadedAssets = new Dictionary<string, string>();

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
				if (MGEConfig.typeToExtention.ContainsValue(IO.GetFullExt(file.Value)))
				{
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
						asset = Texture2D.FromStream(Engine.game.GraphicsDevice, fs);
						Logger.LogWarning($"Upgrade {path} to a psd");
					}
					break;
				case ".psd":
					using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
					{
						asset = Texture2D.FromStream(Engine.game.GraphicsDevice, fs);
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
				case ".font.psd":
					using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
					{
						var fontTex = Texture2D.FromStream(Engine.game.GraphicsDevice, fs);
						var info = IO.GetInfoFileLines(path);

						var bounds = new List<Rectangle>();
						var croppings = new List<Rectangle>();
						var chars = new List<char>();
						var kernings = new List<Vector3>();

						chars = info.Select((x) => x[0]).ToList();

						for (int i = 0; i < chars.Count; i++)
						{
							bounds.Add(new Rectangle(i * 10, 0, 10, 16));
							croppings.Add(new Rectangle(0, 0, 0, 0));
							kernings.Add(new Vector3(0, 12, 0));
						}

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
			foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
			{
				if (!MGEConfig.typeToExtention.ContainsValue(IO.GetFullExt(file))) continue;

				string relitivePath = folder.GetRelitivePath(file);

				if (relitivePath.Contains('@'))
				{
					if (unloadedAssets.ContainsKey(relitivePath))
						unloadedAssets[relitivePath] = file;
					else
						unloadedAssets.Add(relitivePath, file);
				}
				else
				{
					if (filesIndex.ContainsKey(relitivePath))
						filesIndex[relitivePath] = file;
					else
						filesIndex.Add(relitivePath, file);
				}
			}
		}
		#endregion

		#region Asset Getting
		public static T GetAsset<T>(string path) where T : class
		{
			if (!path.Contains('.')) path += MGEConfig.typeToExtention[typeof(T)];

			if (preloadedAssets.ContainsKey(path))
				return preloadedAssets[path] as T;

			Logger.LogError($"Can't find asset \"{path}\"!");
			return null;
		}

		public static T LoadAsset<T>(string path) where T : class
		{
			if (unloadedAssets.ContainsKey(path))
				return LoadAsset(unloadedAssets[path]) as T;

			Logger.LogError($"Can't find asset \"{path}\"!");
			return null;
		}
		#endregion
	}
}