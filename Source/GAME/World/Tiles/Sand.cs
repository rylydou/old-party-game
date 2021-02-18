using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Sand : ITile
	{
		public string name => "Sand";
		public Color color => new Color(0.9f, 0.9f, 0.1f);

		public void Update(Vector2Int position)
		{
			if (!CWorld.grid.MoveTile(position, position + new Vector2Int(0, 1)))
			{
				if (!CWorld.grid.MoveTile(position, position + new Vector2Int(-1, 1)))
				{
					CWorld.grid.MoveTile(position, position + new Vector2Int(1, 1));
				}
			}
		}
	}
}