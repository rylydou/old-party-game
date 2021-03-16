using System.Collections.Generic;

namespace MGE.StageSystem
{
	[System.Serializable]
	public class Stage
	{
		public Vector2Int size = new Vector2Int(64);
		public List<StageLayer> layers = new List<StageLayer>();

		public Stage() { }
	}
}