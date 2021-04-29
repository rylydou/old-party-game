using MGE;
using MGE.ECS;

namespace GAME.Components
{
	public class Projectile : Component
	{
		public ProjectileData data = new ProjectileData();

		public override void Update()
		{
			base.Update();

			entity.position += entity.roationVector * data.speed;
			data.lifetime -= Time.deltaTime;

			if (data.lifetime < 0)
				entity.Destroy();
		}

		public override void Draw()
		{
			base.Draw();

			Draw(null, Vector2.zero, Color.red);
		}
	}
}