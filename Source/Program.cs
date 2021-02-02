using System;
using System.Linq;
using MGE;

namespace GAME
{
	public static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			Logger.Log("Loading game...");

			if (args.Length != 0)
				Logger.Log($"Args: {string.Join(", ", args)}");
			else
				Logger.Log("No Args");

			Args.args = args.ToList();

			using (var game = new Game()) game.Run();
		}
	}
}