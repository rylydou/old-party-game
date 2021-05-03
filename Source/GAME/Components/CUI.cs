using GAME.Types;
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

			var index = 0;

			foreach (var player in Main.current.players)
			{
				var offset = player.index * 96;

				var fill = player.timeRespawing > 0 ? Math.RoundToInt(player.timeRespawing / Player.timeToRespawn * 100) : player.player.health;

				for (int i = 0; i < fill; i++)
				{
					GFX.DrawLine(new Vector2(42 + 32 + offset, 42 + 32), 42, player.color, Math.pi2 - ((float)i / player.player.maxHealth * Math.pi2 + Math.piOver2), 3f);
				}

				GFX.DrawCircle(new Vector2(42 + 32 + offset, 42 + 32), 44, new Color(Math.Round(player.color.inverted.grayscale)), 5, 32);

				var iconOffset = (float)(42 * 2 - 42) / 2 + 32;

				GFX.Draw(player.icon, new Rect(iconOffset + offset, iconOffset, 42, 42));

				index++;
			}

			var time = System.TimeSpan.FromSeconds(Main.current.timeLeft).ToString(Main.current.timeLeft < 60 ? @"ss\.ff" : @"mm\:ss");
			var size = Config.font.Measure(time, 1.5f);

			Config.font.DrawText(time, new Vector2(Window.renderSize.x - size.x - 6, 10), new Color(0, 0.25f), 1.5f);
			Config.font.DrawText(time, new Vector2(Window.renderSize.x - size.x - 8, 8), Main.current.timeLeft < 30 ? new Color(0.9f, 0.05f, 0.05f) : new Color(0.95f), 1.5f);
		}
	}
}