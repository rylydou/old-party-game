using Microsoft.Xna.Framework.Graphics;
using MGE;
using MGE.Graphics;
using MGE.ECS;

namespace GAME.Components
{
	public class Background : Component
	{
		Texture2D background;

		public override void Init()
		{
			background = Assets.GetAsset<Texture2D>("Sprites/Background");
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				sb.Draw(background, new Rect(Vector2.zero, Window.gameSize), Color.white);
			}
		}
	}
}