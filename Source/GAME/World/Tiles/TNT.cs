using MGE;

namespace GAME.World
{
	public class TNT : Gravity, IExplodable
	{
		readonly Vector2Int exploSize = new Vector2Int(32, 32);

		public override string name => "TNT";
		public override Color color => new Color(0.75f, 0.25f, 0.25f);
		public override short density => 0;
		public override TileType type => TileType.Solid;
		public override TileInfo info => TileInfo.None;

		public void Explode(Vector2Int position, bool recursive)
		{
			grid.SetTile(position.x, position.y, null);
			grid.Explode(position, exploSize, recursive, true);
		}

		public override void Update(Vector2Int position)
		{
			base.Update(position);

			var tileType = grid.GetTile(position.x, position.y + 1).GetType();
			if (tileType == typeof(Air) || tileType == typeof(TNT)) return;

			Explode(position, true);
		}
	}
}