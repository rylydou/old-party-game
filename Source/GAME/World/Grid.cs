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
		public static readonly List<ITile> tiles = new List<ITile>()
		{
			new Air(),
			new Dirt(),
			new Stone(),
			new Sand(),
			new Water(),
			new TNT(),
		};

		public int[,] oldWorld;
		public int[,] newWorld;
		public Vector2 position;

		public readonly Vector2Int size;

		public Grid(Vector2Int size, IGenerator generator)
		{
			this.size = size;
			this.position = Window.gameSize / 2 - size / 2;

			this.newWorld = new int[size.x, size.y];
			new GenTest().Generate(ref newWorld);

			this.oldWorld = newWorld.Clone() as int[,];
		}

		public int GetTileID(Vector2Int position) => GetTileID(position.x, position.y);
		public int GetTileID(int x, int y)
		{
			if (x < 0 || x >= size.x || y < 0 || y >= size.y) return 1;
			return oldWorld[x, y];
		}

		public ITile GetTileLogic(Vector2Int position) => tiles[GetTileID(position)];
		public ITile GetTileLogic(int x, int y) => tiles[GetTileID(x, y)];

		public bool SetTileID(Vector2Int position, int id) => SetTileID(position.x, position.y, id);
		public bool SetTileID(int x, int y, int id)
		{
			if (x < 0 || x >= size.x || y < 0 || y >= size.y) return false;
			newWorld[x, y] = id;
			return true;
		}

		public bool SetTileLogic(Vector2Int position, Type tile) => SetTileLogic(position.x, position.y, tile);
		public bool SetTileLogic(int x, int y, Type tile) => SetTileID(x, y, TileToID(tile));

		public bool MoveTile(Vector2Int from, Vector2Int to)
		{
			if (GetTileID(to) != 0) return false;

			var tile = GetTileID(from);
			SetTileID(from, 0);
			SetTileID(to, tile);

			return true;
		}

		public static ITile IDToTile(int id) => tiles[id];
		public static int TileToID(Type tile) => tiles.IndexOf(tile as ITile);

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
			for (int y = 0; y < size.y; y++)
				for (int x = 0; x < size.x; x++)
					oldWorld[x, y] = newWorld[x, y];

			for (int x = 0; x < size.x; x++)
				for (int y = 0; y < size.y; y++)
					if (GetTileID(x, y) != 0)
						GetTileLogic(x, y).Update(new Vector2Int(x, y));
		}

		public void Draw()
		{
			using (new DrawBatch(SamplerState.PointClamp, null, SpriteSortMode.Deferred, BlendState.NonPremultiplied))
			{
				for (int y = 0; y < size.y; y++)
				{
					for (int x = 0; x < size.x; x++)
					{
						var tile = GetTileID(x, y);
						if (tile != 0) GFX.DrawPoint(position + new Vector2(x, y), IDToTile(tile).color);
					}
				}
			}
		}
	}
}