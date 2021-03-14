using MGE;
using MGE.ECS;
using MGE.InputSystem;
using MGE.Graphics;
using MGE.Physics;

namespace GAME.Components
{
	public class CPlayer : Component
	{
		public float maxSpeed = 0.75f;
		public float acceleration = 12.0f;
		public float friction = 1.0f - 0.1f;

		public float jumpVel = 1.5f;

		public float groundedRem = 0.20f;
		public float jumpRem = 0.15f;

		float groundedMem;
		float jumpMem;

		CRigidbody rb;
		Texture body;

		public override void Init()
		{
			body = Assets.GetAsset<Texture>("Sprites/Player");

			rb = entity.GetComponent<CRigidbody>();

			rb.position = new Vector2(64);
		}

		public override void Update()
		{
			var moveInput = ((Input.GetButton(Inputs.D) ? 1.0f : 0.0f) - (Input.GetButton(Inputs.A) ? 1.0f : 0.0f));

			rb.velocity.x = rb.velocity.x * friction;

			rb.velocity.x += moveInput * acceleration * Time.deltaTime;

			rb.velocity.x = Math.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);

			groundedMem -= Time.deltaTime;
			if (RaycastHit.WithinDistance(rb.raycaster.Raycast(rb.position + new Vector2(rb.size.x / 2, rb.size.y), Vector2.up), 2f))
				groundedMem = groundedRem;

			jumpMem -= Time.deltaTime;
			if (Input.GetButtonPress(Inputs.Space))
				jumpMem = jumpRem;

			if (groundedMem > 0f && jumpMem > 0f)
			{
				groundedMem = -1f;
				jumpMem = -1f;
				rb.velocity.y = -jumpVel;
			}

			Camera.main.zoom -= Input.scroll * Camera.main.zoom * Time.deltaTime * 4;

			Camera.main.position = entity.position + rb.size - (Vector2)Window.gameSize / (2 * (Camera.main.zoom * Camera.main.zoom));
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