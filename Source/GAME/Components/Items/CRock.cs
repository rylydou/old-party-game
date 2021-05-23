using System.Collections.Generic;
using MGE;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CRock : CItem
	{
		int damage;
		float radius;
		int selfDamageOnHit;
		float knockback;
		float bouncebackX;
		float bouncebackY;

		List<Entity> thingsHit = new List<Entity>();

		public override void Init()
		{
			base.Init();

			damage = @params.GetInt("damage");
			radius = @params.GetFloat("radius");
			selfDamageOnHit = @params.GetInt("selfDamageOnHit");
			knockback = @params.GetFloat("knockback");
			bouncebackX = @params.GetFloat("bouncebackX");
			bouncebackY = @params.GetFloat("bouncebackY");
		}

		public override void Tick()
		{
			base.Tick();

			if (owner is null || player is object) return;

			if (state == ItemState.Thrown)
			{
				var hitThing = false;

				var things = entity.layer.GetEntities(entity.position + new Vector2(0.25f), radius, "Ranged Vulnerable");

				foreach (var thing in things)
				{
					if (this is null || thing == entity || thing == owner.entity || thingsHit.Contains(thing)) continue;
					hitThing = true;

					thingsHit.Add(thing);

					thing.GetSimilarComponent<CObject>()?.Damage(damage, rb.velocity * knockback, owner);
					thing.GetComponent<CPlayer>()?.Pickup(null);
					Damage(selfDamageOnHit, rb.velocity, null);

					PlaySound("Hit");
				}

				if (hitThing)
				{
					rb.velocity.x *= bouncebackX;
					rb.velocity.y = bouncebackY;
				}
			}
		}

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			thingsHit.Clear();
		}
	}
}