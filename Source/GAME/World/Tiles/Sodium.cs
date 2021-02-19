using MGE;
using GAME.Components;

namespace GAME.World
{
	public class Sodium : Powder
	{
		public override string name => "Sodium";
		public override Color color => Color.white;
		public override int density => 0;

		public override void Update(Vector2Int position)
		{
			for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++)
					if (CWorld.grid.GetTileLogic(position.x + x, position.y + y).GetType() == typeof(Water))
					{
						CWorld.grid.SetTileID(position.x, position.y, 0);

						for (int yy = -2; yy <= 2; yy++)
							for (int xx = -2; xx <= 2; xx++)
								if (xx != 0 && yy != 0) CWorld.grid.SetTileID(position.x + xx, position.y + yy, 0);
					}

			base.Update(position);
		}
	}
}