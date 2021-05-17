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

		CPlayer pulling;

		float pullSpeed;
		float unstickRange;

		float timeLeft;

		Vector2 lastPos;

		Texture sprite;

		public override void Init()
		{
			base.Init();

			pullSpeed = @params.GetFloat("pullSpeed");
			unstickRange = @params.GetFloat("unstickRange");
			timeLeft = @params.GetFloat("timePulling");

			sprite = GetAsset<Texture>("Sprite");
		}

		public override void Tick()
		{
			pulling.rb.position += Vector2.GetDirection(pulling.entity.position, entity.position - 0.5f) * pullSpeed;
			pulling.rb.velocity = Vector2.zero;

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

			GFX.DrawLine(pulling.entity.position + 0.5f, entity.position, new Color(0.95f), 1);

			Draw(sprite, new Vector2(-0.5f));
		}
	}
}