namespace MGE.ECS
{
	public abstract class Component : EssentialVars
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
	}
}