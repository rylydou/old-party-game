using MGE;
using MGE.Graphics;

namespace GAME.Components.Projectiles
{
	public class CPlunger : CChildObject
	{
		public CPlunger(CPlayer pulling, string basePath) : base(basePath)
		{
			this.pulling = pulling;
		}

		protected CPlayer pulling;

		protected float pullSpeed;
		protected float unstickRange;

		protected float timeLeft;

		protected Vector2 lastPos;

		public override void Init()
		{
			base.Init();

			pullSpeed = @params.GetFloat("pullSpeed");
			unstickRange = @params.GetFloat("unstickRange");
			timeLeft = @params.GetFloat("timePulling");
		}

		public override void Tick()
		{
			pulling.rb.velocity = Vector2.GetDirection(pulling.entity.position, entity.position) * pullSpeed;

			if (Vector2.DistanceLT(entity.position, pulling.entity.position, unstickRange))
				Death();

			timeLeft -= Time.fixedDeltaTime;
			if (timeLeft < 0)
				Death();

			lastPos = pulling.rb.position;
		}

		public override void Draw()
		{
			base.Draw();

			GFX.DrawLine(entity.position + Vector2.one / 2, pulling.entity.position + Vector2.one / 2, new Color(0.95f), 1);
		}
	}
}