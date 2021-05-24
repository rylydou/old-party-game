using MGE;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.Components
{
	public class CFog : Component
	{
		Noise noise;

		Texture texDither;

		public override void Init()
		{
			base.Init();

			texDither = Assets.GetAsset<Texture>("Sprites/Fog");

			noise = new Noise();
			noise.noiseType = Noise.NoiseType.OpenSimplex2;
			noise.fractalType = Noise.FractalType.FBm;
			noise.octaves = 4;
			noise.gain = 0.5f;
			noise.frequency = 0.01f;
		}

		public override void Draw()
		{
			base.Draw();

			for (int i = -GameSettings.stage.fogDetail * 2; i < Window.sceneSize.x * GameSettings.stage.fogDetail; i++)
			{
				GFX.Draw(texDither,
				new Vector2(
					(float)i / GameSettings.stage.fogDetail,
					Window.sceneSize.y - GameSettings.stage.fogHeight -
					noise.GetNoise(Time.time * GameSettings.stage.fogSpeed, (float)i / ((float)Window.sceneSize.x / GameSettings.stage.fogSize)).Abs()
				),
					GameSettings.stage.fogColor);
			}
		}
	}
}