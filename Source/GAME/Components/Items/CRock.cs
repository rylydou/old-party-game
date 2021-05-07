using MGE;

namespace GAME.Components.Items
{
	public class CRock : CItem
	{
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

				var things = entity.layer.GetEntities(entity.position + new Vector2(0.25f), @params.GetFloat("range"), "Ranged Vulnerable");

				foreach (var thing in things)
				{
					if (this is null || thing == entity || lastPlayer is null || thing == lastPlayer.entity) continue;
					hitThing = true;

					thing.GetSimilarComponent<CObject>()?.Damage(@params.GetInt("damage"), rb.velocity * @params.GetFloat("knockback"), lastPlayer);
					thing.GetComponent<CPlayer>()?.Pickup(null);
					Damage(@params.GetInt("selfDamageOnHit"), rb.velocity, null);

					PlaySound("Hit");
				}

				if (hitThing)
				{
					cooldown = @params.GetFloat("hitCooldown");
					rb.velocity.x *= @params.GetFloat("bouncebackX");
					rb.velocity.y = @params.GetFloat("bouncebackY");
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