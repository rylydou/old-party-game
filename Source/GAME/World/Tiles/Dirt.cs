using MGE;

namespace GAME.World
{
	public class Dirt : Wall
	{
		public override string name => "Dirt";
		public override Color color => new Color(0.4f, 0.3f, 0.2f);
		public override TileInfo info => TileInfo.Airtight;

		public override void Update(Vector2Int position)
		{
			if (grid.TileIsType(position.x, position.y - 1, typeof(Water)))
			{
				grid.SetTile(position, new Grass());

				if (Random.Bool(1))
					grid.SetTile(position.x, position.y - 1, new Vine());
			}
		}
	}
}