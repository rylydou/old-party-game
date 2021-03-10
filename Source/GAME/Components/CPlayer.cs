using MGE;
using MGE.ECS;
using MGE.InputSystem;
using MGE.Graphics;

namespace GAME.Components
{
	public class CPlayer : Component
	{
		public double maxSpeed = 0.75;
		public double acceleration = 12.0;
		public double friction = 1.0 - 0.05;

		public double jumpVel = 1.5;

		CRigidbody rb;
		Texture body;

		public override void Init()
		{
			body = Assets.GetAsset<Texture>("Sprites/Player");

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

			Camera.main.position = entity.position + rb.size - (Vector2)Window.gameSize / 2;
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