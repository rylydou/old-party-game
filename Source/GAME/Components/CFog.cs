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

			for (int i = -GameSettings.current.stage.fogDetail * 2; i < Window.sceneSize.x * GameSettings.current.stage.fogDetail; i++)
			{
				GFX.Draw(texDither,
				new Vector2(
					(float)i / GameSettings.current.stage.fogDetail,
					Window.sceneSize.y - GameSettings.current.stage.fogHeight -
						noise.GetNoise(Time.time * GameSettings.current.stage.fogSpeed, (float)i / ((float)Window.sceneSize.x / GameSettings.current.stage.fogSize)).Abs()),
					GameSettings.current.stage.fogColor);
			}
		}
	}
}