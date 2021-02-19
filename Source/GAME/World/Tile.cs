using MGE;

namespace GAME.World
{
	public abstract class Tile
	{
		public abstract string name { get; }
		public abstract Color color { get; }
		public abstract int density { get; }

		public abstract void Update(Vector2Int position);
	}
}