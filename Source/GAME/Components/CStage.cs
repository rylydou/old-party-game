using MGE;
using MGE.ECS;
using MGE.Physics;

namespace GAME.Components
{
	public class CStage : Component, ICanRaycast
	{
		public static CStage current { get; private set; }

		public Vector2Int mapSize = Vector2Int.zero;

		Grid<byte> map;
		Grid<RectInt> tiles;

		Tileset tileset;

		public override void Init()
		{
			current = this;

			tileset = Assets.GetAsset<Tileset>("Tilesets/Grass");

			mapSize = (Vector2)Window.gameRenderSize / Config.pixelsPerUnit;

			map = new Grid<byte>(mapSize, 1);
			tiles = new Grid<RectInt>(mapSize, RectInt.zero);

			var noise = new Noise();
			noise.noiseType = Noise.NoiseType.OpenSimplex2S;
			noise.fractalType = Noise.FractalType.FBm;
			noise.octaves = 8;
			noise.gain = 0.5f;
			noise.frequency = 0.001f;

			map.For((x, y) =>
			{
				return (byte)(x == 0 || y == 0 || x == mapSize.x - 1 || y == mapSize.y - 1 ? 1 : (noise.GetNoise(x * 8, 0).Abs() * 1.5f + 0.25f < (float)y / mapSize.y ? 1 : 0));
			});
		}

		public override void Draw()
		{
			tileset.GetTiles(ref tiles, (x, y) => map.Get(x, y) != 0);

			tileset.DrawTiles(in tiles, entity.position + new Vector2(0.1f, 0.1f), new Color(0, 0.1f));
			tileset.DrawTiles(in tiles, entity.position, Color.white);
		}

		public RaycastHit Raycast(Vector2 origin, Vector2 direction, int maxIterations = -1)
		{
			origin = (origin - entity.position);

			var hit = Physics.RayVsGrid(origin, direction, (x, y) => map.Get(x, y) != 0, maxIterations);

			if (hit is object)
			{
				hit.origin = origin + entity.position;
			}

			return hit;
		}
	}
}