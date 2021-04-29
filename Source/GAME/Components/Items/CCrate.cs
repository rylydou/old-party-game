using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CCrate : CItem
	{
		public CItem item;

		CRigidbody rb;

		public override void Init()
		{
			base.Init();

			item = new CShotgun();

			rb = entity.GetComponent<CRigidbody>();

			rb.position = new Vector2(Random.Int(1, Window.sceneSize.x - 1), 1);

			rb.raycaster = CStage.current;
		}

		public override void Pickup(CPlayer player)
		{
			entity.layer.AddEntity(new Entity(new CRigidbody(), new CCrate()));
			entity.Destroy();
		}
	}
}