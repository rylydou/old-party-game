using MGE;

namespace GAME.Components.Items
{
	public class CMinigun : CRangedWeapon
	{
		float attackTimePerClick;
		float timeAttacking;

		public override void Init()
		{
			base.Init();

			attackTimePerClick = @params.GetFloat("attackTimePerClick");
		}

		public override void Use()
		{
			timeAttacking = attackTimePerClick;
		}

		public override void Tick()
		{
			base.Tick();

			timeAttacking -= Time.fixedDeltaTime;

			if (timeAttacking > 0 && cooldown < 0)
			{
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
		}
	}
}