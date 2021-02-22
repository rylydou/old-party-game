using MGE;
using GAME.Components;

namespace GAME.World
{
	public abstract class Gas : Tile
	{
		public override TileType type => TileType.Gas;

		public override void Update(Vector2Int position)
		{
			if (!grid.SwapTile(position, position + new Vector2Int(0, -1)))
			{
				if (!grid.SwapTile(position, position + new Vector2Int(1, -1)))
				{
					if (!grid.SwapTile(position, position + new Vector2Int(-1, -1)))
					{
						if (!grid.SwapTile(position, position + new Vector2Int(1, 0)))
						{
							grid.SwapTile(position, position + new Vector2Int(-1, 0));
						}
					}
				}
			}
		}
	}
}