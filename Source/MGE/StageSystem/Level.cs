using System.Collections.Generic;
using MGE.Components;

namespace MGE.StageSystem
{
	[System.Serializable]
	public class Level
	{
		public string name;

		public List<LevelLayer> layers;

		public Vector2Int position { get; internal set; }
		public World world { get; internal set; }

		public Level()
		{
			this.layers = new List<LevelLayer>();
		}

		public void LayerAdd(LevelLayer layer)
		{
			layer.level = this;

			layer.Init();
			layers.Add(layer);
		}

		public void Log(string message)
		{
			CEditor.current.Log(message);
		}

		public void LogWarning(string message)
		{
			CEditor.current.LogWarning(message);
		}

		public void LogError(string message)
		{
			CEditor.current.LogError(message);
		}
	}
}