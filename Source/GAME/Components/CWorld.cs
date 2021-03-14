using System.Collections.Generic;
using System.Linq;
using MGE;
using MGE.ECS;
using MGE.FileIO;
using MGE.Physics;

namespace GAME.Components
{
	public class CWorld : Component, ICanRaycast
	{
		public const string chunkPath = "/Chunks";
		public const int chunkSize = 16;
		public const int loadDistance = 2;
		public const int unloadDistance = 4;

		public List<CChunk> chunks = new List<CChunk>();

		public Folder worldFolder;

		Entity player;

		bool firstFrame = true;

		int currentPos = 0;

		public override void Init()
		{
			worldFolder = new Folder(App.exePath + "/Data/World");
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
					if (!chunks.Any((x) => x.data.position == pos))
						LoadChunk(pos);
				}
			}

			for (int i = 0; i < chunks.Count; i++)
			{
				if ((playerPos - chunks[i].data.position).sqrMagnitude > unloadDistance * unloadDistance)
				{
					entity.layer.RemoveEntity(chunks[i].entity);
					chunks.RemoveAt(i);
				}
			}

			if (currentPos >= chunks.Count)
				currentPos = 0;

			var chunk = chunks[currentPos];

			chunk.Save(worldFolder.GetFullPath($"/Chunks/{chunk.data.position.x} {chunk.data.position.y}.chunk"));

			currentPos++;
		}

		public void LoadChunk(Vector2Int position)
		{
			var path = $"/Chunks/{position.x} {position.y}.chunk";

			CChunk newChunk = null;

			if (worldFolder.FileExists(path))
				newChunk = new CChunk(IO.Load<ChunkData>(worldFolder.GetFullPath(path)));
			else
			{
				var data = new bool[chunkSize, chunkSize];

				for (int y = 0; y < chunkSize; y++)
				{
					for (int x = 0; x < chunkSize; x++)
					{
						var absPos = position * chunkSize + new Vector2Int(x, y);
						if (absPos.y > chunkSize * 1)
						{
							if (absPos.y > chunkSize * 6)
							{
								data[x, y] = Random.Bool(95);
							}
							if (absPos.y > chunkSize * 5)
							{
								data[x, y] = Random.Bool(10);
							}
							if (absPos.y > chunkSize * 4)
							{
								data[x, y] = Random.Bool(67);
							}
							else if (absPos.y > chunkSize * 3)
							{
								data[x, y] = Random.Bool(75);
							}
							else if (absPos.y > chunkSize * 2)
							{
								data[x, y] = Random.Bool(90);
							}
							else
							{
								data[x, y] = Random.Bool(99);
							}
						}
						else if (absPos.y == chunkSize)
						{
							data[x, y] = Math.Sin(absPos.x * Math.Abs(position.x)) < 0.5f;
						}
					}
				}

				newChunk = new CChunk(new ChunkData(position, data));
			}

			chunks.Add(newChunk);

			entity.layer.AddEntity(new Entity(newChunk) { position = position * chunkSize * 8 });
		}

		public RaycastHit Raycast(Vector2 origin, Vector2 direction, int maxIterations = -1)
		{
			var originPosition = (Vector2Int)(origin / chunkSize / 8);

			var chunk = chunks.Find((x) => x.data.position == originPosition);

			if (chunk != null)
				return chunk.Raycast(origin, direction, maxIterations);

			Logger.LogWarning($"Could not find chunk {originPosition}");

			return null;
		}
	}
}