using System;
using System.Collections.Generic;
using System.Linq;

namespace MGE
{
	public struct Util
	{
		public static string CleanRound(double fps)
		{
			if (fps > 1.0)
				return Math.Round(fps).ToString();
			else
				return (Math.Round(fps * 100.0) / 100.0).ToString();
		}

		public static string RemoveBadChars(string text)
		{
			text = text.Replace("\0", "");
			text = text.Replace("", "");
			text = text.Replace("\a", "");
			text = text.Replace("\b", "");
			text = text.Replace("\f", "");
			text = text.Replace("\n", "");
			text = text.Replace("\r", "");
			text = text.Replace("\t", "");
			text = text.Replace("\v", "");

			return text;
		}
	}
}