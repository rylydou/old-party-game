using MGE;
using MGE.Graphics;

namespace GAME.Components
{
	public class CPlayer : CObject
	{
		protected override string basePath => "Players";
		protected override string relitivePath => player.skin;

		public Player player;

		public float moveSpeed;
		public float crouchingMoveSpeed;

		float jumpMinVel;
		float jumpMaxVel;

		float groundedRem;
		float jumpRem;

		float extraFrictionGround;
		float extraFrictionAir;

		float interactionRange;

		Vector2 punchOffset;
		float punchRadius;
		float punchCooldown;
		int punchDamage;
		Vector2 punchKnockback;

		int healthOnKill;

		float flashTime;
		float flashIntensity;

		float lastHealthMem;
		float lastHealthMaxDelta;

		public CItem heldItem;
		CPlayer lastHitBy;
		CItem nearestItem;
		float groundedMem = -1;
		float jumpMem = -1;
		public float hitFlash = -1;
		float extraVelocity;
		public float lastHealth = 100;
		float lastHealthStayTime = -1;

		bool inputJump;
		bool inputJumpRelease;
		bool inputUse;
		bool inputDie;

		Texture texBody;
		// TODO: Add Animations
		Texture texCrouching;
		Texture texHand;
		Texture texArrow;

		public CPlayer(Player player)
		{
			this.player = player;
		}

		public override void Init()
		{
			base.Init();

			moveSpeed = @params.GetFloat("moveSpeed");
			crouchingMoveSpeed = @params.GetFloat("crouchingMoveSpeed");

			jumpMinVel = @params.GetFloat("jumpMinVelocity");
			jumpMaxVel = @params.GetFloat("jumpMaxVelocity");

			groundedRem = @params.GetFloat("groundedRem");
			jumpRem = @params.GetFloat("jumpRem");

			extraFrictionGround = @params.GetFloat("extraFrictionGround");
			extraFrictionAir = @params.GetFloat("extraFrictionAir");

			interactionRange = @params.GetFloat("interactionRange");

			punchOffset = @params.GetVector2("punchOffset");
			punchRadius = @params.GetFloat("punchRadius");
			punchCooldown = @params.GetFloat("punchCooldown");
			punchDamage = @params.GetInt("punchDamage");
			punchKnockback = @params.GetVector2("punchKnockback");

			healthOnKill = @params.GetInt("healthOnKill");

			flashTime = @params.GetFloat("flashTime");
			flashIntensity = @params.GetFloat("flashIntensity");

			lastHealthMem = @params.GetFloat("lastHealthMem");
			lastHealthMaxDelta = @params.GetFloat("lastHealthMaxDelta");

			texBody = GetAsset<Texture>("Body");
			texCrouching = GetAsset<Texture>("Crouching");
			texHand = GetAsset<Texture>("Hand");
			texArrow = GetAsset<Texture>("Arrow");

			rb.position = new Vector2(Random.Float(4, Window.sceneSize.x - 4), -1);

			PlaySound("Spawn");
		}

