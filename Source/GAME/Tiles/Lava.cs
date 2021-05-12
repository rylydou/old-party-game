using GAME.Components;
using MGE;

namespace GAME.Tiles
{
	public class Lava : Tile
	{
		public override bool IsSolid(Vector2Int tilePos, Vector2 origin, Vector2 direction)
		{
			return false;
		}

		public override void ObjectInside(CObject obj)
		{
			obj.Damage(5, Vector2.zero, null);
		}
	}
}