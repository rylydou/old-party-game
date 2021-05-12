using MGE;

namespace GAME.Tiles
{
	public class Solid : Tile
	{
		public override bool IsSolid(Vector2Int tilePos, Vector2 origin, Vector2 direction)
		{
			return true;
		}
	}
}