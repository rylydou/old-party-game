using MGE;
using MGE.ECS;
using MGE.InputSystem;
using MGE.Graphics;

namespace GAME.Components
{
	public class CPlayer : Component
	{
		public float maxSpeed = 0.75f;
		public float acceleration = 12.0f;
		public float friction = 1.0f - 0.05f;

		public float jumpVel = 1.5f;

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
			var moveInput = ((Input.GetButton(Inputs.D) ? 1.0f : 0.0f) - (Input.GetButton(Inputs.A) ? 1.0f : 0.0f));

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