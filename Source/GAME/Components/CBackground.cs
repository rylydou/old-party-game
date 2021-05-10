using MGE;
using MGE.Graphics;
using MGE.ECS;

namespace GAME.Components
{
	public class CBackground : Component
	{
		public class Cloud
		{
			public Texture texture;
			public Vector2 position;
			public float speed;
			public Color color;
		}

		public int amountOfClouds = 8;
		public Range cloudSpeed = new Range(0.25f, 0.5f);
		public Range cloudHeight = new Range(0.0f, 7.5f);
		public Range cloudOpacity = new Range(0.9f, 1.0f);

		public Cloud[] clouds;

		public Texture[] cloudTextures;

		Texture background;

		public override void Init()
		{
			base.Init();

			background = Assets.GetAsset<Texture>("Sprites/Background");

			cloudTextures = Assets.GetAssets<Texture>("Sprites/Clouds/Cloud");

			clouds = new Cloud[amountOfClouds];

			for (int i = 0; i < clouds.Length; i++)
			{
				clouds[i] = new Cloud();
				clouds[i].position.x = Random.Float(-10, Window.sceneSize.x + 10);
				clouds[i].position.y = cloudHeight.random;
				clouds[i].speed = cloudSpeed.random * (Random.Bool() ? -1 : 1);
				clouds[i].color = new Color(1.0f, cloudOpacity.random);
				clouds[i].texture = cloudTextures.Random();
			}
		}

		public override void Update()
		{
			base.Update();

			foreach (var cloud in clouds)
			{
				if (cloud.speed < 0 && cloud.position.x < -10 || cloud.speed > 0 && cloud.position.x > Window.sceneSize.x + 10 || Math.Approximately(cloud.speed, 0))
				{
					if (Random.Bool())
					{
						cloud.position.x = -10;
						cloud.speed = cloudSpeed.random;
					}
					else
					{
						cloud.position.x = Window.sceneSize.x + 10;
						cloud.speed = -cloudSpeed.random;
					}

					cloud.position.y = cloudHeight.random;
					cloud.color = new Color(1.0f, cloudOpacity.random);
					cloud.texture = cloudTextures.Random();
				}

				cloud.position.x += cloud.speed * Time.deltaTime;
			}
		}

		public override void Draw()
		{
			base.Draw();

			GFX.Draw(background, new Rect(Camera.position, Window.sceneSize));

			foreach (var cloud in clouds)
			{
				GFX.Draw(cloud.texture, cloud.position, cloud.color);
			}
		}
	}
}