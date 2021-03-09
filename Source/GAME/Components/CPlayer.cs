using Microsoft.Xna.Framework.Graphics;
using MGE;
using MGE.ECS;
using MGE.InputSystem;
using MGE.Graphics;

namespace GAME.Components
{
	public class CPlayer : Component
	{
		public double maxSpeed = 0.5;
		public double acceleration = 8.0;
		public double friction = 1.0 - 0.0125;

		public double jumpVel = 1;

		CRigidbody rb;
		Texture2D body;

		public override void Init()
		{
			body = Assets.GetAsset<Texture2D>("Sprites/Player");

			rb = entity.GetComponent<CRigidbody>();

			rb.position = new Vector2(CStage.current.tileSize * 4);
		}

		public override void Update()
		{
			var moveInput = ((Input.GetButton(Inputs.D) ? 1.0 : 0.0) - (Input.GetButton(Inputs.A) ? 1.0 : 0.0));

			rb.velocity.x = rb.velocity.x * friction;

			rb.velocity.x += moveInput * acceleration * Time.deltaTime;

			rb.velocity.x = Math.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);

			if (Input.GetButtonPress(Inputs.Space))
				rb.velocity.y = -jumpVel;
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				GFX.Draw(body, entity.position, Color.white);
			}
		}
	}
}