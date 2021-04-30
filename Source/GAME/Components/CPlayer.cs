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
		public float moveSpeed = 8f;
		public float crouchSpeed = 6f;

		public float jumpMinVel = 0.3f;
		public float jumpMaxVel = 0.4f;

		public float groundedRem = 0.20f;
		public float jumpRem = 0.15f;

		public float interactionDist = 1.0f;

		CItem item = null;
		CItem nearestItem = null;
		float groundedMem = -1;
		float jumpMem = -1;

		PlayerControls controls = null;
		bool jump = false;
		bool use = false;

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

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			controls.Update();

			rb.velocity.x = controls.move * (controls.crouch ? crouchSpeed : moveSpeed) * Time.fixedDeltaTime;

			groundedMem -= Time.fixedDeltaTime;
			if (RaycastHit.WithinDistance(rb.raycaster.Raycast(rb.position + new Vector2(rb.size.x / 2, rb.size.y), Vector2.up), rb.skinWidth))
				groundedMem = groundedRem;

			jumpMem -= Time.fixedDeltaTime;

			if (jump)
				jumpMem = jumpRem;
			jump = false;

			if (groundedMem > 0f && jumpMem > 0f)
			{
				groundedMem = -1f;
				jumpMem = -1f;
				rb.velocity.y = -jumpMaxVel;
			}

			nearestItem = entity.layer.GetNearestEntity(entity.position, 1f, "Pickupable")?.GetComponent<CItem>();

			if (use)
			{
				if (controls.crouch)
				{
					if (item is null)
						nearestItem?.Pickup(this);
					else
						item.Use();
				}
				else
				{
					if (item is null)
						Log("Used Base Attack");
					else
						item.Drop();
				}
			}
			use = false;
		}

		public override void Update()
		{
			base.Update();

			if (controls.jump) jump = true;
			if (controls.use) use = true;
		}

		public override void Draw()
		{
			base.Draw();

			GFX.DrawCircle(Input.cameraMousePosition, 0.5f, Color.black, 0.25f);

			GFX.Draw(body, entity.position + new Vector2(0.1f, 0.1f), new Color(0, 0.1f));
			GFX.Draw(body, entity.position);
			if (nearestItem is object) GFX.DrawRect(new Rect(nearestItem.entity.position, 1, 1), Color.red, 0.1f);
		}

		public void Drop()
		{
			item?.Drop();
		}

		public void Pickup(CItem item)
		{
			this.item = item;
		}
	}
}