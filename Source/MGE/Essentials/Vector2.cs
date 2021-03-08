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
		public static Vector2 Lerp(Vector2 current, Vector2 target, double time)
		{
			time = Math.Clamp01(time);
			return new Vector2(
				current.x + (target.x - current.x) * time,
				current.y + (target.y - current.y) * time
			);
		}

		public static Vector2 LerpUnclamped(Vector2 current, Vector2 target, double time)
		{
			return new Vector2(
				current.x + (target.x - current.x) * time,
				current.y + (target.y - current.y) * time
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

		public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal)
		{
			double factor = -2F * Dot(inNormal, inDirection);
			return new Vector2(factor * inNormal.x + inDirection.x, factor * inNormal.y + inDirection.y);
		}

		public static Vector2 Perpendicular(Vector2 inDirection) => new Vector2(-inDirection.y, inDirection.x);

		public static double Dot(Vector2 left, Vector2 right) => left.x * right.x + left.y * right.y;

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
			double unsignedAngle = Angle(from, to);
			double sign = Math.Sign(from.x * to.y - from.y * to.x);
			return unsignedAngle * sign;
		}

		public static bool DistanceET(Vector2 from, Vector2 to, double value) =>
			Math.Approximately(DistanceSqr(from, to), value * value);
		public static bool DistanceLT(Vector2 from, Vector2 to, double value) =>
			DistanceSqr(from, to) < value * value;
		public static bool DistanceGT(Vector2 from, Vector2 to, double value) =>
			DistanceSqr(from, to) > value * value;

		public static double Distance(Vector2 from, Vector2 to)
		{
			return Math.Sqrt(DistanceSqr(from, to));
		}

		public static double DistanceSqr(Vector2 from, Vector2 to) => (from - to).sqrMagnitude;

		public static Vector2 Clamp(Vector2 vector, double length) =>
			new Vector2(Math.Clamp(vector.x, -length, length), Math.Clamp(vector.y, -length, length));

		public static Vector2 Clamp(Vector2 vector, Vector2 size) =>
			new Vector2(Math.Clamp(vector.x, -size.x, size.x), Math.Clamp(vector.y, -size.y, size.y));

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
					default: throw new IndexOutOfRangeException($"Invalid Vector2 index of {index}!");
				}
			}

			set
			{
				switch (index)
				{
					case 0: x = value; break;
					case 1: y = value; break;
					default: throw new IndexOutOfRangeException($"Invalid Vector2 index of {index}!");
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

		public double sqrMagnitude { get => x * x + y * y; }
		public double magnitude { get => Math.Sqrt(sqrMagnitude); }

		public double max { get => Math.Max(x, y); }
		public double min { get => Math.Min(x, y); }
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
		public void Normalize()
		{
			var mag = magnitude;

			if (mag > epsilon)
				this = this / mag;
			else
				this = zero;
		}

		public void Clamp(double max)
		{
			x = Math.Clamp(x, 0, max);
			y = Math.Clamp(y, 0, max);
		}

		public void Clamp(double min, double max)
		{
			x = Math.Clamp(x, min, max);
			y = Math.Clamp(y, min, max);
		}

		public void Clamp(double minX, double minY, double maxX, double maxY)
		{
			x = Math.Clamp(x, minX, maxX);
			y = Math.Clamp(y, minY, maxY);
		}
		#endregion

		#region Implicit
		public static Vector2 operator +(Vector2 left, Vector2 right) => new Vector2(left.x + right.x, left.y + right.y);
		public static Vector2 operator -(Vector2 left, Vector2 right) => new Vector2(left.x - right.x, left.y - right.y);
		public static Vector2 operator *(Vector2 left, Vector2 right) => new Vector2(left.x * right.x, left.y * right.y);
		public static Vector2 operator /(Vector2 left, Vector2 right) => new Vector2(left.x / right.x, left.y / right.y);

		public static Vector2 operator -(Vector2 vector) => new Vector2(-vector.x, -vector.y);

		public static Vector2 operator *(Vector2 left, double right) => new Vector2(left.x * right, left.y * right);
		public static Vector2 operator *(double left, Vector2 right) => new Vector2(right.x * left, right.y * left);
		public static Vector2 operator /(Vector2 left, double right) => new Vector2(left.x / right, left.y / right);

		public static bool operator ==(Vector2 left, Vector2 right)
		{
			var diff_x = left.x - right.x;
			var diff_y = left.y - right.y;
			return Math.Abs(diff_x * diff_x + diff_y * diff_y) < epsilonNormalSqrt;
		}
		public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);

		public static implicit operator bool(Vector2 vector) => vector != Vector2.zero;

		public static implicit operator Microsoft.Xna.Framework.Vector2(Vector2 vector) => new Microsoft.Xna.Framework.Vector2((float)vector.x, (float)vector.y);
		public static implicit operator Vector2(Microsoft.Xna.Framework.Vector2 vector) => new Vector2(vector.X, vector.Y);
		public static implicit operator Vector2(Microsoft.Xna.Framework.Point point) => new Vector2(point.X, point.Y);
		public static implicit operator Microsoft.Xna.Framework.Point(Vector2 point) => new Microsoft.Xna.Framework.Point((int)point.x, (int)point.y);
		public static implicit operator Vector2Int(Vector2 point) => new Microsoft.Xna.Framework.Point((int)point.x, (int)point.y);
		public static implicit operator Vector2(Vector2Int point) => new Microsoft.Xna.Framework.Point(point.x, point.y);
		#endregion

		#region Inherited
		public override string ToString() => ToString(2);

		public string ToString(int amountOfRounding = -1)
		{
			if (amountOfRounding < 0)
				return $"({x}, {y})";
			else
				return $"({Math.Round(x, amountOfRounding)}, {Math.Round(y, amountOfRounding)})";
		}

		public string ToString(string format) => string.Format(format, x, y);

		public override int GetHashCode() => (x.GetHashCode() >> 2) ^ (y.GetHashCode() << 2);

		public bool Equals(Vector2 other) => x == other.x && y == other.y;

		public override bool Equals(object other)
		{
			if (!(other is Vector2)) return false;

			return Equals((Vector2)other);
		}
		#endregion

		#endregion
	}
}