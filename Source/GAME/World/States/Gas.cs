using MGE;
using GAME.Components;

namespace GAME.World
{
	public abstract class Gas : Tile
	{
		protected override void OnUpdate(Vector2Int position)
		{
			if (!grid.MoveTile(position, position + new Vector2Int(0, -1)))
			{
				if (!grid.MoveTile(position, position + new Vector2Int(-1, -1)))
				{
					if (!grid.MoveTile(position, position + new Vector2Int(1, -1)))
					{
						if (!grid.MoveTile(position, position + new Vector2Int(-1, 0)))
						{
							grid.MoveTile(position, position + new Vector2Int(1, 0));
						}
					}
				}
			}
		}
	}
}