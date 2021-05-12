using MGE;

namespace GAME.Tiles
{
	public class Air : Tile
	{
		public override bool IsSolid(Vector2Int tilePos, Vector2 origin, Vector2 direction)
		{
			return false;
		}
	}
}