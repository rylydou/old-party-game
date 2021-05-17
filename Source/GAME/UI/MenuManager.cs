using System;
using System.Collections.Generic;
using System.Linq;

namespace GAME.UI
{
	public static class MenuManager
	{
		public static List<Menu> menus = new List<Menu>();

		public static Action<Menu.Option> onOptionSelect = (o) => { };
		public static Action<Menu.Option> onOptionChange = (o) => { };
		public static Action<Menu.Option> onOptionError = (o) => { };
		public static Action<Menu> onMenuOpen = (m) => { };
		public static Action onMenuClose = () => { };

		public static void OpenMenu(Menu menu)
		{
			menus.Add(menu);
		}

		public static void GoBack()
		{
			if (menus.Count > 0)
				menus.RemoveAt(menus.Count - 1);
		}

		public static void Update()
		{
			if (menus.Count > 0)
				menus.Last().Update();
		}

		public static void Draw()
		{
			if (menus.Count > 0)
				menus.Last().Draw();
		}
	}
}