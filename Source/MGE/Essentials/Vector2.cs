using System;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public struct Vector2
	{
		#region Static

		#region Constants
		static readonly Vector2 _zero = new Vector2(0.0f, 0.0f);
		public static Vector2 zero { get { return _zero; } }
		static readonly Vector2 _one = new Vector2(1.0f, 1.0f);
		public static Vector2 one { get { return _one; } }
		static readonly Vector2 _up = new Vector2(0.0f, 1.0f);
		public static Vector2 up { get { return _up; } }
		static readonly Vector2 _down = new Vector2(0.0f, -1.0f);
		public static Vector2 down { get { return _down; } }
		static readonly Vector2 _left = new Vector2(-1.0f, 0.0f);
		public static Vector2 left { get { return _left; } }
		static readonly Vector2 _right = new Vector2(1.0f, 0.0f);
		public static Vector2 right { get { return _right; } }
		static readonly Vector2 _positiveInfinity = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
		public static Vector2 positiveInfinity { get { return _positiveInfinity; } }
		static readonly Vector2 _negativeInfinity = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
		public static Vector2 negativeInfinity { get { return _negativeInfinity; } }

		public const float epsilon = Single.Epsilon;
		public const float epsilonNormalSqrt = Single.Epsilon * 2;
		#endregion

		#region Methods
		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Math.Cos(angle), Math.Sin(angle));
		}

		public static Vector2 Lerp(Vector2 current, Vector2 target, float time)
		{
			time = Math.Clamp01(time);
			return new Vector2(
				current.x + (target.x - current.x) * time,
				current.y + (target.y - current.y) * time
			);
		}

		public static Vector2 LerpUnclamped(Vector2 current, Vector2 target, float time)
		{
			return new Vector2(
				current.x + (target.x - current.x) * time,
				current.y + (target.y - current.y) * time
			);
		}

		public static Vector2 MoveTowards(Vector2 from, Vector2 to, float maxDistanceDelta)
		{
			var toVector_x = to.x - from.x;
			var toVector_y = to.y - from.y;

			var sqDist = toVector_x * toVector_x + toVector_y * toVector_y;

			if (sqDist == 0 || (maxDistanceDelta >= 0.0f && sqDist <= maxDistanceDelta * maxDistanceDelta)) return to;

			var dist = Math.Sqrt(sqDist);

			return new Vector2
			(
				from.x + toVector_x / dist * maxDistanceDelta,
				from.y + toVector_y / dist * maxDistanceDelta
			);
		}

		public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal)
		{
			var factor = -2f * Dot(inNormal, inDirection);
			return new Vector2(factor * inNormal.x + inDirection.x, factor * inNormal.y + inDirection.y);
		}

		public static Vector2 Perpendicular(Vector2 inDirection) => new Vector2(-inDirection.y, inDirection.x);

		public static float Dot(Vector2 left, Vector2 right) => left.x * right.x + left.y * right.y;

		public static float Angle(Vector2 from, Vector2 to)
		{
			var denominator = Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
			if (denominator < epsilonNormalSqrt)
				return 0.0f;

			var dot = Math.Clamp(Dot(from, to) / denominator, -1.0f, 1.0f);
			return Math.Acos(dot) * Math.rad2Deg;
		}

		public static float SignedAngle(Vector2 from, Vector2 to)
		{
			var unsignedAngle = Angle(from, to);
			var sign = Math.Sign(from.x * to.y - from.y * to.x);
			return unsignedAngle * sign;
		}

		public static bool DistanceET(Vector2 from, Vector2 to, float value) =>
			Math.Approximately(DistanceSqr(from, to), value * value);
		public static bool DistanceLT(Vector2 from, Vector2 to, float value) =>
			DistanceSqr(from, to) < value * value;
		public static bool DistanceGT(Vector2 from, Vector2 to, float value) =>
			DistanceSqr(from, to) > value * value;

		public static float Distance(Vector2 from, Vector2 to) => Math.Sqrt(DistanceSqr(from, to));

		public static float DistanceSqr(Vector2 from, Vector2 to) => (from - to).sqrMagnitude;

		public static Vector2 Clamp(Vector2 vector, float length) =>
			new Vector2(Math.Clamp(vector.x, -length, length), Math.Clamp(vector.y, -length, length));

		public static Vector2 Clamp(Vector2 vector, Vector2 size) =>
			new Vector2(Math.Clamp(vector.x, -size.x, size.x), Math.Clamp(vector.y, -size.y, size.y));

		public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
		{
			var sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude > maxLength * maxLength)
			{
				var mag = Math.Sqrt(sqrMagnitude);

				var normalized_x = vector.x / mag;
				var normalized_y = vector.y / mag;
				return new Vector2(normalized_x * maxLength, normalized_y * maxLength);
			}
			return vector;
		}

		public static Vector2 GetDirection(Vector2 start, Vector2 end) => (start - end).normalized;

		public static Vector2 Min(Vector2 a, Vector2 b) => new Vector2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));

		public static Vector2 Max(Vector2 a, Vector2 b) => new Vector2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
		#endregion

		#endregion

		#region Object

		#region Variables
		[JsonProperty] public float x;
		[JsonProperty] public float y;

		public float this[int index]
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
				var vector = new Vector2(x, y);
				vector.Normalize();
				return vector;
			}
		}
		public Vector2 sign { get => new Vector2(Math.Sign(x), Math.Sign(y)); }
		public Vector2 abs { get => new Vector2(x.Abs(), y.Abs()); }

		public Vector2 isolateX { get => new Vector2(x, 0.0f); }
		public Vector2 isolateY { get => new Vector2(0.0f, y); }

		public float sqrMagnitude { get => x * x + y * y; }
		public float magnitude { get => Math.Sqrt(sqrMagnitude); }

		public float max { get => Math.Max(x, y); }
		public float min { get => Math.Min(x, y); }

		public Vector2Int floored { get => this; }
		public Vector2Int rounded { get => new Vector2Int(Math.RoundToInt(x), Math.RoundToInt(y)); }
		public Vector2Int ceiled { get => new Vector2Int(Math.CeilToInt(x), Math.CeilToInt(y)); }
		#endregion

		#region Constructors
		public Vector2(float value)
		{
			this.x = value;
			this.y = value;
		}

		public Vector2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
		#endregion

		#region Methods
		public void Normalize()
		{
			var mag = magnitude;

			if (mag > 1.0f)
				if (mag > epsilon)
					this = this / mag;
				else
					this = zero;
		}

		public void Clamp(float max)
		{
			x = Math.Clamp(x, 0, max);
			y = Math.Clamp(y, 0, max);
		}

		public void Clamp(float min, float max)
		{
			x = Math.Clamp(x, min, max);
			y = Math.Clamp(y, min, max);
		}

		public void Clamp(float minX, float minY, float maxX, float maxY)
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

		public static Vector2 operator +(Vector2 left, float right) => new Vector2(left.x + right, left.y + right);
		public static Vector2 operator -(Vector2 left, float right) => new Vector2(left.x - right, left.y - right);
		public static Vector2 operator *(Vector2 left, float right) => new Vector2(left.x * right, left.y * right);
		public static Vector2 operator *(float left, Vector2 right) => new Vector2(right.x * left, right.y * left);
		public static Vector2 operator /(Vector2 left, float right) => new Vector2(left.x / right, left.y / right);

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