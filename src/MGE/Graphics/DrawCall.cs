using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.Graphics
{
	public struct DrawCall
	{
		public Texture2D sprite;
		public Color color;
		public Rect rect;
		public Vector2 center;
		public Effect effect;

		public DrawCall(Texture2D sprite, Color color, Rect rect, Vector2 center, Effect effect = null)
		{
			this.sprite = sprite;
			this.color = color;
			this.center = center;
			this.rect = rect;
			this.effect = effect;
		}
	}
}