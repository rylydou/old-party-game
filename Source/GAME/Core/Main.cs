using System.Collections.Generic;
using GAME.Components;
using GAME.Components.Items;
using GAME.Types;
using MGE;
using MGE.Components;
using MGE.ECS;

using XNA_Game = Microsoft.Xna.Framework.Game;
using XNA_GameTime = Microsoft.Xna.Framework.GameTime;

namespace GAME
{
	public class Main : XNA_Game
	{
		public static Main current { get; private set; }

		public Engine engine;

		public List<Player> players = new List<Player>
		{
			new Player(-2, "Goose"),
			new Player(-1, "_Default"),
			new Player(0, "Amogus"),
			// new Player(1, "Robot"),
		};

		public float roundTime = 60 * 3;
		public float timeLeft;

		public float timeBtwCrates = 6.0f;
		public float crateSpawnCooldown;

		public Main()
		{
			current = this;

			engine = new Engine(this);
		}

		protected override void Initialize()
		{
			engine.Initialize();
			base.Initialize();
		}

		protected override void LoadContent()
		{
			engine.LoadContent();

			SceneManager.QueueScene(
				new Scene(
					new Layer(
						false,
						new Entity(new CBackground())
					),
					new Layer(
						false,
						new Entity(new CStage())
					),
					new Layer(
						true,
						new Entity(new CUI())
					)
				)
			);

			timeLeft = roundTime;

			foreach (var player in players)
			{
				if (player is null) continue;

				player.player = new CPlayer(player);

				SceneManager.activeScene.layers[1].AddEntity(new Entity(new CRigidbody(), player.player));
			}

			SceneManager.activeScene.clearScreen = false;
#if INDEV
			SceneManager.activeScene.clearScreen = true;
#endif

			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			engine.UnloadContent();
			base.UnloadContent();
		}

		protected override void Update(XNA_GameTime gameTime)
		{
			timeLeft -= Time.fixedDeltaTime;

			crateSpawnCooldown -= Time.fixedDeltaTime;
			if (crateSpawnCooldown < 0)
			{
				crateSpawnCooldown = timeBtwCrates;
				SceneManager.activeScene.layers[1].AddEntity(new Entity(new CRigidbody(), new CCrate()));
			}

			foreach (var player in players)
			{
				if (player is null) continue;

				if (player.player.health < 1)
				{
					player.timeRespawing += Time.fixedDeltaTime;

					if (player.timeRespawing > Player.timeToRespawn)
					{
						player.timeRespawing = 0;
						player.player = new CPlayer(player);

						SceneManager.activeScene.layers[1].AddEntity(new Entity(new CRigidbody(), player.player));
					}
				}
			}

			engine.Update(gameTime);
#if INDEV
			SceneManager.activeScene.screenClearColor = MGE.Color.AnimColor(0.75f);
#endif
			base.Update(gameTime);
		}

		protected override void Draw(XNA_GameTime gameTime)
		{
			engine.Draw(gameTime);
			base.Draw(gameTime);
		}
	}
}