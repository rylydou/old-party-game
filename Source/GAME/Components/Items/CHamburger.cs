using System;

namespace GAME.Components.Items
{
	public class CHamburger : CItem
	{
		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			player.health = Math.Clamp(player.health + @params.GetInt("restore"), 0, player.maxHealth);

			player.Pickup(null);
			entity.Destroy();
		}
	}
}