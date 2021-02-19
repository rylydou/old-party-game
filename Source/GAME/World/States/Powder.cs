using GAME.Components;
using MGE;

namespace GAME.World
{
	public abstract class Powder : Solid
	{
		protected override void OnUpdate(Vector2Int position)
		{
			if (!grid.MoveTile(position, position + new Vector2Int(0, 1)))
			{
				if (!grid.MoveTile(position, position + new Vector2Int(-1, 1)))
				{
					grid.MoveTile(position, position + new Vector2Int(1, 1));
				}
			}
		}
	}
}