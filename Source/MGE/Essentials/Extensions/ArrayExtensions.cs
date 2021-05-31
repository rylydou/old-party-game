using System.Collections.Generic;

namespace MGE
{
	static class ArrayExtensions
	{
		public static T Random<T>(this T[] array) => array.Length > 0 ? array[MGE.Random.Int(0, array.Length - 1)] : default;
		public static T Random<T>(this T[] array, int[] weights) => array.Length > 0 ? MGE.Random.WeigthedOdds<T>(array, weights) : default;
		public static T Random<T>(this T[] array, IList<int> weights) => array.Length > 0 ? MGE.Random.WeigthedOdds<T>(array, weights) : default;
	}
}