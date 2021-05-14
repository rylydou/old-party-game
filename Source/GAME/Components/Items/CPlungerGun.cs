using GAME.Components.Projectiles;
using MGE;
using MGE.Physics;

namespace GAME.Components.Items
{
	public class CPlungerGun : CItem
	{
		public override void Use()
		{
			var ray = entity.layer.raycaster.Raycast(entity.position + 0.5f, new Vector2(entity.scale.x, 0));

			if (RaycastHit.WithinDistance(ray, @params.GetFloat("range")))
			{
				Spawn(new MGE.ECS.Entity(new CPlunger(player, relitivePath)), ray.position, entity.scale);
				Death();
			}
		}
	}
}