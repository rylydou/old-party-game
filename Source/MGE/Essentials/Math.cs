using System;
using Mathd = System.Math;

namespace MGE
{
	public static class Math
	{
		#region Constants
		public const double e = 2.71828175;
		public const double log10E = 0.4342945;
		public const double Log2E = 1.442695;
		public const double pi = Mathd.PI;
		public const double pi2 = Mathd.PI * 2;
		public const double tau = Mathd.PI * 2;
		public const double piOver2 = Mathd.PI / 2;
		public const double piOver4 = Mathd.PI / 4;
		public const double infinity = Double.PositiveInfinity;
		public const double negativeInfinity = Double.NegativeInfinity;
		public const double deg2Rad = pi * 2.0 / 360.0;
		public const double rad2Deg = 1.0 / deg2Rad;
		public const double epsilon = Double.Epsilon;
		#endregion

		#region General
		public static double Sin(double value) => Mathd.Sin(value);
		public static double Cos(double value) => Mathd.Cos(value);
		public static double Tan(double value) => Mathd.Tan(value);
		public static double Asin(double value) => Mathd.Asin(value);
		public static double Acos(double value) => Mathd.Acos(value);
		public static double Atan(double value) => Mathd.Atan(value);
		public static double Atan2(double y, double x) => Mathd.Atan2(y, x);
		public static double Sqrt(double value) => Mathd.Sqrt(value);
		public static double Abs(double value) => Mathd.Abs(value);
		public static int Abs(int value) => Mathd.Abs(value);
		public static double Pow(double value, double power) => Mathd.Pow(value, power);
		public static double Exp(double power) => Mathd.Exp(power);
		public static double Log(double value, double power) => Mathd.Log(value, power);
		public static double Log(double value) => Mathd.Log(value);
		public static double Log10(double value) => Mathd.Log10(value);
		public static double Ceil(double value) => Mathd.Ceiling(value);
		public static double Floor(double value) => Mathd.Floor(value);
		public static double Round(double value) => Mathd.Round(value);
		public static double Round(double value, int places)
		{
			var amount = (int)Math.Pow(10, places);
			return Round(value * amount) / amount;
		}
		public static int CeilToInt(double value) => (int)Mathd.Ceiling(value);
		public static int FloorToInt(double value) => (int)Mathd.Floor(value);
		public static int RoundToInt(double value) => (int)Mathd.Round(value);
		public static double Sign(double value) => value >= 0.0 ? 1.0 : -1.0;
		#endregion

		#region Min Maxes
		public static double Min(double a, double b) { return a < b ? a : b; }

		public static double Min(params double[] values)
		{
			int length = values.Length;
			if (length == 0)
				return 0;
			double min = values[0];
			for (int i = 1; i < length; i++)
			{
				if (values[i] < min)
					min = values[i];
			}
			return min;
		}
		public static int Min(int a, int b) { return a < b ? a : b; }

		public static int Min(params int[] values)
		{
			int length = values.Length;
			if (length == 0)
				return 0;
			int min = values[0];
			for (int i = 1; i < length; i++)
			{
				if (values[i] < min)
					min = values[i];
			}
			return min;
		}

		public static double Max(double a, double b) { return a > b ? a : b; }

		public static double Max(params double[] values)
		{
			int length = values.Length;
			if (length == 0)
				return 0;
			double min = values[0];
			for (int i = 1; i < length; i++)
			{
				if (values[i] > min)
					min = values[i];
			}
			return min;
		}

		public static int Max(int a, int b) { return a > b ? a : b; }

		public static int Max(params int[] values)
		{
			int length = values.Length;
			if (length == 0)
				return 0;
			int min = values[0];
			for (int i = 1; i < length; i++)
			{
				if (values[i] > min)
					min = values[i];
			}
			return min;
		}
		#endregion

		#region Clamps
		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			if (value.CompareTo(min) < 0)
				value = min;
			else if (value.CompareTo(max) > 0)
				value = max;
			return value;
		}

		public static int Clamp(int value, int min, int max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		public static double Clamp(double value, double min, double max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		public static decimal Clamp(decimal value, decimal min, decimal max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		public static float Clamp01(float value)
		{
			if (value < 0.0f)
				return 0.0f;
			else if (value > 1.0f)
				return 1.0f;
			else
				return value;
		}

		public static double Clamp01(double value)
		{
			if (value < 0.0)
				return 0.0;
			else if (value > 1.0)
				return 1.0;
			else
				return value;
		}

		public static decimal Clamp01(decimal value)
		{
			if (value < 0.0m)
				return 0.0m;
			else if (value > 1.0m)
				return 1.0m;
			else
				return value;
		}
		#endregion

		#region Lerps
		public static double Lerp(double current, double target, double time) =>
		current + (target - current) * Clamp01(time);

		public static double LerpUnclamped(double current, double target, double time) =>
		current + (target - current) * time;

		public static double LerpPrecise(double current, double target, double time)
		{
			time = Clamp01(time);
			return ((1 - time) * current) + (target * time);
		}

		public static double LerpPreciseUnclamped(double current, double target, double time) =>
		((1 - time) * current) + (target * time);

		public static double InverseLerp(double current, double target, double time)
		{
			if (current != target)
				return Clamp01((time - current) / (target - current));
			else
				return 0.0;
		}

		public static double LerpAngle(double current, double target, double time)
		{
			double delta = Repeat((target - current), 360.0);
			if (delta > 180.0)
				delta -= 360.0;
			return current + delta * Clamp01(time);
		}

		static public double MoveTowards(double current, double target, double maxDelta)
		{
			if (Abs(target - current) <= maxDelta)
				return target;
			return current + Sign(target - current) * maxDelta;
		}

		static public double MoveTowardsAngle(double current, double target, double maxDelta)
		{
			double deltaAngle = DeltaAngle(current, target);
			if (-maxDelta < deltaAngle && deltaAngle < maxDelta)
				return target;
			target = current + deltaAngle;
			return MoveTowards(current, target, maxDelta);
		}

		public static double SmoothStep(double from, double to, double time)
		{
			time = Math.Clamp01(time);
			time = -2.0 * time * time * time + 3.0 * time * time;
			return to * time + from * (1 - time);
		}
		#endregion

		#region Utils
		public static double Repeat(double target, double length)
		{
			return Clamp(target - Floor(target / length) * length, 0.0, length);
		}

		public static double DeltaAngle(double current, double target)
		{
			double delta = Repeat((target - current), 360.0);
			if (delta > 180.0)
				delta -= 360.0;
			return delta;
		}

		public static bool Approximately(double a, double b) => Abs(b - a) < Max(0.000001 * Max(Abs(a), Abs(b)), epsilon * 8);

		public static int BitwiseDouble(int value) => value << 1;
		public static int BitwiseHalf(int value) => value >> 1;
		#endregion
	}
}