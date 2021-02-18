using Microsoft.Xna.Framework;

namespace MGE
{
	public static class Time
	{
		public static double timeScale { get; set; } = 1.0;

		public static double time { get; private set; } = 0.0;
		public static double deltaTime { get; private set; } = 0.0;

		public static double unscaledTime { get; private set; } = 0.0;
		public static double unscaledDeltaTime { get; private set; } = 0.0;

		static internal void Update(GameTime gameTime)
		{
			time += gameTime.ElapsedGameTime.TotalSeconds;
			deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

			unscaledTime += gameTime.ElapsedGameTime.TotalSeconds * timeScale;
			unscaledDeltaTime = gameTime.ElapsedGameTime.TotalSeconds * timeScale;
		}
	}
}