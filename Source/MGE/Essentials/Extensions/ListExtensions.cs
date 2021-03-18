using System.Collections.Generic;

namespace MGE
{
	static class ListExtensions
	{
		public static void AddToFront<T>(this List<T> list, T item) => list.Insert(0, item);

		public static void Move<T>(this List<T> list, int from, int to)
		{
			var item = list[from];
			list.RemoveAt(from);
			list.Insert(to > from ? to : to, item);
		}

		public static T Random<T>(this List<T> list) => list[MGE.Random.Int(0, list.Count - 1)];
		public static T Random<T>(this List<T> list, int[] weights) => MGE.Random.WeigthedOdds<T>(list, weights);
		public static T Random<T>(this List<T> list, IList<int> weights) => MGE.Random.WeigthedOdds<T>(list, weights);
	}
}