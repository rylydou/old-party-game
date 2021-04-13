using MGE.ECS;

namespace GAME
{
	public abstract class CInteractable : Component
	{
		public override void Init()
		{
			base.Init();

			entity.AddTag("Interactable");
		}

		public abstract void Interact(Entity player);
	}
}