using System.Linq;

namespace MGE
{
	public class Args
	{
		public static string[] args;

		public static bool HasFlag(string flag) => args.Contains(flag);
	}
}