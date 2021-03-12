using System;
using System.Collections.Generic;
using MGE.InputSystem;
using MGE.UI;

namespace MGE.Debug
{
	public static class Menuing
	{
		const int menuMenuSize = 32;
		const int menuMenuItemOffset = 16;

		public static List<DebugMenu> menus = new List<DebugMenu>();

		static bool menuMenuOpen = false;

		static GUI gui;

		public static void Update()
		{
			gui = new GUI(new Rect(Vector2.zero, Window.windowedSize), Input.mouseIsInWindow);

			var moveMenuToTop = -1;

			for (int i = menus.Count - 1; i >= 0; i--)
			{
				menus[i].UpdateBG();

				var interaction = GUI.gui.MouseInteraction(new Rect(menus[i].position.x, menus[i].position.y - DebugMenu.barSize, menus[i].size.x, menus[i].size.y + DebugMenu.barSize));

				if (interaction.HasFlag(PointerInteraction.LClick) || interaction.HasFlag(PointerInteraction.RClick) || interaction.HasFlag(PointerInteraction.MClick))
					moveMenuToTop = i;
			}

			if (moveMenuToTop > -1)
			{
				var menu = menus[moveMenuToTop];

				menus.RemoveAt(moveMenuToTop);
				menus.Insert(0, menu);
			}

			if (menus.Count > 0)
				if (menus[0].gui != null)
					menus[0].Update();

			if (!menuMenuOpen)
			{
				if (Input.windowMousePosition.y < 1)
					menuMenuOpen = true;
			}
			else
			{
				if (Input.windowMousePosition.y > menuMenuSize)
					menuMenuOpen = false;
			}

			Terminal.enabled = !menuMenuOpen;

			if (menuMenuOpen)
			{
				gui.Image(new Rect(0, 0, Window.windowedSize.x, menuMenuSize), Colors.transBG);

				var offsetIncerment = (double)menuMenuItemOffset / 2;
				var yOffset = menuMenuSize / 2 - Config.defualtFont.charPaddingSize.y / 2;

				foreach (var menu in Config.availableMenus)
				{
					var text = $"[{menu.name}]";

					var width = Config.defualtFont.charPaddingSize.x * text.Length;

					gui.Text(text, new Vector2(offsetIncerment, yOffset), Colors.text);

					var rect = new Rect(offsetIncerment - (float)menuMenuItemOffset / 2, 0, width + menuMenuItemOffset, menuMenuSize);

					if (gui.MouseInside(rect))
					{
						gui.Image(rect, Colors.highlight);

						if (Input.GetButtonPress(Inputs.MouseLeft))
							OpenMenu((DebugMenu)Activator.CreateInstance(menu.GetType()));
					}

					offsetIncerment += width + menuMenuItemOffset;
				}
			}
		}

		public static void Draw()
		{
			for (int i = menus.Count - 1; i >= 0; i--)
				menus[i].Draw();

			gui.Draw();
		}

		public static void OpenMenu(DebugMenu menu, Vector2? position = null)
		{
			if (!position.HasValue)
				position = new Vector2(DebugMenu.allSize, DebugMenu.allSize * 2);

			menu.position = position.Value;

			menu.Init();
			menus.Insert(0, menu);
		}

		public static void CloseMenu(DebugMenu menu)
		{
			menus.Remove(menu);
		}
	}
}