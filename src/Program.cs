using System;

namespace MGE
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			Logger.Log("Loading game...");
			using (var game = new Main()) game.Run();
		}
	}
}
