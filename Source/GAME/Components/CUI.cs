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

				GFX.DrawCircle(new Vector2(42 + 32 + 2 + offset, 42 + 32 + 2), 44, new Color(0, 0.25f), 5, 32);

				GFX.DrawCircle(new Vector2(42 + 32 + offset, 42 + 32), 44, new Color(Math.Round(player.color.inverted.grayscale)), 5, 32);

				var iconOffset = (float)(42 * 2 - 42) / 2 + 32;

				GFX.Draw(player.player.health < 1 ? player.iconDead : player.icon, new Rect(iconOffset + offset + 2, iconOffset + 2, 42, 42), new Color(0, 0.25f));

				GFX.Draw(player.player.health < 1 ? player.iconDead : player.icon, new Rect(iconOffset + offset, iconOffset, 42, 42));

				var text = player.deaths.ToString();
				var textSize = Config.font.Measure(text);
				var textDeathsOffset = (96 - textSize.x) / 2 * 1.625f;

				Config.font.DrawText(text, new Vector2(textDeathsOffset + offset + 2, 96), new Color(0, 0.25f));
				Config.font.DrawText(text, new Vector2(textDeathsOffset + offset, 96 - 2), Color.red);

				index++;
			}

			var time = System.TimeSpan.FromSeconds(Main.current.timeLeft).ToString(Main.current.timeLeft < 60 ? @"ss\.ff" : @"mm\:ss");
			var size = Config.font.Measure(time, 2);
			var textOffset = (Window.renderSize.x - size.x) / 2;

			for (int y = -2; y <= 2; y++)
			{
				for (int x = -2; x <= 2; x++)
				{
					Config.font.DrawText(time, new Vector2(textOffset - 8 + x, 8 + y), new Color(0, 0.025f), 2);
				}
			}

			Config.font.DrawText(time, new Vector2(textOffset - 8, 8), Main.current.timeLeft < 30 ? Color.red : Color.white, 2);
		}
	}
}