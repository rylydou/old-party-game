using MGE.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.UI.Elements
{
	public class GUIImage : GUIElement
	{
		public Texture2D texture;

		public GUIImage()
		{
			this.texture = GFX.pixel;
		}

		public GUIImage(Texture2D texture)
		{
			this.texture = texture;
		}

		public override void Draw()
		{
			GFX.Draw(texture, rect, color);
		}
	}
}