using System;

namespace MGE
{
	public struct Vector2Int
	{
		#region Static

		#region Constants
		static readonly Vector2Int _zero = new Vector2Int(0, 0);
		public static Vector2Int zero { get { return _zero; } }
		static readonly Vector2Int _one = new Vector2Int(1, 1);
		public static Vector2Int one { get { return _one; } }
		static readonly Vector2Int _up = new Vector2Int(0, 1);
		public static Vector2Int up { get { return _up; } }
		static readonly Vector2Int _down = new Vector2Int(0, -1);
		public static Vector2Int down { get { return _down; } }
		static readonly Vector2Int _left = new Vector2Int(-1, 0);
		public static Vector2Int left { get { return _left; } }
		static readonly Vector2Int _right = new Vector2Int(1, 0);
		public static Vector2Int right { get { return _right; } }
		static readonly Vector2Int _positiveInfinity = new Vector2Int(int.MaxValue, int.MaxValue);
		public static Vector2Int positiveInfinity { get { return _positiveInfinity; } }
		static readonly Vector2Int _negativeInfinity = new Vector2Int(int.MinValue, int.MinValue);
		public static Vector2Int negativeInfinity { get { return _negativeInfinity; } }

		#endregion

		#region Methods
		public static Vector2Int Scale(Vector2Int left, Vector2Int right) => new Vector2Int(left.x * right.x, left.y * right.y);

		public static Vector2Int Perpendicular(Vector2Int inDirection) => new Vector2Int(-inDirection.y, inDirection.x);

		public static int Dot(Vector2Int left, Vector2Int right) => left.x * right.x + left.y * right.y;

		public static int Distance(Vector2Int from, Vector2Int to)
		{
			int diff_x = from.x - to.x;
			int diff_y = from.y - to.y;
			return (int)Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
		}

		public static Vector2Int Min(Vector2Int a, Vector2Int b) => new Vector2Int(Math.Min(a.x, b.x), Math.Min(a.y, b.y));

		public static Vector2Int Max(Vector2Int a, Vector2Int b) => new Vector2Int(Math.Max(a.x, b.x), Math.Max(a.y, b.y));

		#endregion

		#endregion

		#region Object

		#region Variables
		public int x;
		public int y;

		public int this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return x;
					case 1: return y;
					default: throw new IndexOutOfRangeException($"Invalid Vector2Int index of {index}!");
				}
			}

			set
			{
				switch (index)
				{
					case 0: x = value; break;
					case 1: y = value; break;
					default: throw new IndexOutOfRangeException($"Invalid Vector2Int index of {index}!");
				}
			}
		}
		#endregion

		#region Properties
		public int sqrMagnitude { get => x * x + y * y; }
		public double magnitude { get => Math.Sqrt(sqrMagnitude); }

		public int max { get => Math.Max(x, y); }
		public int min { get => Math.Min(x, y); }
		#endregion

		#region Constructors
		public Vector2Int(int value)
		{
			this.x = value;
			this.y = value;
		}

		public Vector2Int(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		#endregion

		#region Methods
		public void Clamp(int max)
		{
			x = Math.Clamp(x, 0, max);
			y = Math.Clamp(y, 0, max);
		}

		public void Clamp(int min, int max)
		{
			x = Math.Clamp(x, min, max);
			y = Math.Clamp(y, min, max);
		}

		public void Clamp(int minX, int minY, int maxX, int maxY)
		{
			x = Math.Clamp(x, minX, maxX);
			y = Math.Clamp(y, minY, maxY);
		}
		#endregion

		#region Implicit
		public static Vector2Int operator +(Vector2Int left, Vector2Int right) => new Vector2Int(left.x + right.x, left.y + right.y);
		public static Vector2Int operator -(Vector2Int left, Vector2Int right) => new Vector2Int(left.x - right.x, left.y - right.y);
		public static Vector2Int operator *(Vector2Int left, Vector2Int right) => new Vector2Int(left.x * right.x, left.y * right.y);
		public static Vector2Int operator /(Vector2Int left, Vector2Int right) => new Vector2Int(left.x / right.x, left.y / right.y);

		public static Vector2Int operator -(Vector2Int vector) => new Vector2Int(-vector.x, -vector.y);

		public static Vector2Int operator *(Vector2Int left, int right) => new Vector2Int(left.x * right, left.y * right);
		public static Vector2Int operator *(int left, Vector2Int right) => new Vector2Int(right.x * left, right.y * left);
		public static Vector2Int operator /(Vector2Int left, int right) => new Vector2Int(left.x / right, left.y / right);

		public static bool operator ==(Vector2Int left, Vector2Int right) => left.x == right.x && left.y == right.y;
		public static bool operator !=(Vector2Int lhs, Vector2Int rhs) => !(lhs == rhs);

		public static implicit operator bool(Vector2Int vector) => vector != Vector2Int.zero;

		public static implicit operator Microsoft.Xna.Framework.Vector2(Vector2Int vector) => new Microsoft.Xna.Framework.Vector2(vector.x, vector.y);
		public static implicit operator Vector2Int(Microsoft.Xna.Framework.Vector2 vector) => new Vector2Int((int)vector.X, (int)vector.Y);
		public static implicit operator Vector2Int(Microsoft.Xna.Framework.Point point) => new Vector2Int(point.X, point.Y);
		public static implicit operator Microsoft.Xna.Framework.Point(Vector2Int point) => new Microsoft.Xna.Framework.Point(point.x, point.y);
		#endregion

		#region Inherited
		public override string ToString() => $"({x}, {y})";

		public string ToString(string format) => string.Format(format, x, y);

		public override int GetHashCode() => (x.GetHashCode() >> 2) ^ (y.GetHashCode() << 2);

		public bool Equals(Vector2Int other) => x == other.x && y == other.y;

		public override bool Equals(object other)
		{
			if (!(other is Vector2Int)) return false;

			return Equals((Vector2Int)other);
		}
		#endregion

		#endregion
	}
}