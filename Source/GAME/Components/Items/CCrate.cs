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
		};

		public override bool meleeOnly => false;

		public CItem item;

		public override void Init()
		{
			maxHealth = 30;

			base.Init();

			var itemType = lootTable.Random();

			item = (CItem)System.Activator.CreateInstance(itemType);

			rb.position = new Vector2(Random.Float(1, Window.sceneSize.x - 1), 1);
		}

		public override void OnDeath()
		{
			var newRB = new CRigidbody();
			newRB.velocity = rb.velocity;
			Spawn(new Entity(newRB, item), entity.position);

			base.OnDeath();
		}
	}
}