using MGE;

namespace GAME.World
{
	public class TNT : Powder
	{
		const int exploSize = 10;

		public override string name => "TNT";
		public override Color color => new Color(0.75f, 0.25f, 0.25f);
		public override int density => 0;

		protected override void OnUpdate(Vector2Int position)
		{
			base.OnUpdate(position);

			if (!grid.TileIsType(position.x, position.y + 1, typeof(Air)))
			{
				if (grid.TileIsType(position.x, position.y + 1, typeof(TNT))) return;
				for (int y = -exploSize; y <= exploSize; y++)
					for (int x = -exploSize; x <= exploSize; x++)
						if (Random.Bool(Math.Abs((double)(y * x) / exploSize))) grid.SetTile(position.x + x, position.y + y, null);
			}
		}
	}
}