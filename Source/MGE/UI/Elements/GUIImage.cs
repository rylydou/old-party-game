using MGE.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.UI.Elements
{
	public class GUIImage : GUIElement
	{
		public Texture texture;

		public GUIImage(Texture texture = null)
		{
			this.texture = texture;
		}

		public override void Draw()
		{
			GFX.Draw(texture, rect, color);
		}
	}
}