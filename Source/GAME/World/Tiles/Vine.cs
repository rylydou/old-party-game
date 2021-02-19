using MGE;

namespace GAME.World
{
	public class Vine : Tile
	{
		public override string name => "Vine";
		public override Color color => new Color(0.33f, 0.67f, 0.33f);
		public override int density => 10;

		int wetness = 0;

		protected override void OnUpdate(Vector2Int position)
		{
			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					if (grid.TileIsType(position.x + x, position.y + y, typeof(Water)))
					{
						grid.SetTile(position.x + x, position.y + y, null);
						wetness++;
						if (wetness > 9)
						{
							wetness = 0;
							for (int yy = -1; yy <= 1; yy++)
								for (int xx = -1; xx <= 1; xx++)
									if (Random.Bool(1.0 / 9.0)) grid.SetTile(position.x + x + xx, position.y + y + yy, new Vine());
						}
					}
					else if (grid.TileIsType(position.x + x, position.y + y, typeof(Salt)))
					{
						if (Random.Bool()) grid.SetTile(position.x + x, position.y + y, null);
						grid.SetTile(position.x + x, position.y + y, null);
					}
				}
			}
		}
	}
}