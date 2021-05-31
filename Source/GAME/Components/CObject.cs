using System.Collections.Generic;
using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components
{
	public abstract class CObject : Component
	{
		const float despawnDist = 64;

		protected Params @params;

		protected abstract string basePath { get; }
		protected abstract string relitivePath { get; }

		public int health;
		public int maxHealth;

		public CRigidbody rb;

		protected Dictionary<string, SFX> sounds = new Dictionary<string, SFX>();

		public override void Init()
		{
			base.Init();

			@params = GetAsset<Params>(string.Empty);

			maxHealth = @params.GetInt("health");
			health = maxHealth;

			rb = entity.GetComponent<CRigidbody>();

			if (rb is object)
			{
				rb.size = @params.GetVector2("size");
			}

			SetVulnerable(true);
		}

		public override void Tick()
		{
			base.Tick();

			if (rb is object) rb.velocity.x *= rb.grounded ? (1 - @params.GetFloat("frictionGround") * Time.fixedDeltaTime) : (1 - @params.GetFloat("frictionAir") * Time.fixedDeltaTime);

			Stage.tilesets[GameSettings.stage.tiles.Get(entity.position + 0.5f)].Item1.ObjectInside(this);

			if (entity.position.y > Window.sceneSize.y + 1)
			{
				health = int.MinValue;
				PlaySound("Fall");
				Death();
			}
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
				if (@params.GetBool("meleeVulnerable")) entity.AddTag("Melee Vulnerable");
				if (@params.GetBool("rangedVulnerable")) entity.AddTag("Ranged Vulnerable");
			}
			else
			{
				entity.RemoveTag("Melee Vulnerable");
				entity.RemoveTag("Ranged Vulnerable");
			}
		}

		public virtual T GetAsset<T>(string path) where T : class
		{
			var asset = Assets.GetAsset<T>($"{basePath}/{relitivePath}/{path}");
			if (asset is null)
				asset = Assets.GetAsset<T>($"{basePath}/_Default/{path}");

			if (asset is null) LogWarning("No asset found at " + $"{basePath}/{relitivePath}/{path}" + " | " + $"{basePath}/_Default/{path}");

			return asset;
		}

		protected void PlaySound(string name)
		{
			SFX sound;
			if (!sounds.TryGetValue(name, out sound))
				sound = GetAsset<SFX>(name);
			if (sound is object)
				sound.Play(entity.position);
			else
				PlaySound("Error");
		}
	}
}