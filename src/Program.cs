using System;

namespace MGE
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

			using (var game = new Main()) game.Run();
		}
	}
}
