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

		public Player[] players = new Player[]
		{
			new Player(0, "_Default"),
			new Player(1, "Amogus"),
			new Player(2, "Goose"),
			new Player(3, "Robot"),
		};

		public float timeBtwCrates = 10;
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
						new Entity(new CStage()),
						new Entity(new CRigidbody(), new CCrate()),
						new Entity(new CRigidbody(), new CCrate()),
						new Entity(new CRigidbody(), new CCrate())
					),
					new Layer(
						true,
						new Entity(new CUI())
					)
				)
			);

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
			crateSpawnCooldown -= Time.fixedDeltaTime;
			if (crateSpawnCooldown < 0)
			{
				crateSpawnCooldown = timeBtwCrates;
				SceneManager.activeScene.layers[1].AddEntity(new Entity(new CRigidbody(), new CCrate()));
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