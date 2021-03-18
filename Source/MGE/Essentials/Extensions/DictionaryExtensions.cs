using System.Collections.Generic;

namespace MGE
{
	static class DictionaryExtensions
	{
		public static TValue TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defualtValue)
		{
			var value = defualtValue;

			dict.TryGetValue(key, out value);

			return value;
		}
	}
}