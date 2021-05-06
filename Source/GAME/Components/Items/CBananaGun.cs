using MGE;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CBananaGun : CWeapon
	{
		public override int startingUses => 1;
		public override float timeBtwAttacks => 0.1f;

		public ProjectileData projectileData = new ProjectileData(new Damage(50, 1.25f), 0.33f, 0.5f, 0.33f, 1);

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			projectileData.damage.doneBy = player.entity;
		}

		public override void Attack()
		{
			base.Attack();

			projectileData.damage.origin = player.entity.position;

			Spawn(new Entity(new CProjectile(projectileData, relitivePath)), entity.position, entity.scale.x < 0 ? -Math.pi : 0);
		}
	}
}