using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.Physics;
using Microsoft.Xna.Framework.Graphics;

namespace GAME.Components
{
	public class CStage : Component
	{
		public const string stage =
		"########################" +
		"#                      #" +
		"#                      #" +
		"#               #####  #" +
		"# #           #####    #" +
		"# ####   ######      ###" +
		"#                 # ## #" +
		"#            ##  #     #" +
		"#  #       ##        ###" +
		"# ##   ###        #    #" +
		"######          # # ####" +
		"########################"
		;

		public static readonly Vector2Int size = new Vector2Int(24, 12);

		public static CStage current { get; private set; }

		public float tileSize = 8.0f;

		public Vector2 startPos = new Vector2(1, 1);
		public Vector2 endPos = new Vector2(8, 8);

		public RaycastHit hit = null;

		public override void Init()
		{
			if (current != null)
			{
				entity.RemoveComponent(this.GetType());
				return;
			}
			current = this;

			entity.position = new Vector2(tileSize);
		}

		public override void Update()
		{
			if (Input.GetButton(Inputs.MouseLeft))
				startPos = (Input.cameraMousePosition - entity.position) / tileSize;
			if (Input.GetButton(Inputs.MouseRight))
				endPos = (Input.cameraMousePosition - entity.position) / tileSize;

			hit = Raycast(startPos, (endPos - startPos));
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				for (int y = 0; y < size.y; y++)
					for (int x = 0; x < size.x; x++)
						if (GetTile(x, y))
							GFX.DrawBox(new Rect(x * tileSize + entity.position.x, y * tileSize + entity.position.y, tileSize, tileSize), Color.black);

				GFX.DrawCircle(startPos * tileSize + entity.position, 0.5f * tileSize, Color.red, 1f);

				GFX.DrawCircle(endPos * tileSize + entity.position, 0.5f * tileSize, Color.blue, 1f);

				GFX.DrawLine(startPos * tileSize + entity.position, endPos * tileSize + entity.position, Color.gray, 0.25f);

				if (hit == null)
				{
					GFX.DrawCircle(endPos * tileSize + entity.position, 0.25f * tileSize, Color.green, 0.5f);

					GFX.DrawLine(startPos * tileSize + entity.position, endPos * tileSize + entity.position, Color.green, 0.25f);
				}
				else
				{
					GFX.DrawCircle(hit.position * tileSize + entity.position, 0.25f * tileSize, Color.green, 0.5f);

					GFX.DrawLine(startPos * tileSize + entity.position, hit.position * tileSize + entity.position, Color.green, 0.25f);

					GFX.DrawLine(hit.position * tileSize + entity.position, (hit.position + hit.normal / 2) * tileSize + entity.position, Color.red, 0.25f);
				}
			}
		}

		public RaycastHit Raycast(Vector2 origin, Vector2 direction, int maxIterations = -1)
		{
			if (maxIterations < 0) maxIterations = size.max;

			return Physics.RayVsGrid(startPos, direction, (x, y) => GetTile(x, y), maxIterations);
		}

		public bool GetTile(int x, int y)
		{
			if (x < 0 || x >= size.x || y < 0 || y >= size.y) return true;

			return stage[y * size.x + x] == '#';
		}
	}
}

/*
using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.Physics;
using Microsoft.Xna.Framework.Graphics;

namespace GAME.Components
{
	public class CStage : Component
	{
		public const string stage =
		"########################" +
		"#                      #" +
		"#                      #" +
		"#               #####  #" +
		"# #           #####    #" +
		"# ####   ######      ###" +
		"#                 # ## #" +
		"#            ##  #     #" +
		"#  #       ##        ###" +
		"# ##   ###        #    #" +
		"######          # # ####" +
		"########################"
		;

		public static readonly Vector2Int size = new Vector2Int(24, 12);

		public static CStage current { get; private set; }

		public float tileSize = 8.0f;

		public Vector2 startPos = new Vector2(1, 1);
		public Vector2 endPos = new Vector2(8, 8);

		public RaycastHit hit = null;

		public override void Init()
		{
			if (current != null)
			{
				entity.RemoveComponent(this.GetType());
				return;
			}
			current = this;

			entity.position = new Vector2(tileSize);
		}

		public override void Update()
		{
			if (Input.GetButton(Inputs.MouseLeft))
				startPos = Input.cameraMousePosition;
			if (Input.GetButton(Inputs.MouseRight))
				endPos = Input.cameraMousePosition;

			hit = Raycast(startPos, (endPos - startPos));
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				for (int y = 0; y < size.y; y++)
					for (int x = 0; x < size.x; x++)
						if (GetTile(x, y))
							GFX.DrawBox(new Rect(x * tileSize + entity.position.x, y * tileSize + entity.position.y, tileSize, tileSize), Color.black);

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
					GFX.DrawCircle(hit.position, 0.25f * tileSize, Color.green, 0.5f);

					GFX.DrawLine(startPos, hit.position, Color.green, 0.25f);

					GFX.DrawLine(hit.position, (hit.position + hit.normal / 2), Color.red, 0.25f);
				}
			}
		}

		public RaycastHit Raycast(Vector2 origin, Vector2 direction, bool worldSpace = true, int maxIterations = -1)
		{
			if (maxIterations < 0) maxIterations = size.max;

			if (worldSpace)
			{
				origin = (origin / tileSize) - entity.position;
			}

			var hit = Physics.RayVsGrid(startPos, direction, (x, y) => GetTile(x, y), maxIterations);

			if (worldSpace)
			{
				hit.origin = hit.origin * tileSize + entity.position;
				hit.distance = hit.distance * tileSize;
			}

			return hit;
		}

		public bool GetTile(int x, int y)
		{
			if (x < 0 || x >= size.x || y < 0 || y >= size.y) return true;

			return stage[y * size.x + x] == '#';
		}
	}
}
 */