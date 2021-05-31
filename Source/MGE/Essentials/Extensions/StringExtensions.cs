using System;
using System.Text;
using System.Text.RegularExpressions;

namespace MGE
{
	static class StringExtensions
	{
		public static int WordCount(this string str)
		{
			return str.Split("`~!@#$%^&*()=+[{]}\\|;:'\",.<>/? _".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
		}

		public static string ScentenceToCamel(this string str)
		{
			return str.Replace(" ", string.Empty);
		}

		public static string CamelToScentence(this string str)
		{
			return Regex.Replace(str, @"\p{Lu}", c => " " + c.Value.ToUpperInvariant());
		}

		public static StringBuilder GetBuilder(this string str) =>
			new StringBuilder(str);

		public static StringBuilder GetBuilder(this string str, int capacity) =>
			new StringBuilder(str, capacity);

		public static StringBuilder GetBuilder(this string str, int start, int length, int capacity) =>
			new StringBuilder(str, start, length, capacity);
	}
}