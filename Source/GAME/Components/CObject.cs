using System.Collections.Generic;
using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components
{
	public abstract class CObject : Component
	{
		const float despawnDist = 64;

		public Feilds feilds;

		public abstract string basePath { get; }
		public abstract string relitivePath { get; }
		public virtual bool meleeOnly { get; } = false;

		public virtual float frictionGround { get; } = 14f;
		public virtual float frictionAir { get; } = 4f;

		public int health = int.MinValue;
		public int maxHealth = 100;

		public CRigidbody rb;

		Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

		public override void Init()
		{
			base.Init();

			health = maxHealth;

			feilds = GetAsset<Feilds>("");

			rb = entity.GetComponent<CRigidbody>();

			SetVulnerable(true);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (rb is object) rb.velocity.x *= rb.grounded ? (1 - frictionGround * Time.fixedDeltaTime) : (1 - frictionAir * Time.fixedDeltaTime);
		}

		public override void Update()
		{
			base.Update();

			if (entity.position.sqrMagnitude > despawnDist * despawnDist)
				entity.Destroy();
		}

		public virtual void Damage(int damage, Vector2 knockback, CPlayer source)
		{
			health -= damage;

			if (rb is object) rb.velocity = knockback;

			PlaySound("Damage");

			if (health < 1)
				Death();
		}

		public virtual void Death()
		{
			PlaySound("Death");

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

		protected void PlaySound(string name)
		{
			Sound sound;
			if (!sounds.TryGetValue(name, out sound))
				sound = GetAsset<Sound>(name);
			sound?.Play(entity.position);
		}
	}
}