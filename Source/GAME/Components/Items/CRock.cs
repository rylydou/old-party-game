using MGE;

namespace GAME.Components.Items
{
	public class CRock : CItem
	{
		public override float frictionGround => 0.5f;
		public override float frictionAir => 0.0f;

		public int damage = 30;
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

				var things = entity.layer.GetEntities(entity.position + new Vector2(0.25f), 0.5f, "Ranged Vulnerable");

				foreach (var thing in things)
				{
					if (this is null || thing == entity || lastPlayer is null || thing == lastPlayer.entity) continue;
					hitThing = true;

					thing.GetSimilarComponent<CObject>()?.Damage(damage, rb.velocity * 2.0f, lastPlayer);
					thing.GetComponent<CPlayer>()?.Pickup(null);
					Damage(30, rb.velocity, null);

					PlaySound("Hit");
				}

				if (hitThing)
				{
					cooldown = timeBtwHits;
					rb.velocity.x *= -1;
					rb.velocity.y = -0.1f;
				}
			}
		}

		public override void Damage(int damage, Vector2 knockback, CPlayer source)
		{
			base.Damage(damage, knockback, source);

			lastPlayer = source;
		}

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			lastPlayer = player;
		}
	}
}