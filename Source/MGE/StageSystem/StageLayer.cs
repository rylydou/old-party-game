using MGE.Components;
using MGE.UI;

namespace MGE.StageSystem
{
	[System.Serializable]
	public abstract class StageLayer
	{
		public const float itemSize = 32;
		public const float offset = 8;

		public string name;

		public bool isVisible = true;

		public Stage stage { get; internal set; }

		public StageLayer()
		{
			name = "A Layer With No Name (Very Sad)";
		}

		public void Init()
		{
			Editor_Create();
		}

		public virtual void OnDeserilize()
		{
			isVisible = true;
		}

		protected abstract void Editor_Create();
		public virtual void Editor_Update(ref GUI gui) { }
		public virtual void Editor_Draw(Vector2 pan, float zoom) { }

		protected virtual void Game_Init() { }
		public virtual void Game_Update() { }
		public virtual void Game_Draw() { }

		protected Vector2 Scale(Vector2 vector) => CEditor.Scale(vector);
		protected Rect Scale(Rect rect) => CEditor.Scale(rect);
	}
}