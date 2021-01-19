using System;
using System.Collections.Generic;
using System.Collections;
using Mathd = System.Math;

namespace MGE
{
	public struct Math
	{
		#region Constants
		public const double pi = Mathd.PI;
		public const double doublePI = Mathd.PI * 2;
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
		public static double Clamp(double value, double min, double max)
		{
			if (value < min)
				value = min;
			else if (value > max)
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

		public static double Clamp01(double value)
		{
			if (value < 0.0)
				return 0.0;
			else if (value > 1.0)
				return 1.0;
			else
				return value;
		}
		#endregion

		#region Lerps
		public static double Lerp(double a, double b, double t)
		{
			return a + (b - a) * Clamp01(t);
		}

		public static double LerpUnclamped(double a, double b, double t)
		{
			return a + (b - a) * t;
		}

		public static double InverseLerp(double a, double b, double value)
		{
			if (a != b)
				return Clamp01((value - a) / (b - a));
			else
				return 0.0;
		}

		public static double LerpAngle(double a, double b, double t)
		{
			double delta = Repeat((b - a), 360.0);
			if (delta > 180.0)
				delta -= 360.0;
			return a + delta * Clamp01(t);
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

		public static double SmoothStep(double from, double to, double t)
		{
			t = Math.Clamp01(t);
			t = -2.0 * t * t * t + 3.0 * t * t;
			return to * t + from * (1 - t);
		}

		public static double SmoothDamp(double current, double target, ref double currentVelocity, double smoothTime, double maxSpeed)
		{
			double deltaTime = Time.deltaTime;
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static double SmoothDamp(double current, double target, ref double currentVelocity, double smoothTime)
		{
			double deltaTime = Time.deltaTime;
			double maxSpeed = Math.infinity;
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static double SmoothDamp(double current, double target, ref double currentVelocity, double smoothTime, double maxSpeed, double deltaTime)
		{
			smoothTime = Max(0.0001, smoothTime);
			double omega = 2 / smoothTime;

			double x = omega * deltaTime;
			double exp = 1 / (1 + x + 0.48 * x * x + 0.235 * x * x * x);
			double change = current - target;
			double originalTo = target;

			double maxChange = maxSpeed * smoothTime;
			change = Math.Clamp(change, -maxChange, maxChange);
			target = current - change;

			double temp = (currentVelocity + omega * change) * deltaTime;
			currentVelocity = (currentVelocity - omega * temp) * exp;
			double output = target + (change + temp) * exp;

			if (originalTo - current > 0.0 == output > originalTo)
			{
				output = originalTo;
				currentVelocity = (output - originalTo) / deltaTime;
			}

			return output;
		}

		public static double SmoothDampAngle(double current, double target, ref double currentVelocity, double smoothTime, double maxSpeed)
		{
			double deltaTime = Time.deltaTime;
			return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static double SmoothDampAngle(double current, double target, ref double currentVelocity, double smoothTime)
		{
			double deltaTime = Time.deltaTime;
			double maxSpeed = Math.infinity;
			return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static double SmoothDampAngle(double current, double target, ref double currentVelocity, double smoothTime, double maxSpeed, double deltaTime)
		{
			target = current + DeltaAngle(current, target);
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}
		#endregion

		#region Utils
		public static double Repeat(double t, double length)
		{
			return Clamp(t - Floor(t / length) * length, 0.0, length);
		}

		public static double DeltaAngle(double current, double target)
		{
			double delta = Repeat((target - current), 360.0);
			if (delta > 180.0)
				delta -= 360.0;
			return delta;
		}

		public static bool Approximately(double a, double b)
		{
			return Abs(b - a) < Max(0.000001 * Max(Abs(a), Abs(b)), epsilon * 8);
		}

		public static int QuickDouble(int value) => value << 1;
		public static int QuickHalf(int value) => value >> 1;
		#endregion
	}
}