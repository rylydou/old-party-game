using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MGE.UI
{
	public struct TextFeildRule
	{
		public static readonly TextFeildRule all = new TextFeildRule(new Regex(".*"));
		public static readonly TextFeildRule basicText = new TextFeildRule(new Regex("[ 0-9a-zA-Z!@#$%^&*()\\[\\]{}<>,\\.;:'\"]*"));
		public static readonly TextFeildRule colorCode = new TextFeildRule(new Regex("#([0-F]|[0-F]{3}|[0-F]{6}|[0-F]{8})"));

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