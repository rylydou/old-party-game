using MGE;
using MGE.Graphics;
using MGE.ECS;

namespace GAME.Components
{
	public class CBackground : Component
	{
		Texture background;

		public override void Init()
		{
			background = Assets.GetAsset<Texture>("Sprites/Background");
		}

		public override void Draw()
		{
			GFX.DrawDirect(background, new Rect(0, 0, Window.gameRenderSize), Color.white);
		}
	}
}