using MGE;

namespace GAME.World
{
	public class Oil : Liquid
	{
		public override string name => "Oil";
		public override Color color => new Color(0.1f);
		public override short density => -1;
		public override TileInfo info => TileInfo.None;
	}
}