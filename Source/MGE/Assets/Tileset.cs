using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MGE.FileIO;
using MGE.Graphics;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public class Tileset : Asset
	{
		public override string extension => ".tileset.psd";

		public Texture texture;

		[JsonProperty] public string name;
		[JsonProperty] public Color color;
		[JsonProperty] public bool semiSolid = false;
		[JsonProperty] public Vector2Int tileSize;
		[JsonProperty] public Vector2Int defualtTile;
		[JsonProperty] public Dictionary<TileConnection, Vector2Int> tiles;

		public Tileset() { }

		public override void Load(string fullPath, string localPath = null)
		{
			base.Load(fullPath, localPath);

			var ts = IO.LoadJson<Tileset>(fullPath + ".info");

			name = ts.name;
			color = ts.color;
			semiSolid = ts.semiSolid;
			tileSize = ts.tileSize;
			defualtTile = ts.defualtTile;
			tiles = ts.tiles;

			texture = new Texture();
			texture.Load(fullPath, localPath);
		}

		public void DrawTiles(in Grid<RectInt> map, Vector2 position, Color color)
		{
			for (int y = 0; y < map.size.y; y++)
			{
				for (int x = 0; x < map.size.x; x++)
				{
					var tile = map[x, y];

					if (tile == RectInt.zero) continue;

					DrawTile(
						new RectInt(tile.x, tile.y, tileSize.x, tileSize.y),
						new Vector2(position.x + x, position.y + y),
						color
					);
				}
			}
		}

		public RectInt[] GetTiles(Vector2Int mapSize, Func<int, int, bool> isSolid)
		{
			var tileRects = new List<RectInt>();

			for (int y = 0; y < mapSize.y; y++)
			{
				for (int x = 0; x < mapSize.x; x++)
				{
					if (isSolid.Invoke(x, y))
					{
						var connection = GetConnections(x, y, isSolid);
						var tile = defualtTile;

						if (!tiles.TryGetValue(connection, out tile))
						{
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

		public void GetTiles(ref Grid<RectInt> map, Func<int, int, bool> isTile, Func<int, int, bool> isSolid)
		{
			for (int y = 0; y < map.size.y; y++)
			{
				for (int x = 0; x < map.size.x; x++)
				{
					if (isTile.Invoke(x, y))
					{
						var connection = GetConnections(x, y, isSolid);
						var tile = defualtTile;

						if (!tiles.TryGetValue(connection, out tile))
						{
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

		public void DrawTile(RectInt source, Vector2 position, Color color)
		{
			GFX.Draw(texture, source, position, color);
		}

		public TileConnection GetConnections(int x, int y, Func<int, int, bool> isSolid)
		{
			var connection = TileConnection.None;

			for (sbyte checkY = -1; checkY <= 1; checkY++)
			{
				for (sbyte checkX = -1; checkX <= 1; checkX++)
				{
					if (checkX == 0 && checkY == 0) continue;

					if (isSolid.Invoke(x + checkX, y + checkY))
					{
						switch ($"{checkX},{checkY}")
						{
							case "0,-1":
								connection |= TileConnection.Top;
								break;
							case "1,0":
								connection |= TileConnection.Right;
								break;
							case "0,1":
								connection |= TileConnection.Bottom;
								break;
							case "-1,0":
								connection |= TileConnection.Left;
								break;
								// TODO: Make work
								// case "-1,-1":
								// 	connection |= TileConnection.Top_Left;
								// 	break;
								// case "1,-1":
								// 	connection |= TileConnection.Top_Right;
								// 	break;
								// case "-1,1":
								// 	connection |= TileConnection.Bottom_Left;
								// 	break;
								// case "1,1":
								// 	connection |= TileConnection.Bottom_Right;
								// 	break;
						}
					}
				}
			}

			return connection;
		}
	}
}