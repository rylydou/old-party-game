using System.Collections.Generic;
using System.Runtime.Serialization;
using GAME.Tiles;
using MGE;
using MGE.FileIO;

namespace GAME
{
	[System.Serializable]
	public class Stage : Asset
	{
		public override string extension => ".stage";

		static Tileset[] _backgroundTilesets;
		public static Tileset[] backgroundTilesets
		{
			get
			{
				if (_backgroundTilesets is null)
					_backgroundTilesets = new Tileset[]
					{
						null,
					};

				return _backgroundTilesets;
			}
		}

		static (Tile, Tileset)[] _tilesets;
		public static (Tile, Tileset)[] tilesets
		{
			get
			{
				if (_tilesets is null)
					_tilesets = new (Tile, Tileset)[]
					{
						(new Air(), null),
						(new Solid(), Assets.GetAsset<Tileset>("Tilesets/Basic")),
						(new Solid(), Assets.GetAsset<Tileset>("Tilesets/Grass")),
						(new Solid(), Assets.GetAsset<Tileset>("Tilesets/Stone")),
						(new Lava(), Assets.GetAsset<Tileset>("Tilesets/Lava")),
						(new Solid(), Assets.GetAsset<Tileset>("Tilesets/Sand")),
						(new Semisolid(), Assets.GetAsset<Tileset>("Tilesets/Semisolid")),
					};

				return _tilesets;
			}
		}

		public static readonly Vector2Int size = new Vector2Int(40, 23);

		public string name = "Untitled Stage";

		public Grid<(byte, byte)> tilesBackground;
		public Grid<byte> tiles;

		public List<Vector2Int> playerSpawnPoints = new List<Vector2Int>();
		public List<Vector2Int> crateSpawnsPoints = new List<Vector2Int>();

		public int fogDetail = 6;
		public float fogSpeed = 6.0f;
		public float fogHeight = 0.75f;
		public float fogSize = 32.0f;
		public Color fogColor = new Color(0.95f, 0.75f);

		public Stage() { }

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

				if (index == 0) continue;

				var rects = new Grid<RectInt>(tiles.size);

				tileset.Item2.GetTiles(ref rects, (x, y) => tiles.Get(x, y) == index, (x, y) => tiles.IsInBounds(x, y) ? tiles[x, y] != 0 : true);
				tileset.Item2.DrawTiles(rects, position, color);
			}
		}

		public override void Load(string fullPath, string localPath = null)
		{
			base.Load(fullPath, localPath);

			var s = IO.Load<Stage>(fullPath);

			name = s.name;
			tilesBackground = s.tilesBackground;
			tiles = s.tiles;
			playerSpawnPoints = s.playerSpawnPoints;
			crateSpawnsPoints = s.crateSpawnsPoints;

			fogDetail = s.fogDetail;
			fogSpeed = s.fogSpeed;
			fogHeight = s.fogHeight;
			fogSize = s.fogSize;
			fogColor = s.fogColor;
		}

		public override void Save(string fullPath)
		{
			base.Save(fullPath);

			IO.Save(fullPath, this, false);
		}

		[OnDeserialized]
		public void OnDeserialized(StreamingContext context)
		{
			if (tilesBackground is null) tilesBackground = new Grid<(byte, byte)>(size);

			if (fogDetail < 1)
			{
				fogDetail = 6;
				fogSpeed = 6.0f;
				fogHeight = 0.75f;
				fogSize = 32.0f;
				fogColor = new Color(0.95f, 0.75f);
			}

			if (playerSpawnPoints is null)
				playerSpawnPoints = new List<Vector2Int>();

			if (crateSpawnsPoints is null)
				crateSpawnsPoints = new List<Vector2Int>();
		}
	}
}