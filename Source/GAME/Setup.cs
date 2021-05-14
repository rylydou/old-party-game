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
			typeof(CCrate),
			typeof(CCursedPearl),
			typeof(CPearl),
			typeof(CFishingRod),
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
			typeof(CFishingRod),
			typeof(CHamburger),
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
	}
}