using GAME.Components;
using GAME.Components.Items;
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
						new Entity(new CRigidbody(), new CCrate()),
						new Entity(new CRigidbody(), new CPlayer())
					)
				)
			);

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