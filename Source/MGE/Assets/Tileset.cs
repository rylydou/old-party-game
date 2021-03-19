using System;
using System.Collections.Generic;
using MGE.Graphics;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public class Tileset
	{
		public Texture texture;

		[JsonProperty] public Vector2Int tileSize;

		[JsonProperty] public Vector2Int defualtTile;

		[JsonProperty] public Dictionary<TileConnection, Vector2Int> tiles;

		public Tileset() { }

		public void Draw(Vector2 position, float scale, Vector2Int mapSize, Func<int, int, bool> isSolid, Color color)
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
							Logger.LogWarning($"No tile that follows rule: {((TileConnection)connection)} {tile}");

							DrawTile(
								new RectInt(defualtTile.x, defualtTile.y, tileSize.x, tileSize.y),
								new Rect(position.x + x * scale, position.y + y * scale, scale, scale),
								color
							);
							GFX.DrawBox(new Rect(position.x + x * scale, position.y + y * scale, scale, scale), Color.red.ChangeAlpha(0.25f));
						}
						else
						{
							DrawTile(
								new RectInt(tile.x, tile.y, tileSize.x, tileSize.y),
								new Rect(position.x + x * scale, position.y + y * scale, scale, scale),
								color
							);
						}
					}
				}
			}
		}

		public void DrawTiles(in Grid<RectInt?> map, Vector2 position, float scale, Color color)
		{
			for (int y = 0; y < map.size.y; y++)
			{
				for (int x = 0; x < map.size.x; x++)
				{
					var tile = map[x, y];

					if (tile.HasValue)
						DrawTile(
							new RectInt(tile.Value.x, tile.Value.y, tileSize.x, tileSize.y),
							new Rect(position.x + x * scale, position.y + y * scale, scale, scale),
							color
						);
				}
			}
		}

		public RectInt?[] GetTiles(Vector2Int mapSize, Func<int, int, bool> isSolid)
		{
			var tileRects = new List<RectInt?>();

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
							Logger.LogWarning($"No tile that follows rule: {((TileConnection)connection)} {tile}");

							tileRects.Add(new RectInt(defualtTile.x, defualtTile.y, tileSize.x, tileSize.y));
						}
						else
						{
							tileRects.Add(new RectInt(tile.x, tile.y, tileSize.x, tileSize.y));
						}
					}
				}
			}

			return tileRects.ToArray();
		}

		public void GetTiles(ref Grid<RectInt?> map, Func<int, int, bool> isSolid)
		{
			for (int y = 0; y < map.size.y; y++)
			{
				for (int x = 0; x < map.size.x; x++)
				{
					if (isSolid.Invoke(x, y))
					{
						var connection = GetConnections(x, y, ref isSolid);
						var tile = defualtTile;

						if (!tiles.TryGetValue(connection, out tile))
						{
							Logger.LogWarning($"No tile that follows rule: {((TileConnection)connection)} {tile}");

							map[x, y] = new RectInt(defualtTile.x, defualtTile.y, tileSize.x, tileSize.y);
						}
						else
						{
							map[x, y] = new RectInt(tile.x, tile.y, tileSize.x, tileSize.y);
						}
					}
				}
			}
		}

		public void DrawTile(RectInt source, Rect destination, Color color)
		{
			GFX.Draw(texture, source, destination, color);
		}

		public TileConnection GetConnections(int x, int y, ref Func<int, int, bool> isSolid)
		{
			var connection = TileConnection.None;

			for (short checkY = -1; checkY <= 1; checkY++)
			{
				for (short checkX = -1; checkX <= 1; checkX++)
				{
					if (Math.Abs(checkX) + Math.Abs(checkY) > 1 || (checkX == 0 && checkY == 0)) continue;

					if (isSolid.Invoke(x + checkX, y + checkY))
					{
						// TODO: Make not jank
						switch (checkX << 1 | checkY >> 1)
						{
							case 0 << 1 | -1 >> 1:
								connection |= TileConnection.Top;
								break;
							case 1 << 1 | 0 >> 1:
								connection |= TileConnection.Right;
								break;
							case 0 << 1 | 1 >> 1:
								connection |= TileConnection.Bottom;
								break;
							case -1 << 1 | 0 >> 1:
								connection |= TileConnection.Left;
								break;
						}
					}
				}
			}

			return connection;
		}
	}
}