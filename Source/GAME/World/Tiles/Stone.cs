using MGE;

namespace GAME.World
{
	public class Stone : Wall
	{
		public override string name => "Stone";
		public override Color color => new Color(1f / 3f);

		public override void Update(Vector2Int position) { }
	}
}