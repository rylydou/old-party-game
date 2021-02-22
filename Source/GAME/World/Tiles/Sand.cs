using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Sand : Powder
	{
		public override string name => "Sand";
		public override Color color => new Color(0.85f, 0.85f, 0.25f);
		public override short density => 0;
		public override TileInfo info => TileInfo.Airtight;
	}
}