using MGE.Graphics;

namespace MGE.ECS
{
	[System.Serializable]
	public abstract class Component
	{
		public bool enabled = true;
		public bool visible = true;

		public Entity entity = null;

		public bool inited = false;

		public virtual void Init() { }
		public virtual void FixedUpdate() { }
		public virtual void Update() { }
		public virtual void Draw() { }
		public virtual void Destroy() { }

		public virtual void Draw(Texture texture, Vector2 position = default, Color? color = null)
		{
			GFX.Draw(texture, entity.position + position, color);
		}

		protected virtual void Log(object message)
		{
			Logger.Log($"{this.ToString()} - {message}");
		}

		protected virtual void LogWarning(object message)
		{
			Logger.LogWarning($"{this.ToString()} - {message}");
		}

		protected virtual void LogError(object message)
		{
			Logger.LogError($"{this.ToString()} - {message}");
		}
	}
}