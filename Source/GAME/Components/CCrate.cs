using MGE;
using MGE.Components;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.Components
{
	public class CCrate : Component
	{
		static Texture texture;

		CRigidbody rb;

		public override void Init()
		{
			if (texture is null) texture = Assets.GetAsset<Texture>("Sprites/Crate");

			rb = entity.GetComponent<CRigidbody>();

			rb.position = new Vector2(Random.Int(1, Window.gameSize.x - 1), 1);

			rb.raycaster = CStage.current;
		}

		public override void Draw()
		{
			GFX.Draw(texture, entity.position, Color.white);
		}
	}
}