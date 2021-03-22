using MGE.Graphics;

namespace MGE.UI.Elements
{
	public class GUIText : GUIElement
	{
		public string text;
		public float scale;
		public TextAlignment alignment;

		public GUIText(string text)
		{
			this.text = text;
		}

		public override void Draw()
		{
			Config.font.DrawText(text, rect, color, scale, alignment);
		}
	}
}