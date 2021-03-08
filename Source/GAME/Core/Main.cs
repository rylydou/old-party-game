using System.Collections.Generic;
using GAME.Components;
using MGE;
using MGE.Debug;
using MGE.Debug.Menus;
using MGE.ECS;
using Microsoft.Xna.Framework;

namespace GAME
{
	public class Main : Microsoft.Xna.Framework.Game
	{
		public Engine engine;

		public Main()
		{
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
				new Scene(new List<Layer>()
				{
					new Layer(new List<Entity>()
					{
						new Entity(new List<Component>(){new CBackground()}),
						// new Entity(new List<Component>(){new CWorld()}),
						new Entity(new List<Component>(){new CStage()}),
						new Entity(new List<Component>(){new CPlayer()}),
					})
				})
			);

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
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			engine.Draw(gameTime);
			base.Draw(gameTime);
		}
	}
}