		public override void Tick()
		{
			base.Tick();

			if (entity.layer.raycaster.IsSolid(rb.position + 0.5f))
				Damage(5, Vector2.zero, null);

			extraVelocity = Math.Clamp(extraVelocity, -moveSpeed * 2, moveSpeed * 2);

			extraVelocity *= 1 - (rb.grounded ? extraFrictionGround : extraFrictionAir) * Time.fixedDeltaTime;

			rb.velocity.x = player.controls.move * (player.controls.crouch ? crouchingMoveSpeed : moveSpeed) * Time.fixedDeltaTime;

			rb.velocity.x += extraVelocity;

			groundedMem -= Time.fixedDeltaTime;
			if (rb.grounded)
				groundedMem = groundedRem;

			jumpMem -= Time.fixedDeltaTime;

			if (inputJump)
				jumpMem = jumpRem;
			inputJump = false;

			if (groundedMem > 0f && jumpMem > 0f)
			{
				groundedMem = -1f;
				jumpMem = -1f;

				if (player.controls.crouch)
				{
					if (entity.layer.raycaster.IsSolid(entity.position + Vector2.up))
					{
						rb.velocity.y = -jumpMinVel;
						PlaySound("Jump");
					}
					else
					{
						rb.position.y += GFX.currentUnitsPerPixel;
						PlaySound("Fall Through");
					}
				}
				else
				{
					rb.velocity.y = -jumpMaxVel;
					PlaySound("Jump");
				}
			}

			if (inputJumpRelease)
				if (rb.velocity.y < -jumpMinVel)
					rb.velocity.y = -jumpMinVel;
			inputJumpRelease = false;

			if (player.controls.move.Abs() > 0.1f)
				entity.scale.x = player.controls.move.Sign();

			nearestItem = entity.layer.GetNearestEntity(entity.position, interactionRange, "Pickupable")?.GetSimilarComponent<CItem>();

			if (inputUse)
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
						var things = entity.layer.GetEntities(entity.position + punchOffset * entity.scale, punchRadius, "Melee Vulnerable");
						var hitThing = false;

						foreach (var thing in things)
						{
							var thingComp = thing.GetSimilarComponent<CObject>();

							if (thingComp is object && thing != entity)
							{
								hitThing = true;

								thingComp.Damage(thing.GetComponent<CPlayer>()?.hitFlash > 0 ? 0 : punchDamage, punchKnockback * entity.scale, this);
							}
						}

						PlaySound("Punch Swing");

						if (hitThing)
							PlaySound("Punch Hit");
					}
					else
						heldItem.Use();
				}
			}
			inputUse = false;

			if (inputDie)
			{
				if (player.kills > 0)
					player.kills--;

				Damage(int.MinValue, Vector2.zero, null);
			}
			inputDie = false;

			hitFlash -= Time.fixedDeltaTime;
		}

		public override void Update()
		{
			base.Update();

			lastHealthStayTime -= Time.deltaTime;

			if (lastHealthStayTime < 0)
				lastHealth = Math.MoveTowards(lastHealth, health, lastHealthMaxDelta * Time.deltaTime);

			if (player.controls.jump) inputJump = true;
			if (player.controls.jumpRelease) inputJumpRelease = true;
			if (player.controls.use) inputUse = true;
			if (player.controls.die) inputDie = true;
		}

		public override void Draw()
		{
			var offset = hitFlash > 0 ? Random.UnitVector() / 16 * flashIntensity : Vector2.zero;

			Draw(player.controls.crouch ? texCrouching : texBody, new Vector2(GFX.currentUnitsPerPixel) + offset, new Color(0, 0.25f));
			Draw(player.controls.crouch ? texCrouching : texBody, offset, !player.controls.isConnected ? new Color(0.25f) : hitFlash > 0 ? Color.red : Color.white);

			if (nearestItem is object && heldItem is null)
				GFX.Draw(texArrow, nearestItem.entity.position + new Vector2(0, -1.5f + (1 - Math.Clamp01(Math.Tan(Time.time * 8f))) / 8));

			var healthBarPos = new Vector2(entity.position.x, entity.position.y - (player.controls.crouch ? -0.05f : 0.35f));

			GFX.DrawBox(new Rect(healthBarPos, 1, 0.125f), new Color(0, 0.25f));
			GFX.DrawBox(new Rect(healthBarPos, lastHealth / maxHealth, 0.125f), Color.white);
			GFX.DrawBox(new Rect(healthBarPos, (float)health / maxHealth, 0.125f), player.color);
		}

		public void Pickup(CItem item)
		{
			heldItem?.Drop();
			this.heldItem = item;
			item?.Pickup(this);
		}

		public override void Damage(int damage, Vector2 knockback, CPlayer source)
		{
			health -= damage;
			extraVelocity += knockback.x;
			rb.velocity.y = knockback.y;
			groundedMem = -1;

			hitFlash = flashTime;
			lastHealthStayTime = lastHealthMem;

			if (source is object) lastHitBy = source;

			if (health < 1)
				Death();

			PlaySound("Damage");
		}

		public override void Death()
		{
			base.Death();

			hitFlash = -1;
			player.deaths++;

			if (lastHitBy is object)
			{
				lastHitBy.player.kills++;
				lastHitBy.health = Math.Clamp(lastHitBy.health + lastHitBy.healthOnKill, lastHitBy.maxHealth);
			}

			Pickup(null);
		}
	}
}