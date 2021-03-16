using MGE.Components;

namespace MGE.StageSystem
{
	[System.Serializable]
	public abstract class StageLayer
	{
		public abstract string typeId { get; }

		public StageLayer() { }

		public virtual void Draw(Vector2 pan, float zoom) { }

		protected Vector2 Scale(Vector2 vector) => CEditor.Scale(vector);
		protected Rect Scale(Rect rect) => CEditor.Scale(rect);
	}
}