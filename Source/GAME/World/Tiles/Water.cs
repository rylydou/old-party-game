using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Water : Liquid
	{
		public override string name => "Water";
		public override Color color => new Color(0.25f, 0.25f, 0.75f, 0.75f);
		public override short density => 0;
		public override TileInfo info => TileInfo.None;
	}
}