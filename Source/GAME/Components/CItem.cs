using System.Linq;
using System.Text.RegularExpressions;
using MGE;

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

		protected override string basePath { get => "Items"; }
		protected override string relitivePath
		{
			get => Regex.Replace(GetType().ToString().Split('.').Last().Remove(0, 1), @"\p{Lu}", c => " " + c.Value.ToUpperInvariant()).Remove(0, 1);
		}

		protected CPlayer player;
		protected ItemState state = ItemState.Dropped;
		protected float timeAlive;
		protected Texture currentSprite;

		protected Texture sprite;

		public override void Init()
		{
			base.Init();

			entity.AddTag("Pickupable");

			sprite = GetAsset<Texture>("Sprite");

			currentSprite = sprite;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

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

				if (timeAlive > timeToDespawn - 5)
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

			Draw(currentSprite, new Vector2(1f / 16), new Color(0, 0.25f));
			Draw(currentSprite);
		}

		public virtual void Pickup(CPlayer player)
		{
			entity.RemoveTag("Pickupable");
			state = ItemState.Held;
			entity.visible = true;

			this.player = player;

			timeAlive = Math.Clamp(timeAlive - 10, 0, float.PositiveInfinity);

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
			entity.AddTag("Pickupable");
			SetVulnerable(true);
			state = ItemState.Dropped;

			player = null;

			PlaySound("Drop");
		}

		public override void Death()
		{
			base.Death();

			player?.Pickup(null);
		}
	}
}