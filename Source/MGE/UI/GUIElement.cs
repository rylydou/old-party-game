namespace MGE.UI
{
	public abstract class GUIElement : EssentialVars
	{
		public Color color;
		public Rect rect;

		protected GUIElement()
		{
			this.rect = new Rect(0.0, 0.0, 128, 64);
			this.color = Color.white;
		}

		protected GUIElement(Rect rect)
		{
			this.rect = rect;
			this.color = Color.white;
		}

		protected GUIElement(Rect rect, Color color)
		{
			this.rect = rect;
			this.color = color;
		}

		public abstract void Draw();
	}
}