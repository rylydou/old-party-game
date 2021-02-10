using Microsoft.Xna.Framework.Graphics;
using MGE.Graphics;

namespace MGE.UI.Elements
{
	public class GUIImage : GUIElement
	{
		public Texture2D texture;

		public GUIImage(Texture2D texture = null)
		{
			if (texture == null)
				texture = Graphics.Graphics.pixel;

			this.texture = texture;
		}

		public override void Draw()
		{
			Graphics.Graphics.Draw(texture, rect, color);
		}
	}
}