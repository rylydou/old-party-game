using MGE;

namespace GAME.World
{
	public class Stone : ITile
	{
		public string name => "Stone";
		public Color color => new Color(1f / 3f);

		public void Update(Vector2Int position) { }
	}
}