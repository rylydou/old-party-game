using MGE;
using MGE.Graphics;

namespace GAME.Components.Projectiles
{
	public class CPlunger : CProjectile
	{
		public CPlunger(DamageInfo info, string basePath) : base(info, basePath) { }

		protected float pullSpeed;

		protected bool isPulling = false;

		public override void Init()
		{
			base.Init();

			pullSpeed = @params.GetFloat("pullSpeed");
		}

		public override void Tick()
		{
			if (!isPulling && entity.layer.raycaster.IsSolid(entity.position + Vector2.one / 2))
				isPulling = true;

			if (isPulling)
			{
				info.doneBy.rb.velocity = Vector2.GetDirection(info.doneBy.entity.position, entity.position) * pullSpeed;

				var things = entity.layer.GetEntities(entity.position, radius);

				foreach (var thing in things)
				{
					if (thing == info.doneBy.entity)
						Death();
				}
			}
			else
			{
				Move();
			}

			lifetime -= Time.fixedDeltaTime;
			if (lifetime < 0)
				Death();
		}

		public override void Draw()
		{
			base.Draw();

			GFX.DrawLine(entity.position + Vector2.one / 2, info.doneBy.entity.position + Vector2.one / 2, new Color(0.95f), 1);
		}
	}
}