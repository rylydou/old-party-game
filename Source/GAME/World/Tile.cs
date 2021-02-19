using MGE;

namespace GAME.World
{
	public abstract class Tile
	{
		protected Grid grid { get => Grid.current; }

		public abstract string name { get; }
		public abstract Color color { get; }
		public abstract int density { get; }

		public void Update(Vector2Int position)
		{
			OnUpdate(position);
		}

		protected abstract void OnUpdate(Vector2Int position);
	}
}