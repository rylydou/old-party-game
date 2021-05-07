using MGE;

namespace GAME.Components.Items
{
	public class CPearl : CItem
	{
		public CPlayer owner;

		public override void FixedUpdate()
		{
			if (rb.grounded && state == ItemState.Thrown && owner is object)
				Death();

			base.FixedUpdate();
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
				owner.rb.position = entity.position + rb.velocity.normalized * -@params.GetFloat("bumpBack");

				owner.Damage(@params.GetInt("userDamage"), @params.GetVector2("userVelocity"), null);
			}
		}

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			owner = player;
		}
	}
}