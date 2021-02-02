using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MGE.Graphics;

namespace MGE
{
	public abstract class EssentialVars
	{
		protected static GameWindow window { get => Engine.game.Window; }

		protected static GraphicsDeviceManager graphics { get => Engine.current.graphics; }
		protected static GraphicsDevice graphicsDevice { get => Engine.game.GraphicsDevice; }
		protected static SpriteBatch sb { get => Engine.current.sb; }
		protected static Camera camera { get => Engine.current.camera; }
		protected static SpriteFont font { get => MGEConfig.defualtFont; }
	}
}