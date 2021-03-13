using MGE;
using MGE.ECS;
using MGE.FileIO;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.Physics;

namespace GAME.Components
{
	public class CStage : Component
	{
		public static CStage current { get; private set; }

		public ChunkData data;

		public float tileSize = 8.0f;

		public Vector2 shadowOffset = new Vector2(1.0f);

		public Vector2Int pos;

		public Vector2 startPos = new Vector2(3, 4);
		public Vector2 endPos = new Vector2(16, 12);

		public RaycastHit hit = null;

		Tileset tiles;

		public override void Init()
		{
			if (current != null)
			{
				entity.RemoveComponent(this.GetType());
				return;
			}
			current = this;

			tiles = Assets.GetAsset<Tileset>("Sprites/Tiles");

			Load();

			entity.position = new Vector2(tileSize);
		}

		public override void Update()
		{
			pos = (Input.cameraMousePosition - entity.position) / tileSize;
			pos.Clamp(0, 0, data.tiles.GetLength(0) - 1, data.tiles.GetLength(1) - 1);

			if (Input.GetButton(Inputs.LeftAlt))
			{
				if (Input.GetButton(Inputs.MouseLeft))
					startPos = Input.cameraMousePosition;
				else if (Input.GetButton(Inputs.MouseRight))
					endPos = Input.cameraMousePosition;
			}
			else if (Input.GetButton(Inputs.MouseLeft))
				data.tiles[pos.x, pos.y] = true;
			else if (Input.GetButton(Inputs.MouseRight))
				data.tiles[pos.x, pos.y] = false;

			hit = Raycast(startPos, (endPos - startPos));
		}

		public void Save()
		{
			IO.Save(App.exePath + "/Data/Chunks/test.chunk", data);
		}

		public void Load()
		{
			data = IO.Load<ChunkData>(App.exePath + "/Data/Chunks/test.chunk");
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				tiles.Draw(entity.position + shadowOffset, tileSize, data.size, (x, y) => GetTile(x, y), new Color(0, 0.25f));
				tiles.Draw(entity.position, tileSize, data.size, (x, y) => GetTile(x, y), Color.white);

				GFX.DrawCircle(startPos, 0.5f * tileSize, Color.red, 1f);

				GFX.DrawCircle(endPos, 0.5f * tileSize, Color.blue, 1f);

				GFX.DrawLine(startPos, endPos, Color.gray, 0.25f);

				if (hit == null)
				{
					GFX.DrawCircle(endPos, 0.25f * tileSize, Color.green, 0.5f);

					GFX.DrawLine(startPos, endPos, Color.green, 0.25f);
				}
				else
				{
					GFX.DrawCircle(hit.position, 0.25f * tileSize, Color.red, 0.5f);

					GFX.DrawLine(startPos, hit.position, Color.red, 0.25f);

					GFX.DrawLine(hit.position, (hit.position + hit.normal * tileSize / 2), Color.red, 0.25f);
				}

				GFX.DrawRectangle(new Rect(pos.x * tileSize + entity.position.x, pos.y * tileSize + entity.position.y, tileSize, tileSize), Colors.accent2, 1.0f);
			}
		}

		public RaycastHit Raycast(Vector2 origin, Vector2 direction, bool worldSpce = true, int maxIterations = -1)
		{
			if (maxIterations < 0) maxIterations = data.size.max;

			if (worldSpce)
			{
				origin = (origin - entity.position) / tileSize;
			}

			var hit = Physics.RayVsGrid(origin, direction, (x, y) => GetTile(x, y), maxIterations);

			if (hit is object)
			{
				if (worldSpce)
				{
					hit.origin = origin * tileSize + entity.position;
					hit.distance = hit.distance * tileSize;
				}
			}

			return hit;
		}

		public bool GetTile(int x, int y)
		{
			if (x < 0 || x >= data.tiles.GetLength(0) || y < 0 || y >= data.tiles.GetLength(1)) return true;

			return data.tiles[x, y];
		}
	}
}