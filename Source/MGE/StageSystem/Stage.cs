using System.Collections.Generic;
using MGE.StageSystem.Layers;

namespace MGE.StageSystem
{
	[System.Serializable]
	public class Stage : ISerializable
	{
		public float tileSize = 16;

		public Vector2Int size = new Vector2Int(64);
		public List<StageLayer> layers = new List<StageLayer>() { };

		public Stage()
		{
			AddLayer(new IntLayer());
		}

		public void AddLayer(StageLayer layer)
		{
			layer.stage = this;
			layer.Init();
			layers.Add(layer);
		}

		public void OnBeforeSerilize() { }

		public void OnAfterSerilize() { }

		public void OnAfterDeserilize()
		{
			foreach (var layer in layers)
			{
				layer.OnDeserilize();
			}
		}
	}
}