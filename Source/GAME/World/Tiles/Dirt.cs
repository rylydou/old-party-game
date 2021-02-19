using MGE;

namespace GAME.World
{
	public class Dirt : Wall
	{
		public override string name => "Dirt";
		public override Color color => new Color(0.25f, 0.2f, 0.1f);

		protected override void OnUpdate(Vector2Int position) { }
	}
}