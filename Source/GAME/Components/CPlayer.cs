using GAME.Types;
using MGE;
using MGE.Graphics;

namespace GAME.Components
{
	public class CPlayer : CObject
	{
		public override string basePath => "Players";
		public override string relitivePath => player.skin;

		public Player player;

		public float moveSpeed = 6.75f;
		public float crouchSpeed = 2.67f;

		public override float frictionAir { get => 0.875f; }
		public override float frictionGround { get => 0.6f; }

		public float crouchFallVel = 0.25f;
		public float jumpMinVel = 0.2f;
		public float jumpMaxVel = 0.275f;

		public float groundedRem = 0.20f;
		public float jumpRem = 0.15f;

		CItem heldItem;
		CItem nearestItem;
		float groundedMem = -1;
		float jumpMem = -1;

		bool jump;
		bool jumpRelease;
		bool use;

		Texture texBody;
		// FIXME: Add Animations
		Texture texCrouching;
		Texture texArrow;
		Sound jumpSound;
		Sound landSound;
		Sound punchSwingSound;
		Sound punchHitSound;

		public CPlayer(Player player)
		{
			this.player = player;
		}

		public override void Init()
		{
			base.Init();

			texBody = GetAsset<Texture>("Body");
			texCrouching = GetAsset<Texture>("Crouching");
			texArrow = GetAsset<Texture>("Arrow");
			jumpSound = GetAsset<Sound>("Jump");
			landSound = GetAsset<Sound>("Land");
			punchSwingSound = GetAsset<Sound>("Punch Swing");
			punchHitSound = GetAsset<Sound>("Punch Hit");

			rb.position = new Vector2(Random.Float(1, Window.sceneSize.x - 1), 1);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			player.controls.Update();

			if (player.controls.move.Abs() > 0.1f)
				rb.velocity.x = player.controls.move * (player.controls.crouch ? crouchSpeed : moveSpeed) * Time.fixedDeltaTime;

			groundedMem -= Time.fixedDeltaTime;
			if (rb.grounded)
				groundedMem = groundedRem;

			jumpMem -= Time.fixedDeltaTime;

			if (jump)
				jumpMem = jumpRem;
			jump = false;

			if (groundedMem > 0f && jumpMem > 0f)
			{
				groundedMem = -1f;
				jumpMem = -1f;
				rb.velocity.y = player.controls.crouch ? -jumpMinVel : -jumpMaxVel;
				jumpSound?.Play(entity.position);
			}

			if (jumpRelease)
			{
				if (rb.velocity.y < -jumpMinVel)
				{
					rb.velocity.y = -jumpMinVel;
				}
			}
			jumpRelease = false;

			if (player.controls.move.Abs() > 0.1f)
				entity.scale.x = player.controls.move.Sign();

			if (player.controls.crouch)
				rb.velocity.y += crouchFallVel * Time.fixedDeltaTime;

			nearestItem = entity.layer.GetNearestEntity(entity.position, 1.25f, "Pickupable")?.GetSimilarComponent<CItem>();

			if (use)
			{
				if (player.controls.crouch)
				{
					if (heldItem is null)
						Pickup(nearestItem);
					else
						Pickup(null);
				}
				else
				{
					if (heldItem is null)
					{
						var things = entity.layer.GetEntities(entity.position + new Vector2(0.5f * entity.scale.x, 0.0f), 0.9f, "Melee Vulnerable");
						var hitThing = false;

						foreach (var thing in things)
						{
							var thingComp = thing.GetSimilarComponent<CObject>();

							if (thingComp is object && thing.GetComponent<CPlayer>() != this)
							{
								thingComp.OnDamage(10, new Vector2(entity.scale.x * 0.1f, -0.1f));
								hitThing = true;
							}
						}

						punchSwingSound?.Play(entity.position);

						if (hitThing)
							punchHitSound?.Play(entity.position);
					}
					else
						heldItem.Use();
				}
			}
			use = false;
		}

		public override void Update()
		{
			base.Update();

			if (player.controls.jump) jump = true;
			if (player.controls.jumpRelease) jumpRelease = true;
			if (player.controls.use) use = true;

			if (player.controls.DEBUG_SPAWN_BOX)
				Spawn(new MGE.ECS.Entity(new MGE.Components.CRigidbody(), new Items.CCrate()), entity.position + new Vector2(2 * entity.scale.x, -2));
		}

		public override void Draw()
		{
			Draw(player.controls.crouch ? texCrouching : texBody);

			if (nearestItem is object && heldItem is null)
			{
				GFX.Draw(texArrow, nearestItem.entity.position + new Vector2(0, -1.5f + Math.Sin(Time.time * 16) / 14));
			}

			GFX.DrawBox(new Rect(entity.position.x, entity.position.y - (player.controls.crouch ? -0.05f : 0.35f), 1, 0.125f), new Color(0, 0.25f));
			GFX.DrawBox(new Rect(entity.position.x, entity.position.y - (player.controls.crouch ? -0.05f : 0.35f), (float)health / maxHealth, 0.125f), player.color);
		}

		public void Pickup(CItem item)
		{
			heldItem?.Drop();
			this.heldItem = item;
			item?.Pickup(this);
		}

		public override void OnDeath()
		{
			base.OnDeath();

			Pickup(null);
		}
	}
}