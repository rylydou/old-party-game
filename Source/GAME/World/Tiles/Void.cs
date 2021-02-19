using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Void : Wall
	{
		public override string name => "Void";
		public override Color color => new Color(Random.Color().grayscale * 0.15f);

		public override void Update(Vector2Int position)
		{
			var size = Random.Bool(0.001) ? 5 : 1;

			for (int y = -size; y <= size; y++)
				for (int x = -size; x <= size; x++)
				{
					var id = CWorld.grid.GetTileID(position.x + x, position.y + y);
					if (id == 0 || id == 9) continue;

					if (id == 7)
					{
						if (Random.Bool(0.01))
							CWorld.grid.SetTileID(position.x + x, position.y + y, 9);
						return;
					}

					if (Random.Bool(0.0125))
						CWorld.grid.SetTileID(position.x + x, position.y + y, 9);
					else if (Random.Bool(0.025))
						CWorld.grid.SetTileID(position.x + x, position.y + y, 7);
				}

			if (Random.Bool(0.0001))
				for (int y = -size; y <= size; y++)
					for (int x = -size; x <= size; x++)
					{
						if (CWorld.grid.GetTileID(position.x + x, position.y + y) == 9) continue;
						CWorld.grid.SetTileID(position.x + x, position.y + y, 7);
					}
		}
	}
}