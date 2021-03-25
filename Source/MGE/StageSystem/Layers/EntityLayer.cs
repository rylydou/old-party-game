using System.Collections.Generic;
using MGE.ECS;

namespace MGE.StageSystem.Layers
{
	[System.Serializable]
	public class EntityLayer : LevelLayer
	{
		public List<Entity> entities;

		protected override void Editor_Create()
		{
			name = "Entity Layer";

			entities = new List<Entity>();
		}
	}
}