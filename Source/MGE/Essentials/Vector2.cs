using System;

namespace MGE
{
	public struct Vector2
	{
		#region Static

		#region Constants
		static readonly Vector2 _zero = new Vector2(0.0, 0.0);
		public static Vector2 zero { get { return _zero; } }
		static readonly Vector2 _one = new Vector2(1.0, 1.0);
		public static Vector2 one { get { return _one; } }
		static readonly Vector2 _up = new Vector2(0.0, 1.0);
		public static Vector2 up { get { return _up; } }
		static readonly Vector2 _down = new Vector2(0.0, -1.0);
		public static Vector2 down { get { return _down; } }
		static readonly Vector2 _left = new Vector2(-1.0, 0.0);
		public static Vector2 left { get { return _left; } }
		static readonly Vector2 _right = new Vector2(1.0, 0.0);
		public static Vector2 right { get { return _right; } }
		static readonly Vector2 _positiveInfinity = new Vector2(double.PositiveInfinity, double.PositiveInfinity);
		public static Vector2 positiveInfinity { get { return _positiveInfinity; } }
		static readonly Vector2 _negativeInfinity = new Vector2(double.NegativeInfinity, double.NegativeInfinity);
		public static Vector2 negativeInfinity { get { return _negativeInfinity; } }

		public const double epsilon = Double.Epsilon;
		public const double epsilonNormalSqrt = Double.Epsilon * 2;
		#endregion

		#region Methods
		public static Vector2 Lerp(Vector2 from, Vector2 to, double time)
		{
			time = Math.Clamp01(time);
			return new Vector2(
				from.x + (to.x - from.x) * time,
				from.y + (to.y - from.y) * time
			);
		}

		public static Vector2 LerpUnclamped(Vector2 from, Vector2 to, double time)
		{
			return new Vector2(
				from.x + (to.x - from.x) * time,
				from.y + (to.y - from.y) * time
			);
		}

		public static Vector2 MoveTowards(Vector2 from, Vector2 to, double maxDistanceDelta)
		{
			double toVector_x = to.x - from.x;
			double toVector_y = to.y - from.y;

			double sqDist = toVector_x * toVector_x + toVector_y * toVector_y;

			if (sqDist == 0 || (maxDistanceDelta >= 0.0 && sqDist <= maxDistanceDelta * maxDistanceDelta)) return to;

			double dist = Math.Sqrt(sqDist);

			return new Vector2
			(
				from.x + toVector_x / dist * maxDistanceDelta,
				from.y + toVector_y / dist * maxDistanceDelta
			);
		}

		public static Vector2 Scale(Vector2 left, Vector2 right) { return new Vector2(left.x * right.x, left.y * right.y); }

		public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal)
		{
			double factor = -2F * Dot(inNormal, inDirection);
			return new Vector2(factor * inNormal.x + inDirection.x, factor * inNormal.y + inDirection.y);
		}

		public static Vector2 Perpendicular(Vector2 inDirection)
		{
			return new Vector2(-inDirection.y, inDirection.x);
		}

		public static double Dot(Vector2 left, Vector2 right) { return left.x * right.x + left.y * right.y; }

		public static double Angle(Vector2 from, Vector2 to)
		{
			double denominator = Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
			if (denominator < epsilonNormalSqrt)
				return 0.0;

			double dot = Math.Clamp(Dot(from, to) / denominator, -1.0, 1.0);
			return Math.Acos(dot) * Math.rad2Deg;
		}

		public static double SignedAngle(Vector2 from, Vector2 to)
		{
			double unsigned_angle = Angle(from, to);
			double sign = Math.Sign(from.x * to.y - from.y * to.x);
			return unsigned_angle * sign;
		}

		public static double Distance(Vector2 from, Vector2 to)
		{
			double diff_x = from.x - to.x;
			double diff_y = from.y - to.y;
			return Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
		}

		public static Vector2 Clamp(Vector2 vector, double length)
		{
			return new Vector2(Math.Clamp(vector.x, -length, length), Math.Clamp(vector.y, -length, length));
		}

