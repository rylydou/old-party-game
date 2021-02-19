using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Acid : Liquid
	{
		public override string name => "Acid";
		public override Color color => new Color(0, 1, 0, 0.75f);
		public override int density => -1;

		public override void Update(Vector2Int position)
		{
			for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++)
				{
					var id = CWorld.grid.GetTileID(position.x + x, position.y + y);
					if (id == 0 || id == 9 || id == 10) continue;

					if (Random.Bool()) CWorld.grid.SetTileID(position, 0);
					CWorld.grid.SetTileID(position.x + x, position.y + y, 0);
				}

			base.Update(position);
		}
	}
}