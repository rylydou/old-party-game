using System.Collections.Generic;

namespace GAME.UI
{
	public static class MenuManager
	{
		public static List<Menu> menus = new List<Menu>();

		public static void OpenMenu(Menu menu)
		{
			menus.Add(menu);
		}
	}
}