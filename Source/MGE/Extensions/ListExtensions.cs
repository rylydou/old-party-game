using System.Collections.Generic;

namespace MGE
{
	static class ListExtensions
	{
		public static SafeList<T> ToSafeList<T>(this List<T> list)
		{
			return new SafeList<T>(list);
		}
	}
}