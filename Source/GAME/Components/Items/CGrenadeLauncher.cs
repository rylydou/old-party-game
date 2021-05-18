using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CGrenadeLauncher : CRangedWeapon
	{
		public override void SpawnProjectile(DamageInfo info, Vector2 position, float rotation)
		{
			Spawn(new Entity(new CRigidbody(new Vector2(0.33f, 0.33f)), new CGrenade(info, relitivePath)), position, rotation);
		}
	}
}