		public static Vector2 Clamp(Vector2 vector, Vector2 size)
		{
			return new Vector2(Math.Clamp(vector.x, -size.x, size.x), Math.Clamp(vector.y, -size.y, size.y));
		}

		public static Vector2 ClampMagnitude(Vector2 vector, double maxLength)
		{
			double sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude > maxLength * maxLength)
			{
				double mag = Math.Sqrt(sqrMagnitude);

				double normalized_x = vector.x / mag;
				double normalized_y = vector.y / mag;
				return new Vector2(normalized_x * maxLength, normalized_y * maxLength);
			}
			return vector;
		}

		public static Vector2 Min(Vector2 a, Vector2 b) { return new Vector2(Math.Min(a.x, b.x), Math.Min(a.y, b.y)); }

		public static Vector2 Max(Vector2 a, Vector2 b) { return new Vector2(Math.Max(a.x, b.x), Math.Max(a.y, b.y)); }

		// public static Vector2 SmoothDamp(Vector2 from, Vector2 to, ref Vector2 currentVelocity, double smoothTime, double maxSpeed)
		// {
		// 	double deltaTime = Time.deltaTime;
		// 	return SmoothDamp(from, to, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		// }

		// public static Vector2 SmoothDamp(Vector2 from, Vector2 to, ref Vector2 currentVelocity, double smoothTime)
		// {
		// 	double deltaTime = Time.deltaTime;
		// 	double maxSpeed = Math.infinity;
		// 	return SmoothDamp(from, to, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		// }

		// public static Vector2 SmoothDamp(Vector2 from, Vector2 to, ref Vector2 currentVelocity, double smoothTime, double maxSpeed, double deltaTime = double.NaN)
		// {
		// 	if (double.IsNaN(deltaTime)) deltaTime = Time.deltaTime;

		// 	// Based on Game Programming Gems 4 Chapter 1.10
		// 	smoothTime = Math.Max(0.0001, smoothTime);
		// 	double omega = 2F / smoothTime;

		// 	double x = omega * deltaTime;
		// 	double exp = 1.0 / (1.0 + x + 0.48 * x * x + 0.235 * x * x * x);

		// 	double change_x = from.x - to.x;
		// 	double change_y = from.y - to.y;
		// 	Vector2 originalTo = to;

		// 	// Clamp maximum speed
		// 	double maxChange = maxSpeed * smoothTime;

		// 	double maxChangeSq = maxChange * maxChange;
		// 	double sqDist = change_x * change_x + change_y * change_y;
		// 	if (sqDist > maxChangeSq)
		// 	{
		// 		var mag = Math.Sqrt(sqDist);
		// 		change_x = change_x / mag * maxChange;
		// 		change_y = change_y / mag * maxChange;
		// 	}

		// 	to.x = from.x - change_x;
		// 	to.y = from.y - change_y;

		// 	double temp_x = (currentVelocity.x + omega * change_x) * deltaTime;
		// 	double temp_y = (currentVelocity.y + omega * change_y) * deltaTime;

		// 	currentVelocity.x = (currentVelocity.x - omega * temp_x) * exp;
		// 	currentVelocity.y = (currentVelocity.y - omega * temp_y) * exp;

		// 	double output_x = to.x + (change_x + temp_x) * exp;
		// 	double output_y = to.y + (change_y + temp_y) * exp;

		// 	// Prevent overshooting
		// 	double origMinusCurrent_x = originalTo.x - from.x;
		// 	double origMinusCurrent_y = originalTo.y - from.y;
		// 	double outMinusOrig_x = output_x - originalTo.x;
		// 	double outMinusOrig_y = output_y - originalTo.y;

		// 	if (origMinusCurrent_x * outMinusOrig_x + origMinusCurrent_y * outMinusOrig_y > 0)
		// 	{
		// 		output_x = originalTo.x;
		// 		output_y = originalTo.y;

		// 		currentVelocity.x = (output_x - originalTo.x) / deltaTime;
		// 		currentVelocity.y = (output_y - originalTo.y) / deltaTime;
		// 	}
		// 	return new Vector2(output_x, output_y);
		// }
		#endregion

		#endregion

		#region Object

		#region Variables
		public double x;

		public double y;

		public double this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return x;
					case 1: return y;
					default: throw new IndexOutOfRangeException("Invalid Vector2 index!");
				}
			}

			set
			{
				switch (index)
				{
					case 0: x = value; break;
					case 1: y = value; break;
					default: throw new IndexOutOfRangeException("Invalid Vector2 index!");
				}
			}
		}
		#endregion

		#region Properties
		public Vector2 normalized
		{
			get
			{
				Vector2 vector = new Vector2(x, y);
				vector.Normalize();
				return vector;
			}
		}

		public double magnitude { get { return Math.Sqrt(x * x + y * y); } }
		public double sqrMagnitude { get { return x * x + y * y; } }
		#endregion

		#region Constructors
		public Vector2(double value)
		{
			this.x = value;
			this.y = value;
		}

		public Vector2(double x, double y)
		{
			this.x = x;
			this.y = y;
		}
		#endregion

		#region Methods
		public void Set(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		public void Scale(Vector2 scale) { x *= scale.x; y *= scale.y; }

		public void Normalize()
		{
			double mag = magnitude;
			if (mag > epsilon)
				this = this / mag;
			else
				this = zero;
		}
		#endregion

		#region Implicit
		public static Vector2 operator +(Vector2 left, Vector2 right) { return new Vector2(left.x + right.x, left.y + right.y); }
		public static Vector2 operator -(Vector2 left, Vector2 right) { return new Vector2(left.x - right.x, left.y - right.y); }
		public static Vector2 operator *(Vector2 left, Vector2 right) { return new Vector2(left.x * right.x, left.y * right.y); }
		public static Vector2 operator /(Vector2 left, Vector2 right) { return new Vector2(left.x / right.x, left.y / right.y); }

		public static Vector2 operator -(Vector2 vector) { return new Vector2(-vector.x, -vector.y); }

		public static Vector2 operator *(Vector2 left, double right) { return new Vector2(left.x * right, left.y * right); }
		public static Vector2 operator *(double left, Vector2 right) { return new Vector2(right.x * left, right.y * left); }
		public static Vector2 operator /(Vector2 left, double right) { return new Vector2(left.x / right, left.y / right); }

		public static bool operator ==(Vector2 left, Vector2 right)
		{
			double diff_x = left.x - right.x;
			double diff_y = left.y - right.y;
			return (diff_x * diff_x + diff_y * diff_y) < epsilon * epsilon;
		}
		public static bool operator !=(Vector2 lhs, Vector2 rhs) => !(lhs == rhs);

		public static implicit operator bool(Vector2 vector) => vector != Vector2.zero;

		public static implicit operator Microsoft.Xna.Framework.Vector2(Vector2 vector) => new Microsoft.Xna.Framework.Vector2((float)vector.x, (float)vector.y);
		public static implicit operator Vector2(Microsoft.Xna.Framework.Vector2 vector) => new Vector2(vector.X, vector.Y);
		public static implicit operator Vector2(Microsoft.Xna.Framework.Point point) => new Vector2(point.X, point.Y);
		public static implicit operator Microsoft.Xna.Framework.Point(Vector2 point) => new Microsoft.Xna.Framework.Point((int)point.x, (int)point.y);
		public static implicit operator Vector2Int(Vector2 point) => new Microsoft.Xna.Framework.Point((int)point.x, (int)point.y);
		public static implicit operator Vector2(Vector2Int point) => new Microsoft.Xna.Framework.Point(point.x, point.y);
		#endregion

		#region Inherited
		public override string ToString() => $"({Math.Round(x, 2)}, {Math.Round(x, 2)})";

		public string ToString(bool round = true)
		{
			if (round)
				return ToString();
			else
				return $"({x}, {x})";
		}

		public string ToString(string format) => string.Format(format, x, y);

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ (y.GetHashCode() << 2);
		}

		public bool Equals(Vector2 other)
		{
			return x == other.x && y == other.y;
		}

		public override bool Equals(object other)
		{
			if (!(other is Vector2)) return false;

			return Equals((Vector2)other);
		}
		#endregion

		#endregion
	}
}