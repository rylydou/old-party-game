using System;
using System.Collections.Generic;
using MGE.Graphics;
using Newtonsoft.Json;

namespace MGE
{
	public class TileSheet
	{
		[JsonIgnore] public Texture texture;

		public RectInt defualtTile;

		public Dictionary<TileConnection, RectInt> tiles;

		public TileSheet() { }

		public TileSheet(Texture texture, RectInt defualtTile, Dictionary<TileConnection, RectInt> tiles)
		{
			this.texture = texture;
			this.defualtTile = defualtTile;
			this.tiles = tiles;
		}

		public void Draw(Vector2 position, double scale, Vector2Int mapSize, Func<int, int, bool> isSolid)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				for (int x = 0; x < mapSize.x; x++)
				{
					if (isSolid.Invoke(x, y))
					{
						var connection = GetConnections(x, y, ref isSolid);
						var tile = defualtTile;

						if (!tiles.TryGetValue(connection, out tile))
						{
							Logger.Log($"Used Defualt: {((TileConnection)connection)} {tile.position}");
							GFX.DrawBox(new Rect(position.x + x * scale, position.y + y * scale, scale, scale), Color.red);
						}
						else
						{
							GFX.Draw(texture, tile, new Rect(position.x + x * scale, position.y + y * scale, scale, scale), Color.white);
						}
					}
				}
			}

			for (short y = -1; y <= 1; y++)
			{
				for (short x = -1; x <= 1; x++)
				{
					if (Math.Abs(x) + Math.Abs(y) > 1 || (x == 0 && y == 0)) continue;

					if (isSolid.Invoke(x, y))
						GFX.DrawBox(new Rect(position.x + x * scale, position.y + y * scale, scale, scale), Color.green);
					else
						GFX.DrawBox(new Rect(position.x + x * scale, position.y + y * scale, scale, scale), Color.blue);
				}
			}
		}

		TileConnection GetConnections(int x, int y, ref Func<int, int, bool> isSolid)
		{
			var connection = TileConnection.None;

			for (short checkY = -1; checkY <= 1; checkY++)
			{
				for (short checkX = -1; checkX <= 1; checkX++)
				{
					if (Math.Abs(checkX) + Math.Abs(checkY) > 1 || (checkX == 0 && checkY == 0)) continue;

					if (isSolid.Invoke(x + checkX, y + checkY))
						connection |= (TileConnection)((checkX << 2) ^ (checkY >> 2));
				}
			}

			return connection;
		}
	}
}