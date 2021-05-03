using MGE;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CShotgun : CWeapon
	{
		public override int startingUses => 4;
		public override float timeBtwAttacks => 0.4f;

		public ProjectileData projectileData = new ProjectileData(new Damage(10, 0.125f), 0.5f, 0.5f, 0.25f, 1);

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			projectileData.damage.doneBy = player.entity;
		}

		public override void Attack()
		{
			base.Attack();

			projectileData.damage.origin = player.entity.position;

			for (int i = 0; i < 3; i++)
				Spawn(new Entity(new CProjectile(projectileData, relitivePath)), entity.position, entity.scale.x < 0 ? -Math.pi + Random.Float(-0.1f, 0.1f) : 0 + Random.Float(-0.1f, 0.1f));
		}
	}
}