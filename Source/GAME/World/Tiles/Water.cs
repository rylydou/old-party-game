using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Water : ITile
	{
		public string name => "Water";

		public Color color => Color.blue;

		public void Update(Vector2Int position)
		{

			if (!CWorld.grid.MoveTile(position, position + new Vector2Int(0, 1)))
			{
				if (!CWorld.grid.MoveTile(position, position + new Vector2Int(-1, 1)))
				{
					if (!CWorld.grid.MoveTile(position, position + new Vector2Int(1, 1)))
					{
						if (!CWorld.grid.MoveTile(position, position + new Vector2Int(1, 0)))
						{
							CWorld.grid.MoveTile(position, position + new Vector2Int(-1, 0));
						}
					}
				}
			}
		}
	}
}