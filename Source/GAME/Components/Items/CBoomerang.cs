using System.Collections.Generic;
using MGE;
using MGE.Graphics;

namespace GAME.Components.Items
{
	public class CBoomerang : CItem
	{
		public List<CObject> thingsHit = new List<CObject>();

		float radius;
		int damage;
		float knockback;
		int damageOnHit;
		float throwVelocity;
		float goBackSpeed;
		float followSpeed;

		float velocity;
		bool flying;

		float lastPlayedSound;

		public override void Init()
		{
			base.Init();

			radius = @params.GetFloat("radius");
			damage = @params.GetInt("damage");
			knockback = @params.GetFloat("knockback");
			damageOnHit = @params.GetInt("damageOnHit");
			throwVelocity = @params.GetFloat("throwVelocity");
			goBackSpeed = @params.GetFloat("goBackSpeed");
			followSpeed = @params.GetFloat("followSpeed");
		}

		public override void Use()
		{
			base.Use();

			velocity = throwVelocity;
			flying = true;
			thingsHit.Clear();
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
				rb.velocity.x = velocity * entity.scale.x;
				rb.velocity.y = 0;

				if (entity.layer.raycaster.IsSolid(rb.position + 0.5f) | entity.layer.raycaster.IsSolid(rb.position + new Vector2(0.5f, 0.75f)) | entity.layer.raycaster.IsSolid(rb.position + new Vector2(0.5f, 0.25f)))
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

					if (Vector2.DistanceLT(rb.position, owner.rb.position, 2.5f))
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
							obj.Damage(damage, new Vector2(velocity.Sign() * knockback, -0.125f), owner);

							thingsHit.Add(obj);

							Damage(damageOnHit, Vector2.zero, null);
						}
					}
				}
			}
		}

		public override void Update()
		{
			base.Update();

			lastPlayedSound += Time.deltaTime;
			if (flying && lastPlayedSound > 0.2f)
			{
				lastPlayedSound = 0;
				PlaySound("Fly");
			}
		}

		public override void Draw()
		{
			GFX.Draw(sprite, new Rect(entity.position + 0.5f, 1), Color.white, flying ? Time.time * Math.pi * 16 : 0, new Vector2(8));
		}
	}
}