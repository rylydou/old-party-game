using MGE;

namespace GAME.World.Generation
{
	public class GenTest : IGenerator
	{
		public void Generate(ref Tile[,] world, int seed)
		{
			Random.seed = seed;
			var perlin = new Perlin(seed);

			Vector2Int size = new Vector2Int(world.GetLength(0), world.GetLength(1));

			// Place Air
			for (int y = 0; y < size.y; y++)
				for (int x = 0; x < size.x; x++)
					world[x, y] = new Air();

			// Place large Sand blobs
			for (int y = 0; y < size.y; y++)
				for (int x = 0; x < size.x; x++)
				{
					var value = perlin.Noise((float)x / size.x * 10, (float)y / size.y * 5, (float)(x + y) / size.x + size.y);

					if (value < -0.25)
						world[x, y] = new Sand();
				}

			// Place small Sand blobs
			for (int y = 0; y < size.y; y++)
				for (int x = 0; x < size.x; x++)
				{
					var value = perlin.Noise((float)x / size.x * 40, (float)y / size.y * 20, (float)(x + y) / size.x + size.y);

					if (value < -0.33)
						world[x, y] = new Sand();
				}

			// Place Sand mountains
			for (int x = 0; x < size.x; x++)
				for (int y = size.y - 1; y >= size.y - perlin.Noise((float)x / size.x * 15, 0, 0) * 16 - 50; y--)
					world[x, y] = new Sand();

			int lineThickness = Random.Int(size.x / 4, size.x / 3);

			// Replace Sand > Salt
			for (int x = size.x / 2 - lineThickness / 2; x < size.x / 2 + lineThickness / 2; x++)
				for (int y = 0; y < size.y; y++)
					if (Random.Bool(1.0f / 10.0f))
						if (world[x, y].GetType() == typeof(Sand)) world[x, y] = new Salt();

			// Replace Salt > Sodium
			for (int x = size.x / 2 - lineThickness / 2; x < size.x / 2 + lineThickness / 2; x++)
				for (int y = 0; y < size.y; y++)
					if (Random.Bool(1.0f / 3.0f))
						if (world[x, y].GetType() == typeof(Salt)) world[x, y] = new Sodium();

			// Replace Sand @ edges > Water
			for (int y = 0; y < size.y; y++)
				for (int x = 0; x < size.x; x++)
				{
					if (y > size.y / 6)
						if (x < size.x / 8 || x > size.x / 8 * 7)
							if (world[x, y].GetType() == typeof(Sand)) world[x, y] = new Water();
				}

			// Place stone base
			for (int x = 0; x < size.x; x++)
				for (int y = size.y - 1; y >= size.y - perlin.Noise((float)x / size.x * 25, 0, 0) * 4 - 25; y--)
					world[x, y] = new Stone();

			// Place floating Sand @ top
			for (int x = 0; x < size.x; x++)
				for (int i = 0; i < Random.Int(0, 8); i++)
					world[x, Random.Int(0, 16)] = new Sand();

			// Place floating Acid @ top
			for (int x = 0; x < size.x; x++)
				if (Random.Bool(1f / 4))
					world[x, Random.Int(0, 8)] = new Acid();
		}
	}
}