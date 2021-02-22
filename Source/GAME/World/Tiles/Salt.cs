using MGE;
using GAME.Components;

namespace GAME.World
{
	public class Salt : Powder
	{
		public override string name => "Salt";
		public override Color color => new Color(0.75f, 0.75f, 0.75f);
		public override short density => 0;
		public override TileInfo info => TileInfo.BadForEnvironment | TileInfo.Airtight;

		public override void Update(Vector2Int position)
		{
			for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++)
					if (grid.TileIsType(position.x + x, position.y + y, typeof(Water)))
					{
						grid.SetTile(position.x, position.y, null);
						grid.SetTile(position.x + x, position.y + y, null);
					}

			base.Update(position);
		}
	}
}