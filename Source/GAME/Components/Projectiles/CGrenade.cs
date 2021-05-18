using MGE;
using MGE.Components;

namespace GAME.Components
{
	public class CGrenade : CProjectile
	{
		public CGrenade(DamageInfo info, string basePath) : base(info, basePath) { }

		int explosionDamage;

		public override void Init()
		{
			base.Init();

			explosionDamage = @params.GetInt("explosionDamage");
		}

		public override void Death()
		{
			base.Death();

			foreach (var thing in entity.layer.GetEntities(entity.position, radius, "Melee Vulnerable"))
			{
				thing.GetComponent<CObject>()?.Damage(explosionDamage, Vector2.GetDirection(entity.position + 0.5f, thing.position + 0.5f) * knockback, info.doneBy);
			}

			var para = new CParticle(5, GetAsset<Texture>("Explosion"), (p) => { p.frame = (byte)((p.timeAlive + (1 - p.id / 5) / 8) * 50 - 1); if (p.frame > 7) p.Kill(); });

			entity.layer.scene.GetLayer("Effects").AddEntity(new MGE.ECS.Entity(para));

			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(radius * 2 + 0.5f), Vector2.zero, new Color(0.5f), 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(radius * 2 + 0.25f), Vector2.zero, Color.red, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(radius * 2), Vector2.zero, Color.orange, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(radius * 2 - 0.25f), Vector2.zero, Color.yellow, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(radius * 2 - 0.5f), Vector2.zero, Color.white, 0, Vector2.zero);
		}
	}
}