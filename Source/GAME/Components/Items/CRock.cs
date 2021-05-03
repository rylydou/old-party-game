using MGE;

namespace GAME.Components.Items
{
	public class CRock : CItem
	{
		public override float frictionAir => 0.98f;
		public override float frictionGround => 0.5f;

		public int damage = 20;
		public float timeBtwHits = 0.3f;

		float cooldown;
		CPlayer lastPlayer;

		public override void Init()
		{
			base.Init();

			rb.size = new Vector2(0.5f);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			cooldown -= Time.deltaTime;

			if (lastPlayer is null || player is object || cooldown > 0) return;

			if (rb.velocity.sqrMagnitude > 0.1f * Time.fixedDeltaTime)
			{
				var hitThing = false;

				var things = entity.layer.GetEntities(entity.position, 0.5f, "Ranged Vulnerable");

				foreach (var thing in things)
				{
					if (thing == entity || thing == lastPlayer.entity) continue;

					hitThing = true;

					thing.GetSimilarComponent<CObject>()?.OnDamage(damage, rb.velocity * 2.0f);
					OnDamage(34, rb.velocity);
				}

				if (hitThing)
				{
					cooldown = timeBtwHits;
					rb.velocity.x *= -1;
					rb.velocity.y = -0.1f;
				}
			}
		}

		public override void Draw()
		{
			Draw(currentSprite, -rb.size / 2);
		}

		// public override void OnDamage(int damage, Vector2 knockback)
		// {
		// 	base.OnDamage(damage, knockback);

		// 	lastPlayer = player;
		// }

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			lastPlayer = player;
		}
	}
}