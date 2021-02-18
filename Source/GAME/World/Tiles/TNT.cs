using MGE;
using GAME.Components;

namespace GAME.World
{
	public class TNT : ITile
	{
		public string name => "TNT";
		public Color color => Color.red;

		public void Update(Vector2Int position)
		{
			double chance = 0;

			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					if (x == 0 && y == 0) continue;

					if (CWorld.grid.GetTileID(position.x + x, position.y + y) == 0)
						chance += 8.0 / 1.0 / 800.0;
				}
			}

			if (Random.Bool(chance))
			{
				for (int y = -25; y <= 25; y++)
				{
					for (int x = -25; x <= 25; x++)
					{
						if (CWorld.grid.GetTileID(position.x + x, position.y + y) == Grid.TileToID(typeof(TNT)))
						{
							for (int yy = -25; yy <= 25; y++)
								for (int xx = -25; xx <= 25; x++)
									CWorld.grid.SetTileID(position.x + x + xx, position.y + y + xx, 0);
						}
						CWorld.grid.SetTileID(position.x + x, position.y + y, 0);
					}
				}
			}
		}
	}
}