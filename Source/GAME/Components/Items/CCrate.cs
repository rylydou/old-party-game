using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CCrate : CItem
	{
		public static System.Type[] lootTable = new System.Type[]
		{
			typeof(CShotgun),
			typeof(CBananaGun),
			typeof(CRock),
			typeof(CHamburger),
			// typeof(CPearl),
			// typeof(CCursedPearl),
			typeof(CFishingRod),
		};

		public CItem item;

		public override void Init()
		{
			base.Init();

			var itemType = lootTable.Random();

			item = (CItem)System.Activator.CreateInstance(itemType);

			rb.position = new Vector2(Random.Float(4, Window.sceneSize.x - 4), -1);
		}

		public override void Death()
		{
			var newRB = new CRigidbody();
			newRB.velocity = rb.velocity;
			Spawn(new Entity(newRB, item), entity.position);

			base.Death();
		}
	}
}