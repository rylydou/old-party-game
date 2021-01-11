using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MGE.Graphics;

namespace MGE
{
	/// <summary>
	/// Only derive, never instance
	/// </summary>
	public abstract class EssentialVars
	{
		protected static GameWindow window { get => Main.current.Window; }

		protected static GraphicsDeviceManager graphics { get => Main.current.graphics; }
		protected static GraphicsDevice graphicsDevice { get => Main.current.GraphicsDevice; }
		protected static SpriteBatch sb { get => Main.current.sb; }
		protected static Camera camera { get => Main.current.camera; }
	}
}