using System;
using MGE;

namespace GAME
{
	public static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			Logger.Log("Loading game...");

			using (Main game = new Main())
			{
				try
				{
					game.Run();
				}
				catch (Exception e)
				{
					if (Engine.game is object) Engine.game.IsMouseVisible = true;

					Logger.Log("Crash!");
					Logger.Log(e);

#if !INDEV
										using (StreamWriter streamWriter = new StreamWriter($"{now} crash.log", true))
										{
											streamWriter.WriteLine("--- Crash! ---");
											streamWriter.WriteLine(e);
											streamWriter.WriteLine("");
										}
										int num = (int)MessageBox.Show(e.ToString(), "Mangrove Game Engine - Crash", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false);
#endif
				}
			}
		}
	}
}