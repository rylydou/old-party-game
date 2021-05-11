using System.Collections.Generic;
using MGE;
using MGE.FileIO;

namespace GAME
{
	[System.Serializable]
	public class Stage
	{
		static List<Tileset> _tilesets;
		public static List<Tileset> tilesets
		{
			get
			{
				if (_tilesets is null)
					_tilesets = new List<Tileset>()
					{
						null,
						Assets.GetAsset<Tileset>("Tilesets/Basic"),
						Assets.GetAsset<Tileset>("Tilesets/Grass"),
						Assets.GetAsset<Tileset>("Tilesets/Stone"),
						Assets.GetAsset<Tileset>("Tilesets/Lava"),
						Assets.GetAsset<Tileset>("Tilesets/Sand"),
					};

				return _tilesets;
			}
		}

		public string name = "Untitled Stage";
		public string description = "( No Description )";

		public Grid<byte> tiles;

		public List<Vector2Int> spawnPoints = new List<Vector2Int>();

		public Stage(Vector2Int size)
		{
			tiles = new Grid<byte>(size, 0);
		}

		public void Draw(Vector2 position, Color color)
		{
			var index = -1;
			foreach (var tileset in tilesets)
			{
				index++;

				if (tileset is null) continue;

				var rects = new Grid<RectInt>(tiles.size);

				tileset.GetTiles(ref rects, (x, y) => tiles.Get(x, y) == index);
				tileset.DrawTiles(rects, position, color);
			}
		}

		public void Save()
		{
			Logger.Log("Saving...");
			IO.Save($"Assets/Stages/{name}.stage", this, false);
		}

		public static Stage Load(string name)
		{
			Logger.Log("Loading...");
			return IO.Load<Stage>($"Assets/Stages/{name}.stage", false);
		}
	}
}