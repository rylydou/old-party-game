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

		CRigidbody rb = null;

		public override void Init()
		{
			base.Init();

			sprite = GetAsset<Texture>("Sprite");
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
		}

		public virtual void Use() { }

		public virtual void Drop()
		{
			entity.AddTag("Interactable");
			state = ItemState.Dropped;

			player = null;
		}

		public virtual void Throw()
		{
			entity.AddTag("Interactable");
			state = ItemState.Dropped;

			player = null;
		}

		public override void Draw()
		{
			base.Draw();

			if (currentSprite is object)
				Draw(currentSprite, entity.position);
		}

		public T GetAsset<T>(string path) where T : class
		{
			try
			{
				return Assets.GetAsset<T>($"Items/{GetType().ToString().Remove(0, 1)}/{path}");
			}
			catch
			{
				try
				{
					Log($"Used fallback for {path}");
					return Assets.GetAsset<T>($"Items/_Default/{path}");
				}
				catch
				{
					try
					{
						Log($"Used error for {path}");
						return Assets.GetAsset<T>($"Items/_Default/Error");
					}
					catch
					{
						throw new System.Exception($"Could not find error for {typeof(T)} at {path}");
					}
				}
			}
		}

		public override void Update()
		{
			base.Update();

			entity.position = player.entity.position;
		}
	}
}