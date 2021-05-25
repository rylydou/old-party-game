using MGE;
using MGE.Components;
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

		public CItem heldItem = null;
		CPlayer lastHitBy = null;
		CItem nearestItem = null;
		float groundedMem = -1;
		float jumpMem = -1;
		public float hitFlash = -1;
		float extraVelocity = 0;
		public float lastHealth = 100;
		float lastHealthStayTime = -1;
		public float timeRespawing = 0;

		bool inputJump;
		bool inputJumpRelease;
		bool inputUse;
		bool inputDie;

		Texture texBody;
		// TODO: Add Animations
		Texture texCrouching;
		Texture texArrow;

		Texture texPunchSwingEffect;
		Texture texDamageEffect;
		Texture texDeathEffect;
		Texture texSpawnEffect;

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
			texArrow = GetAsset<Texture>("Arrow");
			texPunchSwingEffect = GetAsset<Texture>("Punch Swing");
			texDamageEffect = GetAsset<Texture>("Damage Effect");
			texDeathEffect = GetAsset<Texture>("Death Effect");
			texSpawnEffect = GetAsset<Texture>("Spawn Effect");

			Start();
		}

		public void Start()
		{
			entity.enabled = true;
			entity.visible = true;

			health = maxHealth;

			heldItem = null;
			lastHitBy = null;
			nearestItem = null;
			groundedMem = -1;
			jumpMem = -1;
			hitFlash = -1;
			extraVelocity = 0;
			lastHealth = maxHealth;
			lastHealthStayTime = -1;
			timeRespawing = 0;
			rb.velocity = Vector2.zero;

			if (GameSettings.stage.playerSpawnPoints.Count < 1)
			{
				rb.position.x = Random.Float(5, Window.sceneSize.x - 6);
				rb.position.y = -2;

				PlaySound("Offscreen Spawn");
			}
			else
			{
				var bestSpawn = GameSettings.stage.playerSpawnPoints.Random();
				var bestSpawnScore = GetSpawnScore(bestSpawn);

				foreach (var spawn in GameSettings.stage.playerSpawnPoints)
				{
					var score = GetSpawnScore(spawn);

					if (score > bestSpawnScore)
					{
						bestSpawn = spawn;
						bestSpawnScore = score;
					}
				}

				if (bestSpawn.y > 0)
				{
					rb.position = bestSpawn;

					PlaySound("Spawn");

					var para = new CParticle(6, texSpawnEffect, (p) => { p.frame = (byte)(p.timeAlive * 40 - 1); if (p.frame > 7) p.Kill(); });

					entity.layer.scene.GetLayer("Foreground Effects").AddEntity(new MGE.ECS.Entity(para));

					para.SpawnParticle(rb.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(5.25f), Vector2.zero, Color.white, 0, Vector2.zero);
					para.SpawnParticle(rb.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(5.0f), Vector2.zero, player.color, 0, Vector2.zero);
					para.SpawnParticle(rb.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(4.75f), Vector2.zero, Color.white, 0, Vector2.zero);
					para.SpawnParticle(rb.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(4.5f), Vector2.zero, player.color, 0, Vector2.zero);
					para.SpawnParticle(rb.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(4.25f), Vector2.zero, Color.white, 0, Vector2.zero);
					para.SpawnParticle(rb.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(4.0f), Vector2.zero, player.color, 0, Vector2.zero);
					para.SpawnParticle(rb.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(3.75f), Vector2.zero, Color.white, 0, Vector2.zero);
				}
				else
				{
					rb.position.x = bestSpawn.x;
					rb.position.y = -2;

					PlaySound("Offscreen Spawn");
				}
			}
		}

		public float GetSpawnScore(Vector2 spawn, bool trueScore = false)
		{
			var score = 0.0f;

			foreach (var player in GameSettings.players)
			{
				if (player == this.player || player.player is null) continue;

				var dist = Vector2.DistanceSqr(spawn, player.player.entity.position);
				const float noSpawnDist = 2.5f;

				if (dist < noSpawnDist * noSpawnDist)
				{
					score = float.NegativeInfinity;
				}
				else
				{
					score += dist;
					score -= player.kills * 2;
				}
			}

			return score + (trueScore ? 0 : Random.Float(-7.5f, 7.5f));
		}

		public override void Tick()
		{
			base.Tick();

			if (entity.layer.raycaster.IsSolid(rb.position + 0.5f))
				Damage(5, Vector2.zero, null);

			extraVelocity = Math.Clamp(extraVelocity, -moveSpeed * 2, moveSpeed * 2);

			extraVelocity *= 1 - (rb.grounded ? extraFrictionGround : extraFrictionAir) * Time.fixedDeltaTime;

			if (rb.hitX)
				extraVelocity = 0;

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

						var para = new CParticle(1, texPunchSwingEffect, (p) => { p.frame = (byte)(p.timeAlive * 120 - 1); if (p.frame > 5) p.Kill(); });

						entity.layer.scene.GetLayer("Foreground Effects").AddEntity(new MGE.ECS.Entity(para));

						para.SpawnParticle(entity.position + (entity.scale.x > 0 ? new Vector2(1.0f, 0.25f) : new Vector2(-0.8f, 0.25f)) * entity.scale, Random.Float(-0.1f, 0.1f), new Vector2(1.33f * entity.scale.x, 1.0f), Vector2.zero, Color.white, 0, Vector2.zero);
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
			groundedMem = flashTime;

			hitFlash = flashTime;
			lastHealthStayTime = lastHealthMem;

			if (source is object && source != this) lastHitBy = source;

			PlaySound("Damage");

			var para = new CParticle(3, texDamageEffect, (p) => { p.frame = (byte)(p.timeAlive * 50 - 1); if (p.frame > 7) p.Kill(); });

			entity.layer.scene.GetLayer("Background Effects").AddEntity(new MGE.ECS.Entity(para));

			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(3.0f), Vector2.zero, player.color, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(3.0f), Vector2.zero, player.color.inverted, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(2.75f), Vector2.zero, Color.white, 0, Vector2.zero);

			if (health < 1)
				Death();
		}

		public override void Death()
		{
			hitFlash = -1;
			player.deaths++;

			if (lastHitBy is object && lastHitBy != this)
			{
				lastHitBy.player.kills++;
				lastHitBy.health = Math.Clamp(lastHitBy.health + lastHitBy.healthOnKill, lastHitBy.maxHealth);
			}

			PlaySound("Death");

			var para = new CParticle(6, texDeathEffect, (p) => { p.frame = (byte)((p.timeAlive + (1 - p.id / 6) / 8) * 45 - 1); if (p.frame > 7) p.Kill(); });

			entity.layer.scene.GetLayer("Foreground Effects").AddEntity(new MGE.ECS.Entity(para));

			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(3.75f), Vector2.zero, new Color(0, 0.67f), 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(3.5f), Vector2.zero, Color.violet, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(3.25f), Vector2.zero, Color.white, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(3.0f), Vector2.zero, Color.yellow, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(2.75f), Vector2.zero, Color.red, 0, Vector2.zero);
			para.SpawnParticle(entity.position + 0.5f, Random.Float(0, Math.pi4), new Vector2(2.5f), Vector2.zero, Color.white, 0, Vector2.zero);

			Pickup(null);

			entity.enabled = false;
			entity.visible = false;
		}
	}
}