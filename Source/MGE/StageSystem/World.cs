using MGE.FileIO;
using MGE.StageSystem.Layers;
using System;

namespace MGE.StageSystem
{
	[System.Serializable]
	public class World
	{
		public Vector2Int size;
		public Vector2Int levelSize;
		public float tileSize;

		[System.NonSerialized] public Grid<Level> loadedLevels;
		[System.NonSerialized] public Grid<bool> availableLevels;
		[System.NonSerialized] public Grid<Texture> levelThumbnails;

		[System.NonSerialized] public string path;
		public string levelsPath { get => path + " Levels/"; }

		public static World FromFile(string path)
		{
			try
			{
				var world = IO.Load<World>(path);

				world.path = path;
				world.Reload();

				return world;
			}
			catch (System.Exception e)
			{
				Logger.MSGBox($"Error when loading world from {path}!", e.Message, System.Windows.Forms.MessageBoxIcon.Error);
				return null;
			}
		}

		public World()
		{
			this.size = new Vector2Int(8, 8);
			this.levelSize = new Vector2Int(16, 16);
			this.tileSize = 16;
		}

		public World(Vector2Int size, Vector2Int levelSize, float tileSize)
		{
			this.size = size;
			this.levelSize = levelSize;
			this.tileSize = tileSize;
		}

		public bool LevelIsInBounds(Vector2Int position)
		{
			return availableLevels.IsInBounds(position);
		}

		public bool LevelIsAvailable(Vector2Int position)
		{
			return availableLevels.Get(position);
		}

		public bool LevelIsLoaded(Vector2Int position)
		{
			return loadedLevels.Get(position) is object;
		}

		public Level LevelCreate(Vector2Int position)
		{
			if (!LevelIsInBounds(position)) return null;

			var level = new Level();

			level.world = this;
			level.name = $"Level {position}";

			loadedLevels[position] = level;

			level.LayerAdd(new IntLayer());

			availableLevels[position] = true;

			return level;
		}

		public bool LevelDelete(Vector2Int position)
		{
			if (LevelIsAvailable(position))
			{
				LevelUnload(position);
				availableLevels[position] = false;
				IO.FileDelete(LevelGetPath(position));
				return true;
			}
			return false;
		}

		public Level LevelGet(Vector2Int position)
		{
			return loadedLevels.Get(position);
		}

		public bool LevelSet(Level level, Vector2Int position)
		{
			if (!LevelIsLoaded(position)) return false;

			level.world = this;
			level.position = position;

			loadedLevels[position] = level;

			availableLevels[position] = true;

			return LevelSave(position);
		}

		public bool LevelSave(Vector2Int position)
		{
			if (!LevelIsLoaded(position)) return false;

			IO.Save(LevelGetPath(position), LevelGet(position), false);

			return true;
		}

		public bool LevelLoad(Vector2Int position)
		{
			if (!LevelIsAvailable(position)) return false;

			loadedLevels[position] = IO.Load<Level>(LevelGetPath(position), false);
			availableLevels[position] = true;

			return true;
		}

		public bool LevelUnload(Vector2Int position, bool save = false)
		{
			if (!LevelIsLoaded(position)) return false;

			if (save)
				LevelSave(position);

			loadedLevels[position] = null;

			return true;
		}

		public void LevelUnloadAll(bool save = false)
		{
			if (save)
				loadedLevels.For((x, y) => LevelSave(new Vector2Int(x, y)));

			loadedLevels = new Grid<Level>(size, null);
		}

		public void LevelSaveAll()
		{
			availableLevels.For((x, y) => LevelSave(new Vector2Int(x, y)));
		}

		public void LevelLoadAll()
		{
			ScanForLevels();
			availableLevels.For((x, y) => LevelLoad(new Vector2Int(x, y)));
		}

		public void Reload()
		{
			loadedLevels = new Grid<Level>(size, null);
			availableLevels = new Grid<bool>(size, false);
			levelThumbnails = new Grid<Texture>(size, null);

			ScanForLevels();
		}

		public void ScanForLevels()
		{
			IO.FolderCreate(levelsPath);

			availableLevels.For((x, y, state) =>
					availableLevels[x, y] = IO.FileExists(LevelGetPath(new Vector2Int(x, y)))
				);
		}

		public string LevelGetPath(Vector2Int position)
		{
			return $"{levelsPath}{position.x} {position.y}.level";
		}
	}
}