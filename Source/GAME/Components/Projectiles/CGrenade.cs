using MGE;
using MGE.Components;

namespace GAME.Components
{
	public class CGrenade : CProjectile
	{
		public CGrenade(DamageInfo info, string basePath) : base(info, basePath) { }

		int explosionDamage;
		float explosionRadius;

		public override void Init()
		{
			base.Init();

			explosionDamage = @params.GetInt("explosionDamage");
			explosionRadius = @params.GetInt("explosionRadius");
		}

		public override void Tick()
		{
			entity.scale = entity.roationVector;

			foreach (var thing in entity.layer.GetEntities(entity.position, radius, "Ranged Vulnerable"))
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

			lifetime -= Time.fixedDeltaTime;
			if (lifetime < 0)
				Death();
		}

		public override void Death()
		{
			base.Death();

			Main.current.ShakeCamera(0.2f);

			foreach (var thing in entity.layer.GetEntities(entity.position, explosionRadius, "Melee Vulnerable"))
			{
				thing.GetSimilarComponent<CObject>()?.Damage(explosionDamage, Vector2.GetDirection(entity.position + 0.5f, thing.position + 0.5f) * -knockback, info.doneBy);
			}

			var para = new CParticle(5, GetAsset<Texture>("Explosion"), (p) => { p.frame = (byte)(p.timeAlive * 50 - 1); if (p.frame > 11) p.Kill(); });

			entity.layer.scene.GetLayer("Foreground Effects").AddEntity(new MGE.ECS.Entity(para));

			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(explosionRadius * 2 + 0.5f), Vector2.zero, new Color(0.5f), 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(explosionRadius * 2 + 0.25f), Vector2.zero, Color.red, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(explosionRadius * 2), Vector2.zero, Color.orange, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(explosionRadius * 2 - 0.25f), Vector2.zero, Color.yellow, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(explosionRadius * 2 - 0.5f), Vector2.zero, Color.white, 0, Vector2.zero);
		}
	}
}