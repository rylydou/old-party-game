using MGE;
using MGE.Components;

namespace GAME.Components.Items
{
	public class CCursedPearl : CItem
	{
		public override bool meleeOnly => false;
		public override float frictionAir => 0.0f;

		public CPlayer owner;

		public override void Init()
		{
			base.Init();

			rb.size = new Vector2(0.5f);
		}

		public override void FixedUpdate()
		{
			if (rb.grounded && state == ItemState.Thrown)
				Death();

			base.FixedUpdate();

			if (state == ItemState.Thrown && owner is object)
			{
				var things = entity.layer.GetEntities(entity.position, 2.0f);

				foreach (var thing in things)
				{
					if (thing is null || thing == entity || thing == owner.entity) continue;

					var thingRb = thing.GetComponent<CRigidbody>();

					if (thingRb is object)
					{
						thingRb.position = owner.rb.position;

						PlaySound("TP");
					}
				}
			}
		}

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			owner = player;
		}
	}
}