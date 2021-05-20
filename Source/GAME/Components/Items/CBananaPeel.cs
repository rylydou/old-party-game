namespace GAME.Components.Items
{
	public class CBananaPeel : CItem
	{
		float radius;
		int damage;
		float slipAmount;

		public override void Init()
		{
			base.Init();

			radius = @params.GetFloat("radius");
			damage = @params.GetInt("damage");
			slipAmount = @params.GetFloat("slipAmount");
		}

		public override void Tick()
		{
			base.Tick();

			if (state == ItemState.Dropped)
			{
				foreach (var thing in entity.layer.GetEntities(entity.position + 0.5f, radius, "Ranged Vulnerable"))
				{
					var obj = thing.GetSimilarComponent<CObject>();
					if (obj is object && obj is CPlayer && obj != this && obj != owner)
					{
						obj.Damage(damage, obj.rb.velocity.sign * slipAmount, owner);

						PlaySound("Trip");

						health = int.MinValue;
						Death();
					}
				}
			}
		}
	}
}