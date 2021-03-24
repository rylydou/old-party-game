using System;
using System.Text;

namespace MGE
{
	static class StringExtensions
	{
		public static int WordCount(this string str) =>
			str.Split("`~!@#$%^&*()=+[{]}\\|;:'\",.<>/? _".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;

		public static StringBuilder GetBuilder(this string str) =>
			new StringBuilder(str);

		public static StringBuilder GetBuilder(this string str, int capacity) =>
			new StringBuilder(str, capacity);

		public static StringBuilder GetBuilder(this string str, int start, int length, int capacity) =>
			new StringBuilder(str, start, length, capacity);
	}
}