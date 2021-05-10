using MGE;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.Components.UI
{
	public class CUI_InGameUI : Component
	{
		public override void Draw()
		{
			base.Draw();

			var index = 0;

			foreach (var player in GameSettings.current.players)
			{
				if (player is null) continue;

				var padding = new Vector2(16);

				var offset = index * 96;

				var healthFill =
					player.timeRespawing > 0 ? Math.RoundToInt(player.timeRespawing / Player.timeToRespawn * player.player.maxHealth) :
					Math.RoundToInt(player.player.lastHealth);

				var healthChangeColorAt = player.timeRespawing > 0 ? player.player.maxHealth : player.player.health;

				for (int i = 0; i < healthFill; i++)
				{
					GFX.DrawLine(new Vector2(42 + offset, 42) + padding, 42, i > healthChangeColorAt ? Color.white : player.color, Math.pi2 - ((float)i / player.player.maxHealth * Math.pi2 + Math.piOver2), 3f);
				}

				GFX.DrawCircle(new Vector2(42 + offset, 42) + 2 + padding, 44, new Color(0, 0.25f), 5, 32);
				GFX.DrawCircle(new Vector2(42 + offset, 42) + padding, 44, new Color(Math.Round(player.color.inverted.grayscale)), 5, 32);

				var iconOffset = padding + (float)(42 * 2 - 42) / 2 + new Vector2(offset, 0) + (player.player.hitFlash > 0 ? Random.UnitVector() * 8 : Vector2.zero);

				GFX.Draw(player.player.health < 1 ? player.iconDead : player.icon, new Rect(iconOffset + 2, 42, 42), new Color(0, 0.25f));

				GFX.Draw(player.player.health < 1 ? player.iconDead : player.icon, new Rect(iconOffset, 42, 42), player.controls.isConnected ? Color.white : Color.gray);

				var killsText = player.kills.ToString();
				var killsTextOffset = (42 * 2 - Config.font.Measure(killsText).x) / 2 + padding.x;

				for (int y = -2; y <= 2; y++)
					for (int x = -2; x <= 2; x++)
						Config.font.DrawText(killsText, new Vector2(killsTextOffset + offset + x, padding.y + 42 * 1.5f + y), new Color(0, 0.0125f));

				Config.font.DrawText(killsText, new Vector2(killsTextOffset + offset, padding.y + 42 * 1.5f), Color.white);

				index++;
			}

			// var timeText =
			// 	(GameSettings.current.timeLeft < 0 ? "OVERTIME! " : "") +
			// 	System.TimeSpan.FromSeconds(Main.current.timeLeft.Abs()).ToString(GameSettings.current.timeLeft.Abs() < 60 ? @"ss\.ff" : @"mm\:ss");
			// var timeTextSize = Config.font.Measure(timeText, 2);
			// var timeTextOffset = (Window.renderSize.x - timeTextSize.x) / 2;

			// for (int y = -4; y <= 4; y++)
			// 	for (int x = -4; x <= 4; x++)
			// 		Config.font.DrawText(timeText, new Vector2(timeTextOffset - 8 + x, 8 + y), new Color(0, 0.0125f), 2);

			// Config.font.DrawText(timeText, new Vector2(timeTextOffset - 8, 8), GameSettings.current.timeLeft < 30 ? new Color(Math.Sin(Time.time * Math.pi).Abs() * 0.5f + 0.5f, 0, 0) : Color.white, 2);
		}
	}
}