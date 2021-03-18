using System.Collections.Generic;

namespace MGE
{
	static class ArrayExtensions
	{
		public static T Random<T>(this T[] array) => array[MGE.Random.Int(0, array.Length - 1)];
		public static T Random<T>(this T[] array, int[] weights) => MGE.Random.WeigthedOdds<T>(array, weights);
		public static T Random<T>(this T[] array, IList<int> weights) => MGE.Random.WeigthedOdds<T>(array, weights);
	}
}