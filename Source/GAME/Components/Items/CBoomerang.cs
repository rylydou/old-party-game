using System.Collections.Generic;
using MGE;

namespace GAME.Components.Items
{
	public class CBoomerang : CItem
	{
		public List<CObject> thingsHit = new List<CObject>();

		float radius;
		int damage;
		float knockback;
		float throwVelocity;
		float goBackSpeed;
		float followSpeed;

		float velocity;
		bool flying;

		public override void Init()
		{
			base.Init();

			radius = @params.GetFloat("radius");
			damage = @params.GetInt("damage");
			knockback = @params.GetFloat("knockback");
			throwVelocity = @params.GetFloat("throwVelocity");
			goBackSpeed = @params.GetFloat("goBackSpeed");
			followSpeed = @params.GetFloat("followSpeed");
		}

		public override void Use()
		{
			base.Use();

			velocity = throwVelocity;
			flying = true;
		}

		public override void Tick()
		{
			base.Tick();

			if (flying)
			{
				if (owner is null)
				{
					flying = false;
					return;
				}

				var wasMovingFowards = velocity > 0;
				velocity -= goBackSpeed * Time.fixedDeltaTime;
				if (velocity < 0 && wasMovingFowards)
					thingsHit.Clear();
				rb.position += velocity * entity.scale * Time.fixedDeltaTime;
				rb.velocity.x = velocity * entity.scale
				rb.velocity.y = 0;

				if (entity.layer.raycaster.IsSolid(rb.position + 0.5f))
				{
					if (velocity > 0)
						velocity = -velocity;
					else
					{
						flying = false;
						return;
					}

					rb.position += velocity * entity.scale * 8 * Time.fixedDeltaTime;
				}

				if (velocity < 0)
				{
					rb.position.y = Math.MoveTowards(rb.position.y, owner.rb.position.y, followSpeed);

					if (Vector2.DistanceLT(rb.position, owner.rb.position, 1.0f))
					{
						if (owner.heldItem is null)
							owner.Pickup(this);
						else
						{
							flying = false;
							return;
						}
					}
				}

				foreach (var thing in entity.layer.GetEntities(entity.position, radius, "Ranged Vulnerable"))
				{
					var obj = thing.GetSimilarComponent<CObject>();

					if (obj is object && obj != this && obj != owner)
					{
						if (!thingsHit.Contains(obj))
						{
							obj.Damage(damage, new Vector2(velocity, -0.125f), owner);

							thingsHit.Add(obj);
						}
					}
				}
			}
		}
	}
}