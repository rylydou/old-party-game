using MGE;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.Components.UI
{
	public class CUIHUD : Component
	{
		public Vector2 offset = new Vector2(8.0);

		public double heartSize = 64;

		int maxHealth = 5;
		int health = 3;
		int extraHealth = 1;

		SpriteSheet hearts;

		public override void Init()
		{
			hearts = Assets.GetAsset<SpriteSheet>("Sprites/UI/Hearts");
		}

		public override void Draw()
		{
			using (new DrawBatch(transform: null))
			{
				for (int i = 0; i < maxHealth; i++)
				{
					if (i < health)
						hearts.Draw("Normal", new Rect(offset + new Vector2(i * heartSize, 0), new Vector2(heartSize)), Color.white);
					else
						hearts.Draw("Empty", new Rect(offset + new Vector2(i * heartSize, 0), new Vector2(heartSize)), Color.white);
				}

				for (int i = 0; i < extraHealth; i++)
				{
					hearts.Draw("Extra", new Rect(offset + new Vector2((maxHealth + i) * heartSize, 0), new Vector2(heartSize)), Color.white);
				}
			}
		}
	}
}