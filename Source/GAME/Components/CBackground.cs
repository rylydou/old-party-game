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
			using (new DrawBatch(transform: null))
			{
				GFX.Draw(background, new Rect(Vector2.zero, Window.windowedSize), Color.white);
			}
		}
	}
}