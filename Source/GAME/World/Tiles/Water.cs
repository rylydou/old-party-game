using GAME.Components;
using MGE;

namespace GAME.World
{
	public class Water : ITile
	{
		public string name => "Water";
		public Color color => new Color(0.25f, 0.25f, 0.75f, 0.75f);

		public void Update(Vector2Int position)
		{
			bool onGround = false;

			if (!CWorld.grid.MoveTile(position, position + new Vector2Int(0, 1)))
			{
				if (!CWorld.grid.MoveTile(position, position + new Vector2Int(-1, 1)))
				{
					if (!CWorld.grid.MoveTile(position, position + new Vector2Int(1, 1)))
					{
						onGround = true;
					}
				}
			}

			if (onGround)
			{
				if (!CWorld.grid.MoveTile(position, position + new Vector2Int(-1, 0)))
				{
					CWorld.grid.MoveTile(position, position + new Vector2Int(1, 0));
				}
			}
		}
	}
}