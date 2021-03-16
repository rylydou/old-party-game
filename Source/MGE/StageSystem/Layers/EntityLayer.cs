using System.Collections.Generic;
using MGE.ECS;

namespace MGE.StageSystem.Layers
{
	[System.Serializable]
	public class EntityLayer : StageLayer
	{
		public List<Entity> entities = new List<Entity>();

		public EntityLayer() { }
	}
}