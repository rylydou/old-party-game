using MGE.FileIO;

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

		[System.NonSerialized] public string path;
		public string levelPath { get => path + "Levels/"; }

		public static World FromFile(string path)
		{
			try
			{
				var world = IO.Load<World>(path, false);

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
			this.levelSize = new Vector2Int(32, 32);
		}

		public World(Vector2Int size, Vector2Int levelSize)
		{
			this.size = size;
			this.levelSize = levelSize;
		}

		public bool IsLevelInBounds(Vector2Int position)
		{
			return availableLevels.IsInBounds(position);
		}

		public bool IsLevelAvailable(Vector2Int position)
		{
			return availableLevels.Get(position);
		}

		public bool IsLevelLoaded(Vector2Int position)
		{
			return loadedLevels.Get(position) is object;
		}

		public Level LevelCreateNew(Vector2Int position)
		{
			if (IsLevelInBounds(position)) return null;

			var level = new Level();

			level.world = this;

			loadedLevels[position] = level;

			return level;
		}

		public Level GetLevel(Vector2Int position)
		{
			return loadedLevels.Get(position);
		}

		public bool SetLevel(Level level, Vector2Int position)
		{
			if (!IsLevelLoaded(position)) return false;

			level.world = this;
			level.position = position;

			loadedLevels[position] = level;

			return SaveLevel(position);
		}

		public bool SaveLevel(Vector2Int position)
		{
			if (!IsLevelLoaded(position)) return false;

			IO.Save(GetLevelPath(position), GetLevel(position), false);
			return true;
		}

		public bool LoadLevel(Vector2Int position)
		{
			if (!IsLevelAvailable(position)) return false;
			loadedLevels[position] = IO.Load<Level>(GetLevelPath(position), false);
			return true;
		}

		public bool UnloadLevel(Vector2Int position, bool save = false)
		{
			if (!IsLevelLoaded(position)) return false;

			if (save)
				SaveLevel(position);

			loadedLevels[position] = null;

			return true;
		}

		public void UnloadAllLevels(bool save = false)
		{
			if (save)
				loadedLevels.For((x, y) => SaveLevel(new Vector2Int(x, y)));

			loadedLevels = new Grid<Level>(size, null);
		}

		public void Reload()
		{
			loadedLevels = new Grid<Level>(size, null);
			availableLevels = new Grid<bool>(size, false);

			ScanForLevels();
		}

		public void ScanForLevels()
		{
			IO.FolderCreate(levelPath);

			availableLevels.For((x, y, state) =>
					availableLevels[x, y] = IO.FileExists(GetLevelPath(new Vector2Int(x, y)))
				);
		}

		public string GetLevelPath(Vector2Int position)
		{
			return $"{levelPath}{position.x} {position.y}.level";
		}
	}
}