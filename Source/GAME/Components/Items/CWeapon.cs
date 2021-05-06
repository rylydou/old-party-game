using MGE;

namespace GAME.Components
{
	public abstract class CWeapon : CItem
	{
		public abstract int startingUses { get; }
		public abstract float timeBtwAttacks { get; }

		public int uses;
		public float cooldown;

		public override void Init()
		{
			base.Init();

			uses = startingUses;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			cooldown -= Time.fixedDeltaTime;
		}

		public override void Draw()
		{
			base.Draw();

			Draw(currentSprite, Vector2.zero, Color.Lerp(Color.white, uses < 2 ? Color.red : Color.black, cooldown / timeBtwAttacks));
		}

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			cooldown = timeBtwAttacks;
		}

		public override void Use()
		{
			if (cooldown > 0) return;

			cooldown = timeBtwAttacks;

			uses--;

			Attack();

			if (uses < 1)
			{
				player.Pickup(null);
				Death();
				return;
			}
		}

		public virtual void Attack()
		{
			PlaySound("Attack");
		}
	}
}