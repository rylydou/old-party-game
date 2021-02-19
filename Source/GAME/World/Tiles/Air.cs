using MGE;

namespace GAME.World
{
	public class Air : Tile
	{
		public override string name => "Air";
		public override Color color => Color.clear;
		public override int density => int.MinValue;

		public override void Update(Vector2Int position) { }
	}
}