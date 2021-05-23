using System;
using System.Collections.Generic;
using GAME.Components.Items;
using GAME.Tiles;
using MGE;

namespace GAME
{
	public static class Setup
	{
		public static Type[] items = new Type[]
		{
			typeof(CBananaGun),
			typeof(CBananaPeel),
			typeof(CBoomerang),
			typeof(CCrate),
			typeof(CCursedPearl),
			typeof(CPearl),
			typeof(CFishingRod),
			typeof(CGrenadeLauncher),
			typeof(CHamburger),
			typeof(CMinigun),
			typeof(CPlungerGun),
			typeof(CRifle),
			typeof(CRock),
			typeof(CShotgun),
		};

		public static Type[] baseCrateLootTable = new Type[]
		{
			typeof(CBananaGun),
			typeof(CBoomerang),
			typeof(CFishingRod),
			typeof(CGrenadeLauncher),
			typeof(CHamburger),
			// typeof(CMinigun),
			typeof(CPlungerGun),
			typeof(CRifle),
			typeof(CRock),
			typeof(CShotgun),
		};

		public static List<string> skins = new List<string>
		{
			"_Default",
			"Amogus",
			"Chicken",
			"Goose",
			"Robot",
		};

		static Tileset[] _backgroundTilesets;
		public static Tileset[] backgroundTilesets
		{
			get
			{
				if (_backgroundTilesets is null)
					_backgroundTilesets = new Tileset[]
					{
						null,
					};

				return _backgroundTilesets;
			}
		}

		static (Tile, Tileset)[] _tilesets;
		public static (Tile, Tileset)[] tilesets
		{
			get
			{
				if (_tilesets is null)
					_tilesets = new (Tile, Tileset)[]
					{
						(new Air(), null),
						(new Solid(), Assets.GetAsset<Tileset>("Tilesets/Basic")),
						(new Solid(), Assets.GetAsset<Tileset>("Tilesets/Grass")),
						(new Solid(), Assets.GetAsset<Tileset>("Tilesets/Stone")),
						(new Lava(), Assets.GetAsset<Tileset>("Tilesets/Lava")),
						(new Solid(), Assets.GetAsset<Tileset>("Tilesets/Sand")),
						(new Semisolid(), Assets.GetAsset<Tileset>("Tilesets/Semisolid")),
					};

				return _tilesets;
			}
		}

		public static Palette[] palettes = new Palette[]
		{
			new Palette("#00296b", "#003f88"),
			new Palette("#619b8a", "#a1c181"),
			new Palette("#ff7f51", "#ff9b54"),
			new Palette("#4f000b", "#720026"),
			new Palette("#22577a", "#38a3a5"),
			new Palette("#ff7900", "#ff9100"),
			new Palette("#d0b8ac", "#f3d8c7"),
			new Palette("#ee6055", "#ff9b85"),
			new Palette("#3c096c", "#5a189a"),
			new Palette("#004e98", "#3a6ea5"),
			new Palette("#003554", "#006494"),
			new Palette("#ddbea8", "#f3dfc1"),
			new Palette("#895737", "#c08552"),
			new Palette("#1f2041", "#4b3f72"),
			new Palette("#718355", "#87986a"),
			new Palette("#666a86", "#788aa3"),
			new Palette("#f76c5e", "#f68e5f"),
			new Palette("#a9bcd0", "#d8dbe2"),
			new Palette("#264653", "#2a9d8f"),
			new Palette("#fb8500", "#ffb703"),
			new Palette("#065a60", "#006466"),
			new Palette("#aaaaaa", "#bbbbbb"),
		};
	}
}