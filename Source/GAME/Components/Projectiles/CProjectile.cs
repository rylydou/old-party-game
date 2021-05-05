using MGE;

namespace GAME.Components
{
	public class CProjectile : CChildObject
	{
		public override bool meleeOnly => true;

		public ProjectileData data;

		public Projectile(ProjectileData data, string basePath) : base(basePath)
		{
			this.data = data;
		}

		public Texture texSprite;

		public override void Init()
		{
			base.Init();

			texSprite = GetAsset<Texture>("Sprite");
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			var things = entity.layer.GetEntities(entity.position, data.radius, "Ranged Vulnerable");

			foreach (var thing in things)
			{
				if (thing == data.damage.doneBy) continue;

				thing.GetSimilarComponent<CObject>()?.Damage(data.damage.damage, -Vector2.GetDirection(data.damage.origin, entity.position) * data.damage.knockback + new Vector2(0, data.damage.knockback / 2), data.damage.doneBy.GetComponent<CPlayer>());

				PlaySound("Hit");

				data.hits--;

				if (data.hits < 1)
				{
					entity.Destroy();
					return;
				}
			}

			entity.position += entity.roationVector * data.speed;
			data.lifetime -= Time.fixedDeltaTime;

			if (data.lifetime < 0 || entity.layer.raycaster.IsSolid(entity.position + 0.5f))
				entity.Destroy();
		}

		public override void Draw()
		{
			base.Draw();

			Draw(texSprite);
		}
	}
}