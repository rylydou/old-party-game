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
				if (Config.typeToExtention.ContainsValue(IO.GetFullExt(file.Value)))
				{
					object asset = LoadAsset(file.Value, file.Key);

					if (asset == null)
						Logger.LogError($"Asset {file.Key} is null!");
					else
					{
						if (preloadedAssets.ContainsKey(file.Key))
						{
							Logger.Log($": {file.Key}");
							preloadedAssets[file.Key] = asset;
						}
						else
						{
							Logger.Log($"+ {file.Key}");
							preloadedAssets.Add(file.Key, asset);
						}
					}
				}
			}
		}

		static object LoadAsset(string path, string relitivePath)
		{
			object asset = null;

			switch (IO.GetFullExt(path))
			{
				// # Texture
				case ".psd":
					using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
					{
						asset = Texture.FromStream(fs);
					}
					break;
				case ".spritesheet.psd":
					using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
					{
						var tex = Texture.FromStream(fs);
						var info = IO.LoadJson<SpriteSheet>(path + ".info");
						info.texture = tex;
						asset = info;
					}
					break;
				case ".tileset.psd":
					using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
					{
						var tex = Texture.FromStream(fs);
						var info = IO.LoadJson<Tileset>(path + ".info");
						info.texture = tex;
						asset = info;
					}
					break;
				// # Audio
				case ".wav":
					using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
					{
						asset = SoundEffect.FromStream(fs);
					}
					break;
				case ".sound":
					var sound = IO.LoadJson<Sound>(path);
					sound.path = relitivePath.Replace(".sound", string.Empty);
					asset = sound;
					break;
				// # Mics
				case ".font.psd":
					using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
					{
						var tex = Texture.FromStream(fs);
						var info = IO.LoadJson<Font>(path + ".info");
						info.texture = tex;
						asset = info;
					}
					break;
				case ".feilds":
					asset = IO.LoadJson<Feilds>(path);
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
				if (!Config.typeToExtention.ContainsValue(IO.GetFullExt(file))) continue;

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
			if (!path.Contains('.')) path += Config.typeToExtention[typeof(T)];

			if (preloadedAssets.ContainsKey(path))
				return preloadedAssets[path] as T;
			return null;
		}

		public static T[] GetAssets<T>(string path) where T : class
		{
			var assets = new List<T>();

			foreach (var asset in preloadedAssets)
			{
				if (asset.Key.StartsWith(path))
				{
					if (asset.Value is T a)
						assets.Add(a);
				}
			}

			return assets.ToArray();
		}

		public static T LoadAsset<T>(string path) where T : class
		{
			if (unloadedAssets.ContainsKey(path))
				return LoadAsset(unloadedAssets[path], path) as T;
			return null;
		}
		#endregion
	}
}