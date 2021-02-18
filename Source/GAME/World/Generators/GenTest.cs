using MGE;

namespace GAME.World.Generation
{
	public class GenTest : IGenerator
	{
		public void Generate(ref int[,] world)
		{
			var perlin = new Perlin("cat".GetHashCode());
			Vector2Int size = new Vector2Int(world.GetLength(0), world.GetLength(1));

			for (int y = 0; y < size.y; y++)
			{
				for (int x = 0; x < size.x; x++)
				{
					var value = perlin.Noise((double)x / size.x * 25, (double)y / size.y * 25, (double)(x + y) / size.x + size.y);

					if (value < 0.0)
						world[x, y] = 2;
					else
						world[x, y] = 1;
				}
			}
		}
	}
}