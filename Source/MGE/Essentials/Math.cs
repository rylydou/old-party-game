using System;
using Mathf = System.MathF;

namespace MGE
{
	public static class Math
	{
		#region Constants
		public const float e = 2.71828175f;
		public const float log10E = 0.4342945f;
		public const float Log2E = 1.442695f;
		public const float pi = Mathf.PI;
		public const float pi2 = Mathf.PI * 2;
		public const float pi4 = Mathf.PI * 4;
		public const float piOver2 = Mathf.PI / 2;
		public const float piOver4 = Mathf.PI / 4;
		public const float infinity = Single.PositiveInfinity;
		public const float negativeInfinity = Single.NegativeInfinity;
		public const float deg2Rad = pi * 2.0f / 360.0f;
		public const float rad2Deg = 1.0f / deg2Rad;
		public const float epsilon = Single.Epsilon;
		#endregion

		#region General
		public static float Sin(float value) => Mathf.Sin(value);
		public static float Cos(float value) => Mathf.Cos(value);
		public static float Tan(float value) => Mathf.Tan(value);
		public static float Asin(float value) => Mathf.Asin(value);
		public static float Acos(float value) => Mathf.Acos(value);
		public static float Atan1(float value) => Mathf.Atan(value);
		public static float Atan(float y, float x) => Mathf.Atan2(y, x);
		public static float Sqrt(float value) => Mathf.Sqrt(value);
		public static float Abs(float value) => Mathf.Abs(value);
		public static int Abs(int value) => (int)Mathf.Abs(value);
		public static float Pow(float value, float power) => Mathf.Pow(value, power);
		public static float Exp(float power) => Mathf.Exp(power);
		public static float Log(float value, float power) => Mathf.Log(value, power);
		public static float Log(float value) => Mathf.Log(value);
		public static float Log10(float value) => Mathf.Log10(value);
		public static float Ceil(float value) => Mathf.Ceiling(value);
		public static float Floor(float value) => Mathf.Floor(value);
		public static float Round(float value) => Mathf.Round(value);
		public static int CeilToInt(float value) => (int)Mathf.Ceiling(value);
		public static int FloorToInt(float value) => (int)Mathf.Floor(value);
		public static int RoundToInt(float value) => (int)Mathf.Round(value);
		public static float Sign(float value) => value == 0.0f ? 0.0f : value >= 0.0f ? 1.0f : -1.0f;
		public static int Sign(int value) => value == 0 ? 0 : value >= 0 ? 1 : -1;
		#endregion

		#region Min Maxes
		public static float Min(float a, float b) => a < b ? a : b;

		public static float Min(params float[] values)
		{
			int length = values.Length;
			if (length == 0)
				return 0;
			float min = values[0];
			for (int i = 1; i < length; i++)
			{
				if (values[i] < min)
					min = values[i];
			}
			return min;
		}
		public static int Min(int a, int b) => a < b ? a : b;

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

		public static float Max(float a, float b) => a > b ? a : b;

		public static float Max(params float[] values)
		{
			int length = values.Length;
			if (length == 0)
				return 0;
			float min = values[0];
			for (int i = 1; i < length; i++)
			{
				if (values[i] > min)
					min = values[i];
			}
			return min;
		}

		public static int Max(int a, int b) => a > b ? a : b;

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

		public static int Wrap(int value, int max) => Wrap(value, 0, max);

		public static int Wrap(int value, int min, int max)
		{
			if (value < min)
				return max - (min - value) % (max - min);
			else
				return min + (value - min) % (max - min);
		}

		public static float Wrap(float value, float max) => Wrap(value, 0, max);

		public static float Wrap(float value, float min, float max)
		{
			if (value < min)
				return max - (min - value) % (max - min);
			else
				return min + (value - min) % (max - min);
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

		public static int Clamp(int value, int max) => Clamp(value, 0, max);
		public static int Clamp(int value, int min, int max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		public static float Clamp(float value, float max) => Clamp(value, 0, max);
		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		public static double Clamp(double value, double max) => Clamp(value, 0, max);
		public static double Clamp(double value, double min, double max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		public static decimal Clamp(decimal value, decimal max) => Clamp(value, 0, max);
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

		public static float Clamp11(float value)
		{
			if (value < -1.0f)
				return -1.0f;
			else if (value > 1.0f)
				return 1.0f;
			else
				return value;
		}
		#endregion

		#region Lerps
		public static float Lerp(float current, float target, float time) =>
		current + (target - current) * Clamp01(time);

		public static float LerpUnclamped(float current, float target, float time) =>
		current + (target - current) * time;

		public static float LerpPrecise(float current, float target, float time)
		{
			time = Clamp01(time);
			return ((1 - time) * current) + (target * time);
		}

		public static float LerpPreciseUnclamped(float current, float target, float time) =>
		((1 - time) * current) + (target * time);

		public static float InverseLerp(float current, float target, float time)
		{
			if (current != target)
				return Clamp01((time - current) / (target - current));
			else
				return 0.0f;
		}

		public static float LerpAngle(float current, float target, float time)
		{
			float delta = Repeat((target - current), 360.0f);
			if (delta > 180.0f)
				delta -= 360.0f;
			return current + delta * Clamp01(time);
		}

		static public float MoveTowards(float current, float target, float maxDelta)
		{
			if (Abs(target - current) <= maxDelta)
				return target;
			return current + Sign(target - current) * maxDelta;
		}

		static public float MoveTowardsAngle(float current, float target, float maxDelta)
		{
			float deltaAngle = DeltaAngle(current, target);
			if (-maxDelta < deltaAngle && deltaAngle < maxDelta)
				return target;
			target = current + deltaAngle;
			return MoveTowards(current, target, maxDelta);
		}

		public static float SmoothStep(float from, float to, float time)
		{
			time = Math.Clamp01(time);
			time = -2.0f * time * time * time + 3.0f * time * time;
			return to * time + from * (1 - time);
		}
		#endregion

		#region Utils
		public static float Repeat(float target, float length)
		{
			return Clamp(target - Floor(target / length) * length, 0.0f, length);
		}

		public static float DeltaAngle(float current, float target)
		{
			float delta = Repeat((target - current), 360.0f);
			if (delta > 180.0f)
				delta -= 360.0f;
			return delta;
		}

		public static bool Approximately(float a, float b) => Abs(b - a) < Max(0.000001f * Max(Abs(a), Abs(b)), epsilon * 8);

		public static int BitwiseDouble(int value) => value << 1;
		public static int BitwiseHalf(int value) => value >> 1;
		#endregion
	}
}