using MGE;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.Components.UI
{
	public class CUIHUD : Component
	{
		public Vector2 offset = new Vector2(8);

		public float itemSize = 32;
		public float heartSize = 32 + 16;

		Font font;
		SpriteSheet hearts;

		public override void Init()
		{
			font = Assets.GetAsset<Font>("Fonts/Basic");
			hearts = Assets.GetAsset<SpriteSheet>("Sprites/UI/Hearts");
		}

		public override void Draw()
		{
			using (new DrawBatch(transform: null))
			{
				for (int i = 0; i < Main.current.maxHealth + Main.current.extraHealth; i++)
				{
					hearts.Draw("Shadow", new Rect(offset.x + i * heartSize, offset.y, heartSize, heartSize), Color.white);
				}

				for (int i = 0; i < Main.current.maxHealth; i++)
				{
					if (i < Main.current.health)
						hearts.Draw("Normal", new Rect(offset.x + i * heartSize, offset.y, heartSize, heartSize), Color.white);
					else
						hearts.Draw("Empty", new Rect(offset.x + i * heartSize, offset.y, heartSize, heartSize), Color.white);
				}

				for (int i = 0; i < Main.current.extraHealth; i++)
				{
					hearts.Draw("Extra", new Rect(offset.x + (Main.current.maxHealth + i) * heartSize, offset.y, heartSize, heartSize), Color.white);
				}
			}

			for (int i = 0; i < Main.current.inv.Length; i++)
			{

			}
		}
	}
}