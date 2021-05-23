using System.Collections.Generic;
using System.Linq;
using GAME.Components;
using GAME.Components.Items;
using GAME.UI;
using MGE;
using MGE.Components;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.States
{
	public class StatePlaying : GameState
	{
		public Menu pauseMenu = new Menu(
			"PAUSED",
			(() => "Resume", () => PAUSED = !PAUSED),
			(() => "Restart", () => Main.current.ChangeState(new StatePlaying())),
			(() => "Quit", () => Main.current.ChangeState(new StatePlayerSetup()))
		);

		public static bool PAUSED = false;

		public float timeLeft;

		float crateSpawnCooldown;

		public override void Init()
		{
			base.Init();

			PAUSED = false;

			SceneManager.QueueScene(
				new Scene(
					new Layer(
						"Background",
						new Entity(new CBackground())
					),
					new Layer(
						"Gameplay",
						new Entity(new CStage())
					),
					new Layer(
						"Effects",
						new Entity(new CFog())
					)
				)
			);

			foreach (var player in GameSettings.players)
			{
				if (player is null) continue;

				player.player = new CPlayer(player);

				SceneManager.activeScene.GetLayer("Gameplay").AddEntity(new Entity(new CRigidbody(), player.player));

				SpawnCrate();
			}

			timeLeft = GameSettings.current.roundTime;
		}

		public override void Tick()
		{
			base.Tick();

			crateSpawnCooldown -= Time.fixedDeltaTime;
			if (crateSpawnCooldown < 0)
			{
				crateSpawnCooldown = GameSettings.current.timeBtwCrates;
				SpawnCrate();
			}

			foreach (var player in GameSettings.players)
			{
				if (player.player.entity.enabled == false)
				{
					if (timeLeft > 0)
						player.player.timeRespawing += Time.fixedDeltaTime;

					if (player.player.timeRespawing > GameSettings.current.timeToRespawn)
					{
						player.player.Start();
					}
				}
			}

			if (!Main.current.infiniteTime)
			{
				timeLeft -= Time.fixedDeltaTime;

				if (
					timeLeft < -GameSettings.current.maxOvertime ||
					(timeLeft < 0 && GameSettings.players.Count - GameSettings.players.Count(x => x.player.health < 1) <= 1)
				)
				{
					Main.current.ChangeState(new StatePlayerSetup());
				}
			}
		}

		public override void Update()
		{
			base.Update();

			if (GameSettings.mainController is object && GameSettings.mainController.pause)
			{
				PAUSED = !PAUSED;

				MenuManager.menus = new List<Menu>() { pauseMenu };
				MenuManager.menus[0].onClose = (m) => PAUSED = false;
			}

			if (PAUSED)
			{
				MenuManager.Update();
				Time.timeScale = 0.0f;
			}
			else
			{
				Time.timeScale = 1.0f;
			}
		}

		public override void DrawUI()
		{
			base.DrawUI();

			if (PAUSED)
			{
				MenuManager.Draw();
			}
			else
			{
				var index = 0;

				foreach (var player in GameSettings.players)
				{
					if (player is null) continue;

					var padding = new Vector2(16);

					var offset = index * 96;

					var healthFill =
						player.player.timeRespawing > 0 ? player.player.timeRespawing / GameSettings.current.timeToRespawn * player.player.maxHealth :
						player.player.lastHealth;

					var healthChangeColorAt = player.player.timeRespawing > 0 ? player.player.maxHealth : player.player.health;

					for (int i = 0; i < healthFill; i++)
					{
						GFX.DrawLine(new Vector2(42 + offset, 42) + padding, 42, i > healthChangeColorAt ? Color.white : player.color, Math.pi2 - ((float)i / player.player.maxHealth * Math.pi2 + Math.piOver2), 3f);
					}

					GFX.DrawCircle(new Vector2(42 + offset, 42) + 2 + padding, 44, new Color(0, 0.25f), 5, 32);
					GFX.DrawCircle(new Vector2(42 + offset, 42) + padding, 44, new Color(Math.Round(player.color.inverted.grayscale)), 5, 32);

					var iconOffset = padding + (float)(42 * 2 - 42) / 2 + new Vector2(offset, 0) + (player.player.hitFlash > 0 ? Random.UnitVector() * 8 : Vector2.zero);

					for (int y = -1; y <= 1; y++)
						for (int x = -1; x <= 1; x++)
							GFX.Draw(player.player.health < 1 ? player.iconDead : player.icon, new Rect(iconOffset + new Vector2(x, y), 42, 42), Color.black);

					GFX.Draw(player.player.health < 1 ? player.iconDead : player.icon, new Rect(iconOffset, 42, 42), player.controls.isConnected ? Color.white : Color.gray);

					var killsText = player.kills.ToString();
					var killsTextOffset = (42 * 2 - Config.font.Measure(killsText).x) / 2 + padding.x;

					for (int y = -2; y <= 2; y++)
						for (int x = -2; x <= 2; x++)
							Config.font.DrawText(killsText, new Vector2(killsTextOffset + offset + x, padding.y + 42 * 1.5f + y), new Color(0, 0.0125f));

					Config.font.DrawText(killsText, new Vector2(killsTextOffset + offset, padding.y + 42 * 1.5f), Color.white);

					index++;
				}

				if (!Main.current.infiniteTime)
				{
					var timeText =
						(timeLeft < 0 ? "OVERTIME! " : "") +
						System.TimeSpan.FromSeconds(timeLeft.Abs()).ToString(timeLeft.Abs() < 60 ? @"ss\.ff" : @"mm\:ss");
					var timeTextSize = Config.font.Measure(timeText, 2);
					var timeTextOffset = (Window.renderSize.x - timeTextSize.x) / 2;

					for (int y = -4; y <= 4; y++)
						for (int x = -4; x <= 4; x++)
							Config.font.DrawText(timeText, new Vector2(timeTextOffset + x, 8 + y), new Color(0, 0.0125f), 2);

					Config.font.DrawText(timeText, new Vector2(timeTextOffset, 8), timeLeft < 30 ? new Color(Math.Sin(Time.time * Math.pi).Abs() * 0.5f + 0.5f, 0, 0) : Color.white, 2);
				}
			}
		}

		public void SpawnPlayer(CPlayer player)
		{
			SceneManager.activeScene.GetLayer("Gameplay").AddEntity(new Entity(new CRigidbody(), player));
		}

		public void SpawnCrate()
		{
			SceneManager.activeScene.GetLayer("Gameplay").AddEntity(new Entity(new CRigidbody(), new CCrate()));
		}
	}
}