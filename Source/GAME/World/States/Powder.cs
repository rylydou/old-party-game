using GAME.Components;
using MGE;

namespace GAME.World
{
	public abstract class Powder : Solid
	{
		public override void Update(Vector2Int position)
		{
			if (!CWorld.grid.MoveTile(position, position + new Vector2Int(0, 1)))
			{
				if (!CWorld.grid.MoveTile(position, position + new Vector2Int(-1, 1)))
				{
					CWorld.grid.MoveTile(position, position + new Vector2Int(1, 1));
				}
			}
		}
	}
}