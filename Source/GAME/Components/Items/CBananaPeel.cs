namespace GAME.Components.Items
{
	public class CBananaPeel : CItem
	{
		float radius;
		float slipAmount;

		public override void Init()
		{
			base.Init();

			radius = @params.GetFloat("radius");
			slipAmount = @params.GetFloat("slipAmount");
		}

		public override void Tick()
		{
			base.Tick();

			if (state == ItemState.Dropped)
			{
				foreach (var thing in entity.layer.GetEntities(entity.position + 0.5f, radius, "Melee Vulnerable"))
				{
					var obj = thing.GetSimilarComponent<CObject>();
					if (obj is object && obj != this && obj != owner)
					{
						obj.Damage(0, obj.rb.velocity * slipAmount, owner);

						PlaySound("Trip");

						health = int.MinValue;
						Death();
					}
				}
			}
		}
	}
}