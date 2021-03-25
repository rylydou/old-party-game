using System.Collections.Generic;
using MGE.FileIO;

namespace MGE.StageSystem
{
	[System.Serializable]
	public class World
	{
		public Vector2Int size;
		public Vector2Int levelSize;

		public Grid<Level> loadedLevels;
		public Grid<bool> availableLevels;

		public string path;

		public World()
		{
			size = new Vector2Int(8, 8);
			levelSize = new Vector2Int(64, 64);
		}

		public void SetLevel(Level level, Vector2Int position)
		{
			level.world = this;
			level.position = position;

			loadedLevels[position] = level;

			SaveLevel(position);
		}

		public void UnloadLevel(Vector2Int position, bool save = false)
		{
			if (save)
			{
				SaveLevel(position);
			}
			loadedLevels[position] = null;
		}

		public bool SaveLevel(Vector2Int position)
		{
			if (!availableLevels.IsInBounds(position)) return false;

			if (availableLevels[position])
			{
				var level = loadedLevels[position];

				if (level == null) return false;

				IO.Save(GetLevelPath(position), level, false);
				return true;
			}

			return false;
		}

		public bool LoadLevel(Vector2Int position)
		{
			if (!availableLevels.IsInBounds(position)) return false;

			if (availableLevels[position])
			{
				loadedLevels[position] = IO.Load<Level>(GetLevelPath(position), false);
				return true;
			}
			return false;
		}

		public void Reload()
		{
			loadedLevels = new Grid<Level>(size, null);
			availableLevels = new Grid<bool>(size, false);

			availableLevels.For((x, y, state) =>
				availableLevels[x, y] = IO.FileExists(path)
			);
		}

		public void UnloadAllLevels(bool save = false)
		{
			if (save)
				loadedLevels.For((x, y) => SaveLevel(new Vector2Int(x, y)));

			loadedLevels = new Grid<Level>(size, null);
		}

		public string GetLevelPath(Vector2Int position)
		{
			return $"{path} Levels/{position.x} {position.y}";
		}
	}
}