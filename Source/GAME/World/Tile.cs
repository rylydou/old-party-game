using MGE;

namespace GAME.World
{
	public abstract class Tile
	{
		protected Grid grid { get => Grid.current; }

		public abstract string name { get; }
		public abstract Color color { get; }
		public abstract short density { get; }
		public abstract TileType type { get; }
		public abstract TileInfo info { get; }

		public bool isDirty = true;
		public uint lastUpdated = 0;
		// public Vector2Int velocity = Vector2Int.zero;

		public abstract void Update(Vector2Int position);
	}
}