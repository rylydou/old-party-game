using MGE;

namespace GAME.Components.Items
{
	public class CPearl : CItem
	{
		public override float frictionAir => 0.999f;

		public CPlayer owner;

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (state == ItemState.Dropped && owner is object)
			{
				if (!rb.grounded) return;

				owner.rb.position = entity.position - rb.velocity.sign;

				owner.OnDamage(5, Vector2.zero, null);

				entity.Destroy();
			}
		}

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			owner = player;
		}
	}
}