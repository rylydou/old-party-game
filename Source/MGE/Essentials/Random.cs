using System;
using RNG = System.Random;

namespace MGE
{
	public static class Random
	{
		static int _seed = Environment.TickCount;
		public static int seed
		{
			get => _seed;
			set
			{
				_seed = value;
				rng = new RNG(_seed);
			}
		}

		public static RNG rng { get; private set; } = new RNG(_seed);
		public static Perlin perlin { get; private set; } = new Perlin(_seed);

		public static bool Bool() => rng.Next(2) == 0 ? false : true;
		public static bool Bool(float chance) => Float() < chance;
		public static bool Bool(double chance) => Double() < chance;
		public static bool Bool(decimal chance) => Decimal() < chance;
		public static bool Bool(int chance) => Int(100) < chance;

		public static int Int() => rng.Next();
		public static int Int(int max) => rng.Next(max + 1);
		public static int Int(int min, int max) => rng.Next(min, max + 1);

		public static float Float() => (float)rng.NextDouble();
		public static float Float(float max) => Float() * max;
		public static float Float(float min, float max) => min + Float(max - min);

		public static double Double() => rng.NextDouble();
		public static double Double(float max) => Double() * max;
		public static double Double(float min, float max) => min + Double(max - min);

		public static decimal Decimal() => (decimal)rng.NextDouble();
		public static decimal Decimal(decimal max) => Decimal() * max;
		public static decimal Decimal(decimal min, decimal max) => min + Decimal(max - min);

		public static float Angle() => Float() * Math.pi2;

		public static Color Color() => new Color(Float(), Float(), Float());

		public static Vector2 UnitVector() => Vector().normalized;
		public static Vector2 Vector() => new Vector2(Float(-1, 1), Float(-1, 1));
		public static Vector2 Vector(float max) => new Vector2(Float(-max, max), Float(-max, max));
		public static Vector2 Vector(float min, float max) => new Vector2(Float(min, max), Float(min, max));

		public static float Noise(float x, float y, float z) => perlin.Noise(x, y, z);
	}
}