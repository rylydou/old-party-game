using System;
using System.Collections.Generic;

namespace MGE
{
	public struct Polygon : IEquatable<Polygon>
	{
		public List<Vector2> points;

		public Polygon(List<Vector2> points)
		{
			this.points = points;
		}

		public static bool operator ==(Polygon left, Polygon right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Polygon left, Polygon right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			return obj is Polygon polygon && Equals(polygon);
		}

		public bool Equals(Polygon other)
		{
			return EqualityComparer<List<Vector2>>.Default.Equals(points, other.points);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(points);
		}
	}
}