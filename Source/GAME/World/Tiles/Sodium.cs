using MGE;
using GAME.Components;

namespace GAME.World
{
	public class Sodium : Powder, IExplodable
	{
		readonly Vector2Int exploSize = new Vector2Int(5, 5);

		public override string name => "Sodium";
		public override Color color => Color.white;
		public override short density => 0;
		public override TileInfo info => TileInfo.BadForEnvironment | TileInfo.Airtight;

		public void Explode(Vector2Int position, bool recursive)
		{
			grid.SetTile(position.x, position.y, null);
			grid.Explode(position, exploSize, recursive, false);
		}

		public override void Update(Vector2Int position)
		{
			for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++)
					if (grid.TileIsType(position.x + x, position.y + y, typeof(Water)))
						Explode(position, true);

			base.Update(position);
		}
	}
}