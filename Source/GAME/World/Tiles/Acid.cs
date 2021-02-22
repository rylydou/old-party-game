using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Acid : Liquid
	{
		public override string name => "Acid";
		public override Color color => new Color(0, 1, 0, 0.75f);
		public override short density => -1;
		public override TileInfo info => TileInfo.NonCorruptible | TileInfo.BadForEnvironment | TileInfo.Airtight;

		public override void Update(Vector2Int position)
		{
			for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++)
				{
					if (!grid.Erode(position.x + x, position.y + y)) continue;
					if (Random.Bool()) grid.SetTile(position, null);
				}

			base.Update(position);
		}
	}
}