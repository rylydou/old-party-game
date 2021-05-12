using MGE;

namespace GAME.Tiles
{
	public class Semisolid : Tile
	{
		public override bool IsSolid(Vector2Int tilePos, Vector2 origin, Vector2 direction)
		{
			return origin.y <= tilePos.y && direction.y > 0;
		}
	}
}