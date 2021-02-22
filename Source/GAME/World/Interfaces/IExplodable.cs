using MGE;

namespace GAME.World
{
	public interface IExplodable
	{
		void Explode(Vector2Int position, bool recursive);
	}
}