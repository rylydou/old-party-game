using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MGE.FileIO;
using MGE.Graphics;
using System;

namespace MGE
{
	public static class Assets
	{
		public static List<string> resourcePackStack = new List<string> { };

		public static Dictionary<string, Asset> preloadedAssets = new Dictionary<string, Asset>();
		public static Dictionary<string, string> unloadedAssets = new Dictionary<string, string>();

		public static Dictionary<string, Type> assetExt2Type = new Dictionary<string, Type>();
		public static Dictionary<Type, string> assetType2Ext = new Dictionary<Type, string>();

		public static void RegisterAsset(Asset asset)
		{
			var type = asset.GetType();
			assetExt2Type.Add(asset.extension, type);
			assetType2Ext.Add(type, asset.extension);
		}

		public static void ReloadAssets()
		{
			UnloadAssets();

			var index = new Dictionary<string, string>();

			ScanAssetsInRP(new Folder(IO.ParsePath("//", true)), ref index);

			foreach (var rp in resourcePackStack)
				ScanAssetsInRP(new Folder(IO.ParsePath($"/Resource Packs/{rp}/Assets", true)), ref index);

			LoadAssetsFromIndex(in index);
		}

		public static void UnloadAssets()
		{
			foreach (var asset in preloadedAssets)
				if (asset.Value is System.IDisposable disposable)
					disposable.Dispose();

			preloadedAssets = new Dictionary<string, Asset>();
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
				if (assetExt2Type.ContainsKey(IO.GetFullExt(file.Value)))
				{
					var asset = LoadAsset(file.Value, file.Key);

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

		static Asset LoadAsset(string path, string localPath = null)
		{
			Asset asset = null;

			var ext = IO.GetFullExt(path);

			asset = (Asset)Activator.CreateInstance(assetExt2Type[ext]);

			asset.Load(path, localPath);

			return asset;
		}

		#region Dir Scaning
		static void ScanDir(string path, ref Dictionary<string, string> filesIndex, ref Folder folder)
		{
			foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
			{
				if (!assetExt2Type.ContainsKey(IO.GetFullExt(file))) continue;

				string relitivePath = folder.GetRelitivePath(file);

				if (relitivePath.Contains('@'))
				{
					if (unloadedAssets.ContainsKey(relitivePath))
					{
						Logger.Log($"@: {relitivePath}");
						unloadedAssets[relitivePath] = IO.CleanPath(file);
					}
					else
					{
						Logger.Log($"@+ {relitivePath}");
						unloadedAssets.Add(IO.CleanPath(relitivePath), IO.CleanPath(file));
					}
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
			if (!path.Contains('.')) path += assetType2Ext[typeof(T)];

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
			if (!path.Contains('.')) path += assetType2Ext[typeof(T)];

			if (unloadedAssets.ContainsKey(path))
				return LoadAsset(unloadedAssets[path], path) as T;
			return null;
		}

		public static string[] GetUnloadedAssets<T>(string path) where T : class
		{
			var assets = new List<string>();
			var ext = assetType2Ext[typeof(T)];

			foreach (var asset in unloadedAssets)
			{
				if (asset.Key.StartsWith(path) && asset.Key.EndsWith(ext))
					assets.Add(asset.Key.Replace(ext, string.Empty));
			}

			return assets.ToArray();
		}
		#endregion
	}
}