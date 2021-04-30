using MGE;
using MGE.ECS;

namespace GAME.Components
{
	public class Projectile : Component
	{
		public ProjectileData data = new ProjectileData(default, 1.25f, 8f, 0.15f);

		public Texture sprite;

		public override void FixedUpdate()
		{
			base.Update();

			entity.position += entity.roationVector * data.speed;
			data.lifetime -= Time.fixedDeltaTime;

			sprite = Assets.GetAsset<Texture>("Items/Shotgun/Projectile");

			if (data.lifetime < 0)
				entity.Destroy();
		}

		public override void Draw()
		{
			base.Draw();

			Draw(sprite, Vector2.zero, Color.red);
		}
	}
}