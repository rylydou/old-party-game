using System;
using System.Collections.Generic;
using MGE;
using GAME.World.Generation;
using Microsoft.Xna.Framework.Graphics;
using MGE.Graphics;
using MGE.UI;
using MGE.UI.Elements;
using MGE.InputSystem;

namespace GAME.World
{
	public class Grid
	{
		public static Grid current;

		public static readonly List<Tile> tiles = new List<Tile>()
		{
			new Air(),
			new Dirt(),
			new Stone(),
			new Sand(),
			new Water(),
			new Salt(),
			new Sodium(),
			new Smoke(),
			new Oil(),
			new Void(),
			new Acid(),
			new Vine(),
			new TNT(),
		};

		public bool highTick = true;

		public Tile[,] world;

		public Vector2 position;
		public double scale;

		public readonly Vector2Int size;

		static Tile _air;
		public static Tile air
		{
			get
			{
				if (_air == null) _air = new Air();
				return _air;
			}
		}

		public Grid(Vector2Int size, IGenerator generator)
		{
			current = this;

			this.size = size;
			this.position = Window.gameSize / 2 - size / 2;

			this.world = new Tile[size.x, size.y];
			new GenTest().Generate(ref world);
		}

		public Tile GetTile(Vector2Int position) => GetTile(position.x, position.y);
		public Tile GetTile(int x, int y)
		{
			if (x < 0 || x >= size.x || y < 0 || y >= size.y) return new Stone();
			return world[x, y];
		}

		public bool SetTile(Vector2Int position, Tile tile) => SetTile(position.x, position.y, tile);
		public bool SetTile(int x, int y, Tile tile)
		{
			if (x < 0 || x >= size.x || y < 0 || y >= size.y) return false;
			if (tile == null) tile = air;
			world[x, y] = tile;
			return true;
		}

		public bool TileIsType(Vector2Int position, Type type) => TileIsType(position.x, position.y, type);
		public bool TileIsType(int x, int y, Type type) => GetTile(x, y).GetType() == type;

		public bool MoveTile(Vector2Int from, Vector2Int to)
		{
			if (GetTile(to).density >= GetTile(from).density) return false;

			var tile = GetTile(from);
			SetTile(from, GetTile(to));
			SetTile(to, tile);

			return true;
		}

		public static Tile IDToTile(int id) => tiles[id];
		public static int TileToID(Tile tile) => tiles.IndexOf(tile);

		public Vector2Int CamToTile(Vector2 mousePos)
		{
			var tilePos = Vector2Int.zero;

			mousePos -= position;

			tilePos.x = (int)mousePos.x;
			tilePos.y = (int)mousePos.y;

			tilePos.Clamp(0, 0, size.x - 1, size.y - 1);

			return tilePos;
		}

		public void Update()
		{
			highTick = !highTick;

			for (int x = size.x - 1; x >= 0; x--)
				for (int y = size.y - 1; y >= 0; y--)
					world[x, y].Update(new Vector2Int(x, y));
		}

		public void Draw()
		{
			using (new DrawBatch(SamplerState.PointClamp, null, SpriteSortMode.Deferred, BlendState.AlphaBlend))
			{
				for (int y = 0; y < size.y; y++)
				{
					for (int x = 0; x < size.x; x++)
					{
						var tile = GetTile(x, y);
						if (tile.color != Color.clear)
							GFX.DrawPoint(position + new Vector2(x, y), tile.color);
					}
				}
			}
		}
	}
}