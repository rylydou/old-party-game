using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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
				if (args == null || args.Length < 1 || args.Contains("--hide-exception"))
					game.Run();
				else
				{
					try
					{
						game.Run();
					}
					catch (Exception e)
					{
						try
						{
							if (Engine.game != null) Engine.game.IsMouseVisible = true;
							using (StreamWriter streamWriter = new StreamWriter("crash.log", true))
							{
								streamWriter.WriteLine((object)DateTime.Now);
								streamWriter.WriteLine((object)e);
								streamWriter.WriteLine("");
							}
							int num = (int)MessageBox.Show(e.ToString(), "Mangrove Game Engine: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, false);
						}
						catch
						{
							Console.WriteLine("uhh oh...");
						}
					}
				}
			}
		}
	}
}