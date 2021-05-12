using MGE;

namespace GAME.Components
{
	public abstract class CWeapon : CItem
	{
		protected float attackCooldown;

		protected int uses;
		protected float cooldown;

		public override void Init()
		{
			base.Init();

			uses = @params.GetInt("uses");
			attackCooldown = @params.GetFloat("attackCooldown");
		}

		public override void Tick()
		{
			base.Tick();

			cooldown -= Time.fixedDeltaTime;
		}

		public override void Draw()
		{
			base.Draw();

			Draw(currentSprite, Vector2.zero, Color.Lerp(Color.white, uses < 2 ? Color.red : Color.black, cooldown / attackCooldown));
		}

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			cooldown = attackCooldown;
		}

		public override void Use()
		{
			if (cooldown > 0) return;

			cooldown = attackCooldown;

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