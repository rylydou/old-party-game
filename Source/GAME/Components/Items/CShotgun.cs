using MGE.ECS;

namespace GAME.Components.Items
{
	public class CShotgun : CItem
	{
		public override ItemType type => ItemType.Weapon;

		public override void Use()
		{
			base.Use();

			entity.layer.AddEntity(new Entity(new Projectile()));
		}
	}
}