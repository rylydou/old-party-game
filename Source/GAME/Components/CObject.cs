using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components
{
	public abstract class CObject : Component
	{
		const float despawnDist = 64;

		public abstract string basePath { get; }
		public abstract string relitivePath { get; }
		public virtual bool loadStandardAssets { get; } = true;
		public virtual bool meleeOnly { get; } = false;

		public virtual float frictionAir { get; } = 0.95f;
		public virtual float frictionGround { get; } = 0.700f;

		public int health = int.MinValue;
		public int maxHealth = 100;

		public CRigidbody rb;

		protected Sound damageSound;
		protected Sound deathSound;

		public override void Init()
		{
			base.Init();

			health = maxHealth;

			if (loadStandardAssets)
			{
				damageSound = GetAsset<Sound>("Damage");
				deathSound = GetAsset<Sound>("Death");
			}

			rb = entity.GetComponent<CRigidbody>();

			SetVulnerable(true);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (rb is object) rb.velocity.x *= rb.grounded ? frictionGround : frictionAir;
		}

		public override void Update()
		{
			base.Update();

			if (entity.position.sqrMagnitude > despawnDist * despawnDist)
				entity.Destroy();
		}

		public virtual void OnDamage(int damage, Vector2 knockback)
		{
			health -= damage;

			if (rb is object) rb.velocity = knockback;

			damageSound?.Play(entity.position);

			if (health < 1)
				OnDeath();
		}

		public virtual void OnDeath()
		{
			deathSound?.Play(entity.position);

			entity.Destroy();
		}

		public virtual void SetVulnerable(bool vulnerable)
		{
			if (vulnerable)
			{
				entity.AddTag("Melee Vulnerable");
				if (!meleeOnly)
					entity.AddTag("Ranged Vulnerable");
				else
					entity.RemoveTag("Ranged Vulnerable");
			}
			else
			{
				entity.RemoveTag("Melee Vulnerable");
				entity.RemoveTag("Ranged Vulnerable");
			}
		}

		public T GetAsset<T>(string path) where T : class
		{
			var asset = Assets.GetAsset<T>($"{basePath}/{relitivePath}/{path}");

			if (asset is null)
				asset = Assets.GetAsset<T>($"{basePath}/_Default/{path}");

			return asset;
		}
	}
}