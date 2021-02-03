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

			using (var game = new Game()) game.Run();
		}
	}
}