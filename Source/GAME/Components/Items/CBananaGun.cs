using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CBananaGun : CRangedWeapon
	{
		public override void Use()
		{
			var savePlayer = player;

			base.Use();

			var peel = new CBananaPeel();

			Spawn(new Entity(new CRigidbody(), peel), Vector2.zero);

			savePlayer.Pickup(peel);
		}
	}
}