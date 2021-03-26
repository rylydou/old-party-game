using MGE;
using MGE.Components;
using MGE.ECS;
using Microsoft.Xna.Framework;

namespace GAME
{
	public class Main : Game
	{
		public static Main current { get; private set; }

		public int health = 2;
		public int extraHealth = 1;
		public int maxHealth = 3;

		public Item[] inv = new Item[10];

		public Engine engine;

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
						// new Entity(new CBackground()),
						// new Entity(new CRigidbody(), new CPlayer()),
						// new Entity(new CWorld()),
						new Entity(new CEditor())
					)/* ,
					new Layer(
						true,
						new Entity(new CUIHUD())
					) */
				)
			);

			SceneManager.activeScene.clearScreen = false;
#if Indev
			SceneManager.activeScene.clearScreen = true;
#endif

			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			engine.UnloadContent();
			base.UnloadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			engine.Update(gameTime);
			SceneManager.activeScene.screenClearColor = MGE.Color.AnimColor(0.75f);
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			engine.Draw(gameTime);
			base.Draw(gameTime);
		}
	}
}