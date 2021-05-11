using MGE;

namespace GAME.Components
{
	public class CProjectile : CChildObject
	{
		protected DamageInfo info;

		protected int damage;
		protected float knockback;
		protected float speed;
		protected float lifetime;
		protected int hits;
		protected float radius;
		protected bool enablePhysics;

		public CProjectile(DamageInfo info, string basePath) : base(basePath)
		{
			this.info = info;
		}

		public Texture texSprite;

		public override void Init()
		{
			base.Init();

			enablePhysics = @params.GetBool("enablePhysics");

			damage = @params.GetInt("damage");
			knockback = @params.GetFloat("knockback");
			lifetime = @params.GetFloat("lifetime");
			hits = @params.GetInt("hits");
			radius = @params.GetFloat("radius");

			if (enablePhysics)
				rb.velocity = @params.GetVector2("velocity") * entity.roationVector;
			else
				speed = @params.GetFloat("speed");

			texSprite = GetAsset<Texture>("Sprite");
		}

		public override void Tick()
		{
			base.Tick();

			entity.scale = entity.roationVector;

			var things = entity.layer.GetEntities(entity.position, radius, "Ranged Vulnerable");

			foreach (var thing in things)
			{
				if (thing == entity || thing == info.doneBy.entity) continue;

				thing.GetSimilarComponent<CObject>()?.Damage(damage, -Vector2.GetDirection(info.origin, entity.position) * knockback, info.doneBy);

				PlaySound("Hit");

				hits--;

				if (hits < 1)
				{
					Death();
					return;
				}
			}

			Move();

			lifetime -= Time.fixedDeltaTime;
			if (lifetime < 0 || entity.layer.raycaster.IsSolid(entity.position + 0.5f))
				Death();
		}

		public override void Draw()
		{
			base.Draw();

			Draw(texSprite);
		}

		public virtual void Move()
		{
			if (!enablePhysics)
				entity.position += entity.roationVector * speed /* * Time.fixedDeltaTime */;
		}
	}
}