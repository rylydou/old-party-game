using System.Collections.Generic;
using System.IO;
using System.Linq;
using MGE;
using MGE.ECS;
using MGE.FileIO;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.Physics;

namespace GAME.Components
{
	public class CWorld : Component, ICanRaycast
	{
		public const string chunkPath = "/Chunks";
		public const int chunkSize = 16;
		public const int loadDistance = 3;
		public const int unloadDistance = 5;
		public const int tileSize = 8;

		public Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();
		public List<Tileset> tilesets = new List<Tileset>();

		Entity player;

		bool firstFrame = true;
		int updateChunkIndex = 0;
		Folder worldFolder;

		Font font;

		public override void Init()
		{
			worldFolder = new Folder(App.exePath + "/Data/World");

			foreach (var chunk in worldFolder.GetFilesInDir("/Chunks/"))
				File.Delete(chunk);

			tilesets.Add(Assets.GetAsset<Tileset>("Sprites/Grass"));

			font = Assets.GetAsset<Font>("Fonts/Basic");
		}

		public override void Update()
		{
			if (firstFrame)
			{
				if (player == null) player = entity.layer.FindEntityByComponent<CPlayer>();
				LoadChunk(Vector2Int.zero);
			}
			firstFrame = false;

			var playerPos = (Vector2Int)(player.position / chunkSize / 8);

			for (int y = -loadDistance; y <= loadDistance; y++)
			{
				for (int x = -loadDistance; x <= loadDistance; x++)
				{
					var pos = playerPos + new Vector2Int(x, y);
					if (!chunks.ContainsKey(pos))
						LoadChunk(pos);
				}
			}

			if (Input.GetButton(Inputs.MouseLeft))
				SetTile(Input.cameraMousePosition / tileSize, 1);
			else if (Input.GetButton(Inputs.MouseRight))
				SetTile(Input.cameraMousePosition / tileSize, 0);

			if (updateChunkIndex >= chunks.Count)
				updateChunkIndex = 0;

			var chunkToUpdate = chunks.ElementAt(updateChunkIndex);

			if ((playerPos - chunkToUpdate.Key).sqrMagnitude > unloadDistance * unloadDistance)
			{
				UnloadChunk(chunkToUpdate.Key);
			}
			else
			{
				chunkToUpdate.Value.Save(worldFolder);
			}

			updateChunkIndex++;
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				foreach (var chunk in chunks)
				{
					tilesets[0].Draw(
						(Vector2)chunk.Value.position * chunkSize * tileSize,
						tileSize, new Vector2Int(chunkSize),
						(x, y) =>
						{
							if (chunk.Value.tiles.IsInBounds(x, y))
								return chunk.Value.tiles[x, y] != 0;
							return true;
						},
						Color.white
					);
				}
			}

			using (new DrawBatch())
			{
				foreach (var chunk in chunks)
				{
					GFX.DrawRectangle(new Rect((Vector2)chunk.Key * chunkSize * tileSize, new Vector2(chunkSize * tileSize)), new Color(0, 0, 1, 0.5f), 0.5f);
					font.DrawText(chunk.Key.ToString(), (Vector2)chunk.Key * chunkSize * tileSize + tileSize / 2, new Color(0, 0, 1, 0.5f), 0.25f);
				}
			}
		}

		public void LoadChunk(Vector2Int position)
		{
			var path = $"/Chunks/{position.x} {position.y}.chunk";

			Chunk newChunk = null;

			if (worldFolder.FileExists(path))
				newChunk = Chunk.Load(worldFolder, position.x, position.y);
			else
			{
				var data = new Grid<ushort>(chunkSize, chunkSize, 0);

				for (int y = 0; y < chunkSize; y++)
				{
					for (int x = 0; x < chunkSize; x++)
					{
						var absPos = position * chunkSize + new Vector2Int(x, y);
						if (absPos.y > chunkSize * 1)
						{
							if (absPos.y > chunkSize * 4)
							{
								data[x, y] = System.Convert.ToUInt16(Random.Bool(95));
							}
							else if (absPos.y > chunkSize * 3)
							{
								data[x, y] = System.Convert.ToUInt16(Random.Bool(75));
							}
							else if (absPos.y > chunkSize * 2)
							{
								data[x, y] = System.Convert.ToUInt16(Random.Bool(90));
							}
							else
							{
								data[x, y] = System.Convert.ToUInt16(Random.Bool(99));
							}
						}
						else if (absPos.y == chunkSize + 1)
						{
							data[x, y] = System.Convert.ToUInt16(Math.Sin(absPos.x * Math.Abs(position.x)) < 0.5f);
						}
					}
				}

				newChunk = new Chunk(position, data);
			}

			chunks.Add(position, newChunk);
		}

		public void UnloadChunk(Vector2Int position)
		{
			chunks[position].Save(worldFolder);
			chunks.Remove(position);
		}

		public RaycastHit Raycast(Vector2 origin, Vector2 direction, int maxIterations = -1)
		{
			var chunkPos = WorldToChunk(origin);

			Chunk chunk = null;

			chunks.TryGetValue(chunkPos, out chunk);

			RaycastHit hit = null;

			if (chunk != null)
				hit = Physics.RayVsGrid((origin - (Vector2)chunkPos * chunkSize * tileSize) / tileSize, direction, (x, y) => chunk.tiles.Get(x, y) != 0, maxIterations);

			if (hit is object)
			{
				hit.origin = origin;
				hit.distance = hit.distance * tileSize;
			}

			return hit;
		}

		public Vector2Int WorldToChunk(Vector2 position)
		{
			var newPos = (position / chunkSize / tileSize);

			if (position.x > 0)
				newPos.x = Math.Floor(newPos.x);
			else
				newPos.x = Math.Ceil(newPos.x - 1);

			if (position.y > 0)
				newPos.y = Math.Floor(newPos.y);
			else
				newPos.y = Math.Ceil(newPos.y - 1);

			return newPos;
		}

		public bool SetTile(Vector2Int position, ushort tile)
		{
			Vector2 tmpChunkPos = (Vector2)position / chunkSize;

			var chunkPos = Vector2Int.zero;

			if (position.x > 0)
				chunkPos.x = (int)Math.Floor(tmpChunkPos.x);
			else
				chunkPos.x = (int)Math.Ceil(tmpChunkPos.x - 1);

			if (position.y > 0)
				chunkPos.y = (int)Math.Floor(tmpChunkPos.y);
			else
				chunkPos.y = (int)Math.Ceil(tmpChunkPos.y - 1);

			Vector2Int chunkTilePos = position - chunkPos * chunkSize;

			Logger.Log($"{chunkPos} {chunkTilePos}");

			Chunk chunk = null;

			if (chunks.TryGetValue(chunkPos, out chunk))
			{
				chunk.tiles[chunkTilePos] = tile;
				return true;
			}

			return false;
		}
	}
}