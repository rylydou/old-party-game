using MGE;
using MGE.ECS;
using MGE.FileIO;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.Physics;

namespace GAME.Components
{
	public class CChunk : Component, ICanRaycast
	{
		public ChunkData data;

		public float tileSize = 8.0f;

		public Vector2 shadowOffset = new Vector2(1.0f);

		public RaycastHit hit = null;

		Tileset tiles;

		public CChunk(ChunkData data)
		{
			this.data = data;
		}

		public override void Init()
		{
			tiles = Assets.GetAsset<Tileset>("Sprites/Tiles");
		}

		public override void Update()
		{
			if (new Rect(entity.position, (Vector2)data.size * tileSize).Contains(Input.cameraMousePosition))
			{
				Vector2Int pos = (Input.cameraMousePosition - entity.position) / tileSize;
				pos.Clamp(0, 0, data.tiles.GetLength(0) - 1, data.tiles.GetLength(1) - 1);

				if (Input.GetButton(Inputs.MouseLeft))
					data.tiles[pos.x, pos.y] = true;
				else if (Input.GetButton(Inputs.MouseRight))
					data.tiles[pos.x, pos.y] = false;
			}
		}

		public void Save(string path)
		{
			IO.Save(path, data);
		}

		public void Load(string path)
		{
			data = IO.Load<ChunkData>(path);
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				tiles.Draw(entity.position + shadowOffset, tileSize, data.size, (x, y) => GetTile(x, y), new Color(0, 0.25f));
				tiles.Draw(entity.position, tileSize, data.size, (x, y) => GetTile(x, y), Color.white);
			}
		}

		public RaycastHit Raycast(Vector2 origin, Vector2 direction, int maxIterations = -1)
		{
			if (maxIterations < 0) maxIterations = data.size.max;

			origin = (origin - entity.position) / tileSize;

			var hit = Physics.RayVsGrid(origin, direction, (x, y) => GetTile(x, y), maxIterations);

			if (hit is object)
			{
				hit.origin = origin * tileSize + entity.position;
				hit.distance = hit.distance * tileSize;
			}

			return hit;
		}

		public bool GetTile(int x, int y)
		{
			if (x < 0 || x >= data.tiles.GetLength(0) || y < 0 || y >= data.tiles.GetLength(1)) return false;

			return data.tiles[x, y];
		}
	}
}