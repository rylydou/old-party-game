using MGE;
using GAME.Components;

namespace GAME.World
{
	public class Sodium : Powder
	{
		public override string name => "Sodium";
		public override Color color => Color.white;
		public override int density => 0;

		protected override void OnUpdate(Vector2Int position)
		{
			for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++)
					if (grid.TileIsType(position.x + x, position.y + y, typeof(Water)))
					{
						grid.SetTile(position.x, position.y, null);

						for (int yy = -2; yy <= 2; yy++)
							for (int xx = -2; xx <= 2; xx++)
								if (xx != 0 && yy != 0) grid.SetTile(position.x + xx, position.y + yy, null);
					}

			base.OnUpdate(position);
		}
	}
}