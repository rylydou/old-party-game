using MGE.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.UI.Elements
{
	public class GUIImage : GUIElement
	{
		public Texture2D texture;

		public GUIImage(Texture2D texture = null)
		{
			this.texture = texture;
		}

		public override void Draw()
		{
			GFX.Draw(texture, rect, color);
		}
	}
}