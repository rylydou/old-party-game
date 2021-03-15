using MGE.ECS;

namespace MGE.StageSystem.Layers
{
	public class EntityLayer : StageLayer
	{
		public SafeList<Entity> entities;

		public EntityLayer() { }
	}
}