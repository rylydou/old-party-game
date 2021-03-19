using System.Collections.Generic;
using MGE.ECS;

namespace MGE.StageSystem.Layers
{
	[System.Serializable]
	public class EntityLayer : StageLayer
	{
		public List<Entity> entities;

		protected override void Editor_Init()
		{
			name = "Entity Layer";

			entities = new List<Entity>();
		}
	}
}