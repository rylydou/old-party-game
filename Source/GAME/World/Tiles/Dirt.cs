using MGE;

namespace GAME.World
{
	public class Dirt : ITile
	{
		public string name => "Dirt";
		public Color color => new Color(0.25f, 0.2f, 0.1f);

		public void Update(Vector2Int position) { }
	}
}