using MGE;
using MGE.Components;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.Components
{
	public class CCrate : CInteractable
	{
		static Texture texture;

		CRigidbody rb;

		public override void Init()
		{
			base.Init();

			if (texture is null) texture = Assets.GetAsset<Texture>("Sprites/Crate");

			rb = entity.GetComponent<CRigidbody>();

			rb.position = new Vector2(Random.Int(1, Window.gameSize.x - 1), 1);

			rb.raycaster = CStage.current;
		}

		public override void Draw()
		{
			base.Draw();

			GFX.Draw(texture, entity.position + new Vector2(0.1f, 0.1f), new Color(0, 0.1f));
			GFX.Draw(texture, entity.position, Color.white);
		}

		public override void Interact(Entity player)
		{
			entity.layer.AddEntity(new Entity(new CRigidbody(), new CCrate()));
			entity.Destroy();
		}
	}
}