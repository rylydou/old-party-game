using System.Collections.Generic;
using MGE.StageSystem.Layers;

namespace MGE.StageSystem
{
	[System.Serializable]
	public class Stage
	{
		public Vector2Int size = new Vector2Int(64);
		public List<StageLayer> layers = new List<StageLayer>() { };

		public Stage()
		{
			AddLayer(new IntLayer());
		}

		public void AddLayer(StageLayer layer)
		{
			layer.Init(size);
			layers.Add(layer);
		}
	}
}