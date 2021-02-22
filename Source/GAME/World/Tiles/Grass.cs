using MGE;

namespace GAME.World
{
	public class Grass : Wall
	{
		public override string name => "Grass";
		public override Color color => new Color(0.25f, 0.75f, 0.33f);
		public override TileInfo info => TileInfo.None;

		public override void Update(Vector2Int position)
		{
			if (grid.TileExists(position.x, position.y - 1, typeof(Water)))
				grid.SetTile(position, new Dirt());
		}
	}
}