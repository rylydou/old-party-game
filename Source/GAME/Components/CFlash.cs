using MGE;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.Components
{
	public class CFlash : Component
	{
		readonly float shrinkSpeed;

		Vector2 position;
		float size;

		int framesAlive;

		public CFlash(Vector2 position, float size, float shrinkSpeed)
		{
			this.position = position;
			this.size = size;
			this.shrinkSpeed = shrinkSpeed;
		}

		public override void Update()
		{
			if (size < 0f)
				entity.layer.RemoveEntity(entity);

			size -= shrinkSpeed * (float)Time.deltaTime;

			framesAlive++;
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				if (framesAlive == 1)
					GFX.DrawBox(new Rect(position.x - size, position.y - size, size * 2, size * 2), new Color(1, 0.1f));
				else if (framesAlive == 2)
					GFX.DrawBox(new Rect(position.x - size * 2, position.y - size * 2, size * 4, size * 4), new Color(1, 0.05f));

				float innerSize = framesAlive == 1 ? size * 1.5f : size;

				GFX.DrawBox(new Rect(position.x - innerSize / 2, position.y - innerSize / 2, innerSize, innerSize), new Color(1, 1, 0, 0.75f));
			}
		}
	}
}