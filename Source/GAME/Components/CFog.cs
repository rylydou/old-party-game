using MGE;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.Components
{
	public class CFog : Component
	{
		public int detail = 4;

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

			for (int i = -detail * 2; i < Window.sceneSize.x * detail; i++)
			{
				GFX.Draw(texDither, new Vector2((float)i / detail, Window.sceneSize.y - 0.75f - noise.GetNoise(Time.time * 6, (float)i / ((float)Window.sceneSize.x / 32)).Abs()), new Color(0.95f, 0.75f));
			}
		}
	}
}