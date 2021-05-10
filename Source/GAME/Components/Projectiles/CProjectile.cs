using MGE;

namespace GAME.Components
{
	public class CProjectile : CChildObject
	{
		protected DamageInfo info;

		protected float speed;
		protected float lifetime;
		protected int hits;
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

			lifetime = @params.GetFloat("lifetime");
			hits = @params.GetInt("hits");

			if (enablePhysics)
				rb.velocity = @params.GetVector2("velocity") * entity.roationVector;
			else
				speed = @params.GetFloat("speed");

			texSprite = GetAsset<Texture>("Sprite");
		}

		public override void Tick()
		{
			base.Tick();

			var things = entity.layer.GetEntities(entity.position, @params.GetFloat("radius"), "Ranged Vulnerable");

			foreach (var thing in things)
			{
				if (thing == entity || thing == info.doneBy.entity) continue;

				thing.GetSimilarComponent<CObject>()?.Damage(@params.GetInt("damage"), -Vector2.GetDirection(info.origin, entity.position) * @params.GetFloat("knockback"), info.doneBy);

				PlaySound("Hit");

				hits--;

				if (hits < 1)
				{
					entity.Destroy();
					return;
				}
			}

			Move();

			lifetime -= Time.fixedDeltaTime;

			if (lifetime < 0 || entity.layer.raycaster.IsSolid(entity.position + 0.5f))
				entity.Destroy();
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