using MGE;
using GAME.Components;

namespace GAME.World
{
	public abstract class Gas : Tile
	{
		public override void Update(Vector2Int position)
		{
			if (!CWorld.grid.MoveTile(position, position + new Vector2Int(0, -1)))
			{
				if (!CWorld.grid.MoveTile(position, position + new Vector2Int(-1, -1)))
				{
					if (!CWorld.grid.MoveTile(position, position + new Vector2Int(1, -1)))
					{
						if (!CWorld.grid.MoveTile(position, position + new Vector2Int(-1, 0)))
						{
							CWorld.grid.MoveTile(position, position + new Vector2Int(1, 0));
						}
					}
				}
			}
		}
	}
}