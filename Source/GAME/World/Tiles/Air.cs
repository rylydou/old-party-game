using MGE;

namespace GAME.World
{
	public class Air : Tile
	{
		public override string name => "Air";
		public override Color color => Color.clear;
		public override short density => short.MinValue;
		public override TileType type => TileType.Gas;
		public override TileInfo info => TileInfo.Invincible;

		public override void Update(Vector2Int position) { }
	}
}