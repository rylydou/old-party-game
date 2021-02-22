using MGE;

namespace GAME.World
{
	public abstract class Gravity : Tile
	{
		public override void Update(Vector2Int position)
		{
			grid.SwapTile(position, position + new Vector2Int(0, 1));
		}
	}
}