using System;
using System.Linq;
using System.Collections.Generic;
using MGE.StageSystem.Layers;
using MGE.Components;

namespace MGE.StageSystem
{
	[System.Serializable]
	public class Level
	{
		public float tileSize;

		public Vector2Int size;
		public List<LevelLayer> layers;

		public Dictionary<string, Color> tags;

		public Vector2Int position { get; internal set; }
		public World world { get; internal set; }

		public Level()
		{
			tileSize = 16;

			size = new Vector2Int(64);

			layers = new List<LevelLayer>();
			AddLayer(new IntLayer());

			tags = new Dictionary<string, Color>();
			AddTag("Collidable", Color.green);
		}

		public void AddLayer(LevelLayer layer)
		{
			layer.level = this;
			layer.Init();
			layers.Add(layer);
		}

		public bool AddTag(string name, Color color)
		{
			if (!tags.ContainsKey(name))
			{
				tags.Add(name, color);
				return true;
			}
			return false;
		}

		public KeyValuePair<string, Color> GetTag(int index) => tags.ElementAt(index);
		public Color GetTag(string name) => tags.TryGetValue(name, Colors.error);

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