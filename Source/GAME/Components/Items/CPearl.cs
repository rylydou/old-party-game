using MGE;

namespace GAME.Components.Items
{
	public class CPearl : CItem
	{
		public override bool meleeOnly => false;
		public override float frictionAir => 0.0f;

		public CPlayer owner;

		public override void Init()
		{
			maxHealth = 11;

			base.Init();

			rb.size = new Vector2(0.5f);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (rb.grounded && state == ItemState.Dropped && owner is object)
			{
				Death();
			}
		}

		public override void Damage(int damage, Vector2 knockback, CPlayer source)
		{
			if (health - damage < 1)
				owner = source;

			base.Damage(damage, knockback, source);

		}

		public override void Death()
		{
			base.Death();

			if (owner is object)
			{
				owner.rb.position = entity.position - rb.velocity.sign;

				owner.Damage(5, Vector2.zero, null);
			}
		}

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			owner = player;
		}
	}
}