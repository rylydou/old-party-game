using MGE;

namespace GAME.Components.Items
{
	public class CBoomerang : CItem
	{
		int damage;
		float knockback;
		float throwVelocity;
		float goBackSpeed;

		float velocity;
		bool flying;

		public override void Init()
		{
			base.Init();

			damage = @params.GetInt("damage");
			knockback = @params.GetFloat("knockback");
			throwVelocity = @params.GetFloat("throwVelocity");
			goBackSpeed = @params.GetFloat("goBackSpeed");
		}

		public override void Use()
		{
			base.Use();

			velocity = throwVelocity;
			flying = true;
		}

		public override void Tick()
		{
			base.Tick();

			if (flying)
			{
				velocity -= goBackSpeed * Time.deltaTime;
				rb.velocity = velocity * entity.scale;

				if (velocity < 0)
				{
					flying = false;
				}
			}
		}
	}
}