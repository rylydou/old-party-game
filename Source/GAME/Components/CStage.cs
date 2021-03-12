using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.Physics;

namespace GAME.Components
{
	public class CStage : Component
	{
		public const string stage =
		"##################################" +
		"#                                #" +
		"#                                #" +
		"#    #                          ##" +
		"#    #                        ####" +
		"#   ##          ####     #########" +
		"#    ######              #       #" +
		"#    #                 ###       #" +
		"#    #         ####              #" +
		"#            ###       ####      #" +
		"#     ###                        #" +
		"#            ####                #" +
		"#                  ###           #" +
		"#     #####               #####  #" +
		"# #                     ##       #" +
		"# ####   ###############       ###" +
		"#                           # ## #" +
		"#                 #    ##  #     #" +
		"#  #       #### ########       ###" +
		"# ##   ###                  #    #" +
		"######                    # # ####" +
		"##################################";

		public static readonly Vector2Int size = new Vector2Int(34, 22);

		public static CStage current { get; private set; }

		public float tileSize = 8.0f;

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
				tiles.Draw(entity.position, tileSize, size, (x, y) => GetTile(x, y));

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
			}
		}

		public RaycastHit Raycast(Vector2 origin, Vector2 direction, bool worldSpce = true, int maxIterations = -1)
		{
			if (maxIterations < 0) maxIterations = size.max;

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
			if (x < 0 || x >= size.x || y < 0 || y >= size.y) return true;

			return stage[y * size.x + x] == '#';
		}
	}
}