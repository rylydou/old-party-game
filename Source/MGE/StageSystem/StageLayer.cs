using System;
using MGE.Components;
using MGE.UI;

namespace MGE.StageSystem
{
	[System.Serializable]
	public abstract class StageLayer
	{
		public string name;

		[NonSerialized] public bool isVisible = true;

		public StageLayer()
		{
			name = "A Layer With No Name (Very Sad)";
		}

		public void Init(Vector2Int size)
		{
			OnInit(size);
		}

		protected abstract void OnInit(Vector2Int size);

		public virtual void Update(ref GUI gui) { }
		public virtual void Draw(Vector2 pan, float zoom) { }

		protected Vector2 Scale(Vector2 vector) => CEditor.Scale(vector);
		protected Rect Scale(Rect rect) => CEditor.Scale(rect);
	}
}