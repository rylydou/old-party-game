using MGE;

namespace GAME.World
{
	public class Sponge : Wall
	{
		public override string name => "Sponge";
		public override Color color => new Color(1f, 0.9f, 0.1f);
		public override TileInfo info => TileInfo.None;

		public override void Update(Vector2Int position)
		{
			for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++)
					if (grid.GetTile(position.x + x, position.y + y).type == TileType.Liquid)
						grid.SetTile(position.x + x, position.y + y, null);
		}
	}
}