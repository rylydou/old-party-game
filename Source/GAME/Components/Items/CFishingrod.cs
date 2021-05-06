using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CFishingRod : CWeapon
	{
		public override int startingUses => 1;
		public override float timeBtwAttacks => 0.1f;

		public ProjectileData projectileData = new ProjectileData(new Damage(1, -0.67f), 0.25f, 0.75f, 0.5f, 1);

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			projectileData.damage.doneBy = player.entity;
		}

		public override void Attack()
		{
			base.Attack();

			projectileData.damage.origin = player.entity.position;

			Spawn(new Entity(new CRigidbody(), new CBobber(projectileData, relitivePath)), entity.position, entity.scale.x < 0 ? -Math.pi : 0);
		}
	}
}