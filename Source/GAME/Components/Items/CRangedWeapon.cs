using MGE;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CRangedWeapon : CWeapon
	{
		protected float spread;
		protected float hits;

		public override void Init()
		{
			base.Init();

			spread = @params.GetFloat("spread");
			hits = @params.GetFloat("projectiles");
		}

		public override void Attack()
		{
			base.Attack();

			for (int i = 0; i < hits; i++)
			{
				SpawnProjectile(new DamageInfo(player), entity.position, (entity.scale.x < 0 ? -Math.pi : 0) + Random.Float(-spread, spread));
			}
		}

		public virtual void SpawnProjectile(DamageInfo info, Vector2 position, float rotation)
		{
			Spawn(new Entity(new CProjectile(info, relitivePath)), position, rotation);
		}
	}
}