using MGE;

namespace GAME.Generators
{
	public class PlainsGenerator : IGenerator
	{
		const float maxSize = 64.0f;

		Perlin perlin;

		public PlainsGenerator(int seed)
		{
			this.perlin = new Perlin(seed);
		}

		public ushort Generate(int x, int y)
		{
			// Underground
			if (y > 16)
			{
				return 2;
			}

			// Ground
			if (y > 0)
			{
				if (Random.Bool(Math.Sin(x * Random.Float(0, 1)) + (float)y / Random.Float(0.1f, 8f)))
					return 1;
			}

			return 0;
		}
	}
}