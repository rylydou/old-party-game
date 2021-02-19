using MGE;

namespace GAME.World
{
	public class Stone : Wall
	{
		public override string name => "Stone";
		public override Color color => new Color(1f / 3f);

		protected override void OnUpdate(Vector2Int position) { }
	}
}