using System.Collections.Generic;
using MGE;
using MGE.FileIO;

namespace GAME
{
	[System.Serializable]
	public class Stage
	{
		static Tileset[] _tilesets;
		public static Tileset[] tilesets
		{
			get
			{
				if (_tilesets is null)
					_tilesets = new Tileset[]
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
			try
			{
				using (Timmer.Start("Save Stage"))
					IO.Save($"Assets/Stages/{name}.stage", this, false);
			}
			catch (System.Exception e)
			{
				Logger.LogError(e);
			}
		}

		public static Stage Load(string name)
		{
			try
			{
				using (Timmer.Start("Load Stage"))
				{
					var stage = IO.Load<Stage>($"Assets/Stages/{name}.stage", false);
					if (stage.tiles.size != new Vector2Int(40, 23))
					{
						Logger.Log("Updating Stage...");

						var tiles = new Grid<byte>(40, 23);

						stage.tiles.For((x, y, tile) => tiles.Set(x, y, tile));

						stage.tiles = tiles;
					}
					return stage;
				}
			}
			catch (System.Exception e)
			{
				Logger.LogError(e);
			}

			return null;
		}
	}
}