using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Void : Wall
	{
		public override string name => "Void";
		public override Color color => new Color(Random.Color().grayscale * 0.15f);
		public override TileInfo info => TileInfo.None;

		public override void Update(Vector2Int position)
		{
			for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++)
				{
					var tile = grid.GetTile(position.x + x, position.y + y);
					if (tile.info.HasFlag(TileInfo.NonCorruptible)) return;

					var tileID = grid.GetTile(position.x + x, position.y + y).GetType();

					if (tileID == typeof(Air) || tileID == typeof(Void)) continue;

					if (tileID == typeof(Smoke))
					{
						if (Random.Bool(0.01))
							grid.SetTile(position.x + x, position.y + y, new Void());
						return;
					}

					if (Random.Bool(0.0125))
						grid.SetTile(position.x + x, position.y + y, new Smoke());
					else if (Random.Bool(0.025))
						grid.SetTile(position.x + x, position.y + y, new Void());
					else
						grid.UpdateArea(position.x + x, position.y + y);
				}
		}
	}
}