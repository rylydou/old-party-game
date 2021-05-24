using System;
using System.Collections.Generic;
using System.Linq;
using MGE;
using MGE.Graphics;

namespace GAME.UI
{
	public static class MenuManager
	{
		public static List<Menu> menus = new List<Menu>();

		public static Action<Menu.Option> onOptionSelect = (o) => { };
		public static Action<Menu.Option> onOptionChange = (o) => { };
		public static Action<Menu.Option> onOptionError = (o) => { };
		public static Action<Menu> onMenuOpen = (m) => { };
		public static Action<Menu> onMenuClose = (m) => { };

		static bool inited = false;

		static Texture gradHor;
		static Sound menuSelect;
		static Sound menuChange;
		static Sound menuError;
		static Sound menuOpen;
		static Sound menuClose;

		public static void Init()
		{
			if (inited) return;
			inited = true;

			gradHor = Assets.GetAsset<Texture>("UI/Gradient Horizontal");
			menuSelect = Assets.GetAsset<Sound>("UI/Sounds/Menu Select");
			menuChange = Assets.GetAsset<Sound>("UI/Sounds/Menu Change");
			menuError = Assets.GetAsset<Sound>("UI/Sounds/Menu Error");
			menuOpen = Assets.GetAsset<Sound>("UI/Sounds/Menu Open");
			menuClose = Assets.GetAsset<Sound>("UI/Sounds/Menu Close");

			onOptionSelect += (o) => menuSelect?.Play();
			onOptionChange += (o) => menuChange?.Play();
			onOptionError += (o) => menuError?.Play();
			onMenuOpen += (m) => menuOpen?.Play();
			onMenuClose += (m) => menuClose?.Play();
		}

		public static void OpenMenu(Menu menu)
		{
			menu.cursorPosition = 0;
			menus.Add(menu);
			onMenuOpen.Invoke(menu);
		}

		public static void GoBack()
		{
			if (menus.Count > 0)
			{
				onMenuClose.Invoke(menus.Last());
				menus.RemoveAt(menus.Count - 1);
			}
		}

		public static void Update()
		{
			if (menus.Count > 0)
				menus.Last().Update();
		}

		public static void Draw()
		{
			GFX.Draw(gradHor, new Rect(0, 0, Window.renderSize.x / 2, Window.renderSize.y), new Color(0, 0.25f));

			if (menus.Count > 0)
				menus.Last().Draw();
		}
	}
}