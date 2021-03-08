using MGE.Graphics;

namespace MGE.UI.Elements
{
	public class GUIText : GUIElement
	{
		public string text;

		public GUIText(string text)
		{
			this.text = text;
		}

		public override void Draw()
		{
			GFX.DrawText(text, rect.position, color);
		}
	}
}