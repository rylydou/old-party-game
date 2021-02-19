using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Void : Wall
	{
		public override string name => "Void";
		public override Color color => new Color(Random.Color().grayscale * 0.15f);

		protected override void OnUpdate(Vector2Int position)
		{
			var size = Random.Bool(0.001) ? 5 : 1;

			for (int y = -size; y <= size; y++)
				for (int x = -size; x <= size; x++)
				{
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
				}

			if (Random.Bool(0.0001))
				for (int y = -size; y <= size; y++)
					for (int x = -size; x <= size; x++)
					{
						if (grid.TileIsType(position.x + x, position.y + y, typeof(Void))) continue;
						grid.SetTile(position.x + x, position.y + y, new Smoke());
					}
		}
	}
}