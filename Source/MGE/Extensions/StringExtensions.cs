using System;

namespace MGE
{
	static class StringExtensions
	{
		public static int WordCount(this string str)
		{
			return str.Split("`~!@#$%^&*()=+[{]}\\|;:'\",.<>/? _".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
		}
	}
}