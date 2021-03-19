using MGE.Graphics;

namespace MGE.UI.Elements
{
	public class GUIRect : GUIElement
	{
		float thickness = 1;

		public GUIRect(float thickness)
		{
			this.thickness = thickness;
		}

		public override void Draw()
		{
			GFX.DrawRect(rect, color, thickness);
		}
	}
}