using MGE;

namespace GAME.World
{
	public class Smoke : Gas
	{
		public override string name => "Smoke";
		public override Color color => new Color(0.15f, 0.5f);
		public override short density => -10;
		public override TileInfo info => TileInfo.BadForEnvironment;
	}
}