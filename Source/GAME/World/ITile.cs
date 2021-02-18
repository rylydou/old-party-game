using MGE;

namespace GAME.World
{
	public interface ITile
	{
		string name { get; }
		Color color { get; }

		void Update();
	}
}