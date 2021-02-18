using System.Collections.Generic;
using MGE;
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
			new SceneManager(
				new Scene(new List<Layer>()
				{
					new Layer(new List<Entity>()
					{
						new Entity(new List<Component>(){new GAME.Components.CBackground()}),
						new Entity(new List<Component>(){new GAME.Components.CPlayer()}),
						new Entity(new List<Component>(){new GAME.Components.CWorld()}),
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