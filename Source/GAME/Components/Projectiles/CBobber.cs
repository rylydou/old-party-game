using MGE;

namespace GAME.Components
{
	public class CBobber : CProjectile
	{
		public CBobber(ProjectileData data, string basePath) : base(data, basePath) { }

		public override void Init()
		{
			base.Init();

			rb.size = new Vector2(0.25f);
			rb.velocity = new Vector2(0.66f * entity.roationVector.x, -0.075f);
		}

		public override void Move() { }
	}
}