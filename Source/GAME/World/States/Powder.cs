using GAME.Components;
using MGE;

namespace GAME.World
{
	public abstract class Powder : Solid
	{
		public override void Update(Vector2Int position)
		{
			// if (!grid.TileExists(position.x, position.y + 1))
			// 	velocity.y = 1;
			// else if (!grid.TileExists(position.x - 1, position.y + 1))
			// 	velocity.x = -1;
			// else if (!grid.TileExists(position.x + 1, position.y + 1))
			// 	velocity.x = 1;

			// if (grid.MoveTile(position, velocity) == position)
			// {
			// 	velocity = Vector2.zero;
			// }

			if (!grid.SwapTile(position, position + new Vector2Int(0, 1)))
			{
				if (!grid.SwapTile(position, position + new Vector2Int(-1, 1)))
				{
					grid.SwapTile(position, position + new Vector2Int(1, 1));
				}
			}
		}
	}
}