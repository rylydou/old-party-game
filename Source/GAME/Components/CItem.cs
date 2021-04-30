using System.Linq;
using MGE;
using MGE.Components;
using MGE.ECS;

public enum ItemType
{
	Item = 0,
	Weapon = 1,
	Wearable = 2,
}

public enum ItemState
{
	Dropped = 0,
	Held = 1,
	Thrown = 2
}

namespace GAME.Components
{
	public abstract class CItem : Component
	{
		public abstract ItemType type { get; }

		public ItemState state = ItemState.Dropped;

		public Texture currentSprite = null;
		public CPlayer player = null;

		public Texture sprite;
		public Sound pickupSound;
		public Sound dropSound;

		CRigidbody rb = null;

		public override void Init()
		{
			base.Init();

			entity.AddTag("Pickupable");

			sprite = GetAsset<Texture>("Sprite");
			pickupSound = GetAsset<Sound>("Pickup");
			dropSound = GetAsset<Sound>("Drop");

			currentSprite = sprite;

			rb = entity.GetComponent<CRigidbody>();
		}

		public virtual void Pickup(CPlayer player)
		{
			entity.RemoveTag("Pickupable");
			state = ItemState.Held;

			this.player = player;
			player.Pickup(this);

			pickupSound.Play(entity.position);
		}

		public virtual void Use() { }

		public virtual void Drop()
		{
			entity.AddTag("Pickupable");
			state = ItemState.Dropped;

			player.Pickup(null);
			player = null;

			dropSound.Play(entity.position);
		}

		public override void Draw()
		{
			base.Draw();

			if (currentSprite is object)
				Draw(currentSprite);
		}

		public T GetAsset<T>(string path) where T : class
		{
			T asset = null;

			asset = Assets.GetAsset<T>($"Items/{GetType().ToString().Split('.').Last().Remove(0, 1)}/{path}");

			if (asset is null)
			{
				asset = Assets.GetAsset<T>($"Items/_Default/{path}");

				if (asset is null)
				{
					asset = Assets.GetAsset<T>($"Items/_Default/Error");
					if (asset is null) throw new System.Exception($"Could not find an error asset for {typeof(T)} at {path}");
					else LogError($"Used error asset for {path}");
				}
			}

			return asset;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (player is object) rb.position = player.entity.position;
		}
	}
}