using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CRangedWeapon : CWeapon
	{
		public override void Attack()
		{
			base.Attack();

			var spread = @params.GetFloat("spread");
			var hits = @params.GetFloat("projectiles");
			var enablePhysics = @params.GetBool("enablePhysics");

			for (int i = 0; i < hits; i++)
			{
				if (enablePhysics)
				{
					Spawn(new Entity(new CRigidbody(), new CProjectile(new DamageInfo(player), relitivePath)), entity.position, (entity.scale.x < 0 ? -Math.pi : 0) + Random.Float(-spread, spread));
				}
				else
				{
					Spawn(new Entity(new CProjectile(new DamageInfo(player), relitivePath)), entity.position, (entity.scale.x < 0 ? -Math.pi : 0) + Random.Float(-spread, spread));
				}
			}
		}
	}
}