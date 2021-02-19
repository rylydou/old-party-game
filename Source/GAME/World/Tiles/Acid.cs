using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Acid : Liquid
	{
		public override string name => "Acid";
		public override Color color => new Color(0, 1, 0, 0.75f);
		public override int density => -1;

		protected override void OnUpdate(Vector2Int position)
		{
			for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++)
				{
					var tileType = grid.GetTile(position.x + x, position.y + y).GetType();
					if (tileType == typeof(Air) || tileType == typeof(Void) || tileType == typeof(Acid)) continue;

					if (Random.Bool()) grid.SetTile(position, null);
					grid.SetTile(position.x + x, position.y + y, null);
				}

			base.OnUpdate(position);
		}
	}
}