using MGE;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.Components
{
	public class CUI : Component
	{
		public override void Draw()
		{
			base.Draw();

			int index = 0;

			foreach (var player in Main.current.players)
			{
				var offset = player.index * 96;

				for (int i = 0; i < player.player.health; i++)
				{
					GFX.DrawLine(new Vector2(42 + 16 + offset, 42 + 16), 42, player.color, Math.pi2 - ((float)i / player.player.maxHealth * Math.pi2 + Math.piOver2), 3f);
				}

				GFX.DrawCircle(new Vector2(42 + 16 + offset, 42 + 16), 44, new Color(Math.Round(player.color.inverted.grayscale)), 5, 32);

				var iconOffset = (float)(42 * 2 - 42) / 2 + 16;

				GFX.Draw(player.icon, new Rect(iconOffset + offset, iconOffset, 42, 42));

				index++;
			}
		}
	}
}