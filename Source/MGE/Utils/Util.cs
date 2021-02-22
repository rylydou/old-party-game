using System;
using System.Collections.Generic;
using System.Linq;

namespace MGE
{
	public static class Util
	{
		public const int badCharsAmount = 32;

		static char[] _badChars;
		public static char[] badChars
		{
			get
			{
				if (_badChars == null)
				{
					_badChars = new char[badCharsAmount];

					for (int i = 0; i < badCharsAmount; i++)
						badChars[i] = (char)i;
				}

				return _badChars;
			}
		}

		public static string CleanRound(double fps)
		{
			if (fps > 1.0)
				return Math.Round(fps).ToString();
			else
				return (Math.Round(fps * 100.0) / 100.0).ToString();
		}

		public static string RemoveBadChars(string text)
		{
			foreach (var c in badChars)
				text = text.Replace(c.ToString(), "");

			return text;
		}

		public static List<Vector2Int> LineToPointsInGrid(Vector2 start, Vector2 end)
		{
			var points = new List<Vector2Int>();

			Vector2 t = start;
			var frac = 1 / Math.Sqrt(Math.Pow(end.x - start.x, 2) + Math.Pow(end.y - start.y, 2));
			var ctr = 0.0;

			while ((int)t.x != (int)end.x || (int)t.y != (int)end.y)
			{
				t = Vector2.Lerp(start, end, ctr);
				ctr += frac;
				points.Add(t);
			}

			return points;
		}

		public static int GetHashCode(object obj)
		{
			if (obj is string s)
				return GetHashCode(s.ToCharArray());
			return obj.ToString().GetHashCode() ^ obj.GetHashCode();
		}

		public static int GetHashCode(ICollection<object> objs)
		{
			var hashCode = objs.GetHashCode();

			foreach (var obj in objs)
				hashCode ^= obj.ToString().GetHashCode() ^ obj.GetHashCode();

			return hashCode;
		}
	}
}