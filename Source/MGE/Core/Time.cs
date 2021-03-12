using Microsoft.Xna.Framework;

namespace MGE
{
	public static class Time
	{
		public static float timeScale { get; set; } = 1.0f;

		public static float time { get; private set; } = 0.0f;
		public static float deltaTime { get; private set; } = 0.0f;

		public static float unscaledTime { get; private set; } = 0.0f;
		public static float unscaledDeltaTime { get; private set; } = 0.0f;

		static internal void Update(GameTime gameTime)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds * timeScale;
			deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * timeScale;

			unscaledTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
			unscaledDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		}
	}
}