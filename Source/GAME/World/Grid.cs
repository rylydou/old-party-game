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
		};

		public int[,] world;
		public Vector2 position;

		public readonly Vector2Int size;

		public Grid(Vector2Int size, IGenerator generator)
		{
			this.size = size;
			this.position = Window.gameSize / 2 - size / 2;

			world = new int[size.x, size.y];

			new GenTest().Generate(ref world);
		}

		public int GetTileID(Vector2Int position) => GetTileID(position.x, position.y);
		public int GetTileID(int x, int y) => world[x, y];

		public ITile GetTileLogic(Vector2Int position) => tiles[GetTileID(position)];
		public ITile GetTileLogic(int x, int y) => tiles[GetTileID(x, y)];

		public void SetTileID(Vector2Int position, int id) => SetTileID(position.x, position.y, id);
		public void SetTileID(int x, int y, int id) => world[x, y] = id;

		public void SetTileLogic(Vector2Int position, Type tile) => SetTileLogic(position.x, position.y, tile);
		public void SetTileLogic(int x, int y, Type tile) => SetTileID(x, y, TileToID(tile));

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

		public void Draw()
		{
			using (new DrawBatch(SamplerState.PointClamp, null, SpriteSortMode.Deferred, BlendState.Opaque))
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

			var pos = CamToTile(Input.cameraMousePosition);

			GUI.AddElement(new GUIText($"{pos} {GetTileLogic(pos).name}") { rect = new Rect(Input.windowMousePosition + new Vector2(16, -16), Vector2.zero) });
		}
	}
}