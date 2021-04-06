using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.Physics;
using MGE.Components;

namespace GAME.Components
{
	public class CPlayer : Component
	{
		public float maxSpeed = 0.1f;
		public float acceleration = 1000.0f;
		public float friction = 0.67f;

		public float jumpVel = 0.2f;

		public float groundedRem = 0.20f;
		public float jumpRem = 0.15f;

		public float interactionDist = 1;

		CCrate interactable;
		float groundedMem = -1;
		float jumpMem = -1;

		PlayerControls controls;

		CRigidbody rb;
		Texture body;

		public override void Init()
		{
			body = Assets.GetAsset<Texture>("Sprites/Player");

			rb = entity.GetComponent<CRigidbody>();

			rb.raycaster = CStage.current;

			rb.position = new Vector2(2);

			controls = new PlayerControls();
		}

		public override void Update()
		{
			controls.Update();

			rb.velocity.x = rb.velocity.x * friction;

			rb.velocity.x += controls.move * acceleration * Time.deltaTime;

			rb.velocity.x = Math.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);

			groundedMem -= Time.deltaTime;
			if (RaycastHit.WithinDistance(rb.raycaster.Raycast(rb.position + new Vector2(rb.size.x / 2, rb.size.y), Vector2.up), 2f))
				groundedMem = groundedRem;

			jumpMem -= Time.deltaTime;
			if (controls.jump)
				jumpMem = jumpRem;

			if (groundedMem > 0f && jumpMem > 0f)
			{
				groundedMem = -1f;
				jumpMem = -1f;
				rb.velocity.y = -jumpVel;
			}

			interactable = null;
			var currentDistSqr = interactionDist * interactionDist;

			foreach (var entity in entity.layer.entities)
			{
				var crate = entity.GetComponent<CCrate>();

				if (crate is object)
				{
					var dist = Vector2.DistanceSqr(this.entity.position, crate.entity.position);

					if (dist < currentDistSqr)
					{
						currentDistSqr = dist;
						interactable = crate;
					}
				}
			}
		}

		public override void Draw()
		{
			GFX.Draw(body, entity.position, Color.white);
			if (interactable is object) GFX.DrawRect(new Rect(interactable.entity.position, 16, 16), Color.red);
		}
	}
}