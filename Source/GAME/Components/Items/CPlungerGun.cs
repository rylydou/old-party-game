using GAME.Components.Projectiles;
using MGE;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CPlungerGun : CRangedWeapon
	{
		public override void SpawnProjectile(DamageInfo info, Vector2 position, float rotation)
		{
			Spawn(new Entity(new CPlunger(info, relitivePath)), position, rotation);
		}
	}
}