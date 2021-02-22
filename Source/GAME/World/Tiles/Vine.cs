using MGE;

namespace GAME.World
{
	public class Vine : Wall
	{
		const ushort wetnessToGrow = 10;

		public override string name => "Vine";
		public override Color color => Color.Lerp(new Color(0.33f, 0.67f, 0.33f), new Color(0.2f, 0.8f, 0.2f), (float)wetness / 9);
		public override short density => 10;
		public override TileInfo info => TileInfo.None;

		ushort wetness = 1;

		public override void Update(Vector2Int position)
		{
			if (wetness < 1)
				grid.SetTile(position.x, position.y, null);

			var surroundedTiles = 0;

			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					if (x == 0 && y == 0) continue;

					if (grid.TileIsType(position.x + x, position.y + y, typeof(Water)))
					{
						grid.SetTile(position.x + x, position.y + y, null);
						wetness++;

						if (wetness > wetnessToGrow)
						{
							wetness = 1;

							grid.SetTile(position.x + x, position.y + y, new Vine());

							for (int yy = -1; yy <= 1; yy++)
								for (int xx = -1; xx <= 1; xx++)
									if (Random.Bool(1.0 / 9.0)) grid.SetTile(position.x + x + xx, position.y + y + yy, new Vine());
						}
					}
					else if (grid.TileExists(position.x + x, position.y + y, typeof(Vine)))
						surroundedTiles++;
				}
			}

			if (surroundedTiles > 4)
				grid.SetTile(position, new Oil());
		}
	}
}