using System;

namespace GAME.Components.Items
{
	public class CHamburger : CItem
	{
		public int hp = 25;

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			player.health = Math.Clamp(player.health + hp, 0, player.maxHealth);

			player.Pickup(null);
			entity.Destroy();
		}
	}
}