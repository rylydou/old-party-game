using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CFishingRod : CRangedWeapon
	{
		public override void SpawnProjectile(DamageInfo info, Vector2 position, float rotation)
		{
			Spawn(new Entity(new CRigidbody(), new CTrail(new Color(0.8f), 1, false), new CBobber(info, relitivePath)), position, rotation);
		}
	}
}