using MGE;

namespace GAME.World
{
	public class Air : ITile
	{
		public string name => "Air";
		public Color color => Color.clear;

		public void Update(Vector2Int position) { }
	}
}