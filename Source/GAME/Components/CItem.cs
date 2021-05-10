using System.Linq;
using System.Text.RegularExpressions;
using MGE;
using MGE.Graphics;

public enum ItemState
{
	Dropped = 0,
	Held = 1,
	Thrown = 2
}

namespace GAME.Components
{
	public abstract class CItem : CObject
	{
		const float timeToDespawn = 45;
		const float timeRegainedWhenPickedUp = 10;
		const float timeLeftToStartFlashing = 5;

		protected override string basePath { get => "Items"; }
		protected override string relitivePath
		{
			get => Regex.Replace(GetType().ToString().Split('.').Last().Remove(0, 1), @"\p{Lu}", c => " " + c.Value.ToUpperInvariant()).Remove(0, 1);
		}

		protected CPlayer player;
		protected ItemState state = ItemState.Dropped;
		protected Texture currentSprite;
		protected float timeAlive;

		protected Texture sprite;

		public override void Init()
		{
			base.Init();

			entity.AddTag("Pickupable");

			sprite = GetAsset<Texture>("Sprite");

			currentSprite = sprite;
		}

		public override void Tick()
		{
			base.Tick();

			if (rb.grounded && state == ItemState.Thrown)
				state = ItemState.Dropped;

			if (player is object)
			{
				entity.scale = player.entity.scale;
				rb.velocity = new Vector2(0.05f * entity.scale.x, -0.05f);
				rb.position = player.entity.position + new Vector2(0.67f * entity.scale.x, -0.167f);
			}
			else
			{
				timeAlive += Time.fixedDeltaTime;

				if (timeAlive > timeToDespawn - timeLeftToStartFlashing)
					entity.visible = !entity.visible;

				if (timeAlive > timeToDespawn)
				{
					entity.Destroy();
					PlaySound("Despawn");
				}
			}
		}

		public override void Draw()
		{
			base.Draw();

			Draw(currentSprite, new Vector2(GFX.currentUnitsPerPixel), new Color(0, 0.25f));
			Draw(currentSprite);
		}

		public virtual void Pickup(CPlayer player)
		{
			this.player = player;

			SetVulnerable(false);
			state = ItemState.Held;

			entity.visible = true;

			timeAlive = Math.Clamp(timeAlive - timeRegainedWhenPickedUp, 0, float.PositiveInfinity);

			PlaySound("Pickup");
		}

		public virtual void Use()
		{
			rb.velocity = new Vector2(0.3f * entity.scale.x, -0.075f);

			player.Pickup(null);

			state = ItemState.Thrown;

			PlaySound("Throw");
		}

		public virtual void Drop()
		{
			SetVulnerable(true);
			state = ItemState.Dropped;

			player = null;

			PlaySound("Drop");
		}

		public override void SetVulnerable(bool vulnerable)
		{
			base.SetVulnerable(vulnerable);

			if (vulnerable)
			{
				entity.AddTag("Pickupable");
			}
			else
			{
				entity.RemoveTag("Pickupable");
			}
		}

		public override void Death()
		{
			base.Death();

			player?.Pickup(null);
		}
	}
}