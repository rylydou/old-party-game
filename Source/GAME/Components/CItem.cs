using System.Linq;
using MGE;
using MGE.Components;

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
}

namespace GAME.Components
{
	public abstract class CItem : CInteractable
	{
		public abstract ItemType type { get; }

		public ItemState state = ItemState.Dropped;

		public Texture currentSprite = null;
		public CPlayer player = null;

		public Texture sprite;
		public Sound pickupSound;
		public Sound dropSound;
		public Sound throwSound;

		CRigidbody rb = null;

		public override void Init()
		{
			base.Init();

			sprite = GetAsset<Texture>("Sprite");
			pickupSound = GetAsset<Sound>("Pickup");
			dropSound = GetAsset<Sound>("Drop");
			throwSound = GetAsset<Sound>("Throw");

			currentSprite = sprite;

			rb = entity.GetComponent<CRigidbody>();
		}

		public override void Interact(CPlayer player)
		{
			Pickup(player);
		}

		public virtual void Pickup(CPlayer player)
		{
			entity.RemoveTag("Interactable");
			state = ItemState.Held;

			this.player = player;
			player.Pickup(this);

			pickupSound.Play(entity.position);
		}

		public virtual void Use() { }

		public virtual void Drop()
		{
			entity.AddTag("Interactable");
			state = ItemState.Dropped;

			player.Pickup(null);
			player = null;

			dropSound.Play(entity.position);
		}

		public virtual void Throw()
		{
			entity.AddTag("Interactable");
			state = ItemState.Dropped;

			player.Pickup(null);
			player = null;

			throwSound.Play(entity.position);
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
					if (asset is null) throw new System.Exception($"Could not find error for {typeof(T)} at {path}");
				}
			}

			return asset;
		}

		public override void Update()
		{
			base.Update();

			if (player is object) rb.position = player.entity.position;
		}
	}
}