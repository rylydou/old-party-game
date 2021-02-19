using MGE;
using GAME.Components;

namespace GAME.World
{
	public class Salt : Powder
	{
		public override string name => "Salt";
		public override Color color => new Color(0.75f, 0.75f, 0.75f);
		public override int density => 0;

		public override void Update(Vector2Int position)
		{
			base.Update(position);

			for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++)
					if (CWorld.grid.GetTileLogic(position.x + x, position.y + y).GetType() == typeof(Water))
					{
						CWorld.grid.SetTileID(position.x, position.y, 0);
						CWorld.grid.SetTileID(position.x + x, position.y + y, 0);
					}
		}
	}
}