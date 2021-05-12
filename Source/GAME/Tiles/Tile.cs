using GAME.Components;
using MGE;

namespace GAME
{
	public abstract class Tile
	{
		public abstract bool IsSolid(Vector2Int tilePos, Vector2 origin, Vector2 direction);
		public virtual void ObjectInside(CObject obj) { }
	}
}