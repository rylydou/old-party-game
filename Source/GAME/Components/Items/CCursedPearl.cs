using MGE;
using MGE.Components;

namespace GAME.Components.Items
{
	public class CCursedPearl : CItem
	{
		float range;
		Vector2 offset;

		public override void Init()
		{
			base.Init();

			range = @params.GetFloat("range");
			offset = @params.GetVector2("tpOffset");
		}

		public override void Tick()
		{
			if (rb.grounded && state == ItemState.Thrown)
				Death();

			base.Tick();

			if (state == ItemState.Thrown && owner is object)
			{
				var things = entity.layer.GetEntities(entity.position, range);
				var tpedThing = false;

				foreach (var thing in things)
				{
					if (thing is null || thing == entity || thing == owner.entity) continue;

					var thingRb = thing.GetComponent<CRigidbody>();

					if (thingRb is object)
					{
						tpedThing = true;

						thingRb.position = owner.rb.position + offset * entity.scale;
					}
				}

				if (tpedThing)
				{
					PlaySound("TP");
				}
			}
		}
	}
}