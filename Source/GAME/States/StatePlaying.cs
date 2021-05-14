using GAME.Components;
using GAME.Components.Items;
using GAME.Components.UI;
using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.States
{
	public class StatePlaying : GameState
	{
		public float timeLeft;

		public float crateSpawnCooldown;

		public override void Init()
		{
			base.Init();

			SceneManager.QueueScene(
				new Scene(
					new Layer(
						false,
						"Background",
						new Entity(new CBackground())
					),
					new Layer(
						false,
						"Gameplay",
						new Entity(new CStage())
					),
					new Layer(
						false,
						"Effects",
						new Entity(new CFog())
					),
					new Layer(
						true,
						"UI",
						new Entity(new CUI_InGameUI())
					)
				)
			);

			foreach (var player in GameSettings.current.players)
			{
				if (player is null) continue;

				player.player = new CPlayer(player);

				SpawnPlayer(player.player);
			}
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

			foreach (var player in GameSettings.current.players)
			{
				if (player.player.health < 1)
				{
					player.timeRespawing += Time.fixedDeltaTime;

					if (player.timeRespawing > Player.timeToRespawn)
					{
						player.timeRespawing = 0;
						player.player = new CPlayer(player);

						SpawnPlayer(player.player);
					}
				}
			}

			timeLeft -= Time.fixedDeltaTime;
		}

		public void SpawnPlayer(CPlayer player)
		{
			SceneManager.activeScene.layers[1].AddEntity(new Entity(new CRigidbody(), player));
		}

		public void SpawnCrate()
		{
			SceneManager.activeScene.layers[1].AddEntity(new Entity(new CRigidbody(), new CCrate()));
		}
	}
}