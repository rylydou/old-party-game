using System;
using RNG = System.Random;

namespace MGE
{
	public class Perlin
	{
		int _seed = Environment.TickCount;
		public int seed
		{
			get => _seed;
			set
			{
				_seed = value;
				rng = new RNG(_seed);
			}
		}

		public RNG rng { get; private set; }
		public int[] p { get; private set; }

		public Perlin()
		{
			seed = Environment.TickCount;
			p = new int[512];
			Calculate();
		}

		public Perlin(int seed)
		{
			this.seed = seed;
			p = new int[512];
			Calculate();
		}

		public double Noise(double x, double y, double z)
		{
			int
				X = (int)Math.Floor(x) & 255,
				Y = (int)Math.Floor(y) & 255,
				Z = (int)Math.Floor(z) & 255;
			x -= Math.Floor(x);
			y -= Math.Floor(y);
			z -= Math.Floor(z);
			double u = Fade(x),
			v = Fade(y),
			w = Fade(z);
			int
				A = p[X] + Y,
				AA = p[A] + Z,
				AB = p[A + 1] + Z,
				B = p[X + 1] + Y,
				BA = p[B] + Z,
				BB = p[B + 1] + Z;
			return
			Lerp(w,
				Lerp(v, Lerp(u, Gradient(p[AA], x, y, z), Gradient(p[BA], x - 1, y, z)),
				Lerp(u, Gradient(p[AB], x, y - 1, z), Gradient(p[BB], x - 1, y - 1, z))),
				Lerp(v, Lerp(u, Gradient(p[AA + 1], x, y, z - 1), Gradient(p[BA + 1], x - 1, y, z - 1)),
				Lerp(u, Gradient(p[AB + 1], x, y - 1, z - 1), Gradient(p[BB + 1], x - 1, y - 1, z - 1))));
		}

		double Lerp(double t, double a, double b) => a + t * (b - a);

		double Fade(double t) =>
		 t * t * t * (t * (t * 6d - 15d) + 10d);

		double Gradient(int hash, double x, double y, double z)
		{
			int h = hash & 15;
			double u = h < 8 ? x : y,
			v = h < 4 ? y : h == 12 || h == 14 ? x : z;
			return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
		}

		void Calculate()
		{
			for (int i = 0; i < 256; i++)
				p[i] = p[i + 256] = rng.Next(0, 256);
		}
	}
}