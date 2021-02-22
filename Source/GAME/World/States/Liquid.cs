using GAME.Components;
using MGE;

namespace GAME.World
{
	public abstract class Liquid : Tile
	{
		public override TileType type => TileType.Liquid;

		public override void Update(Vector2Int position)
		{
			if (!grid.SwapTile(position, position + new Vector2Int(0, 1)))
			{
				if (!grid.SwapTile(position, position + new Vector2Int(-1, 1)))
				{
					if (!grid.SwapTile(position, position + new Vector2Int(1, 1)))
					{
						if (!grid.SwapTile(position, position + new Vector2Int(-1, 0)))
						{
							grid.SwapTile(position, position + new Vector2Int(1, 0));
						}
					}
				}
			}
		}
	}
}