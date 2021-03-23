using System.Collections.Generic;

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

		public static string CleanRound(float fps)
		{
			if (fps > 1.0f)
				return Math.Round(fps).ToString();
			else
				return (Math.Round(fps * 100.0f) / 100.0f).ToString();
		}

		public static string RemoveBadChars(string text)
		{
			foreach (var c in badChars)
				text = text.Replace(c.ToString(), "");

			return text;
		}

		public static Vector2Int[] LineToPointsInGrid(Vector2 start, Vector2 end)
		{
			var points = new List<Vector2Int>();

			if ((Vector2Int)start == (Vector2Int)end)
				return new Vector2Int[] { end };

			var t = start;
			var frac = 1 / Math.Sqrt(Math.Pow(end.x - start.x, 2) + Math.Pow(end.y - start.y, 2));
			var ctr = 0.0f;

			while ((int)t.x != (int)end.x || (int)t.y != (int)end.y)
			{
				t = Vector2.Lerp(start, end, ctr);
				ctr += frac;
				points.Add(t);
			}

			return points.ToArray();
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