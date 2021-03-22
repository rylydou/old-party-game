using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MGE.UI
{
	public struct TextFeildRule
	{
		public static readonly TextFeildRule all = new TextFeildRule(new Regex(".*"));
		public static readonly TextFeildRule basicText = new TextFeildRule(new Regex("[ 0-9a-zA-Z!@#$%^&*()\\[\\]{}<>,\\.;:'\"]*"));
		public static readonly TextFeildRule colorCode = new TextFeildRule(new Regex("^#?([0-f]|[0-f]{1}|[[0-f]{3}|[0-f]{4}|[[0-f]{6}|[0-f]{8})$"));
		public static readonly TextFeildRule colorCodeNoAlpha = new TextFeildRule(new Regex("^#?([0-f]|[0-f]{1}|[[0-f]{3}|[[0-f]{6})$"));

		public Regex regex;

		public TextFeildRule(Regex regex)
		{
			this.regex = regex;
		}

		public bool IsValid(string text)
		{
			return regex.IsMatch(text);
		}
	}
}