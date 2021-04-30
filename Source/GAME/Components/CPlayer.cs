using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.Physics;
using MGE.Components;
using MGE.InputSystem;

namespace GAME.Components
{
	public class CPlayer : Component
	{
		public float maxSpeed = 0.1f;
		public float acceleration = 1000.0f;
		public float friction = 0.67f;

		public float jumpVel = 0.215f;

		public float groundedRem = 0.20f;
		public float jumpRem = 0.15f;

		public float interactionDist = 1;

		public CItem item = null;
		CInteractable interactable = null;
		float groundedMem = -1;
		float jumpMem = -1;

		PlayerControls controls = null;

		CRigidbody rb = null;
		Texture body = null;

		public override void Init()
		{
			base.Init();

			body = Assets.GetAsset<Texture>("Sprites/Player");

			rb = entity.GetComponent<CRigidbody>();

			rb.raycaster = CStage.current;

			rb.position = new Vector2(2);

			controls = new PlayerControls();
		}

		public override void Update()
		{
			base.Update();

			controls.Update();

			rb.velocity.x = rb.velocity.x * friction;

			rb.velocity.x += controls.move * acceleration * Time.deltaTime;

			rb.velocity.x = Math.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);

			groundedMem -= Time.deltaTime;
			if (RaycastHit.WithinDistance(rb.raycaster.Raycast(rb.position + new Vector2(rb.size.x / 2, rb.size.y), Vector2.up), rb.skinWidth))
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

			foreach (var entity in entity.layer.GetEntitysWithTag("Interactable"))
			{
				var inter = entity.GetSimilarComponent<CInteractable>();

				var dist = Vector2.DistanceSqr(this.entity.position, inter.entity.position);

				if (dist < currentDistSqr)
				{
					currentDistSqr = dist;
					interactable = inter;
				}
			}

			if (Input.GetButtonPress(Inputs.E))
			{
				if (item is null)
					interactable?.Interact(this);
				else
					item?.Use();
			}

			if (Input.GetButtonPress(Inputs.Q))
			{
				item?.Drop();
			}
		}

		public override void Draw()
		{
			base.Draw();

			GFX.DrawCircle(Input.cameraMousePosition, 0.5f, Color.black, 0.25f);

			GFX.Draw(body, entity.position + new Vector2(0.1f, 0.1f), new Color(0, 0.1f));
			GFX.Draw(body, entity.position);
			if (interactable is object) GFX.DrawRect(new Rect(interactable.entity.position, 1, 1), Color.red, 0.1f);
		}

		public void Pickup(CItem item)
		{
			this.item = item;
		}
	}
}