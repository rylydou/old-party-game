namespace GAME.Components.Items
{
	public class CMinigun : CRangedWeapon
	{
		bool isAttacking;

		public override void Init()
		{
			base.Init();
		}

		public override void Use()
		{
			isAttacking = !isAttacking;
		}

		public override void Tick()
		{
			base.Tick();

			if (isAttacking && cooldown < 0)
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

		public override void Drop()
		{
			base.Drop();

			isAttacking = false;
		}
	}
}