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
			typeof(CPlungerGun),
			typeof(CMinigun),
			typeof(CRifle),
		};

		public CItem item;

		public override void Init()
		{
			base.Init();

			var itemType = lootTable.Random();

			item = (CItem)System.Activator.CreateInstance(itemType);

			if (GameSettings.current.stage.crateSpawns.Count < 1)
			{
				rb.position.x = Random.Float(Window.sceneSize.x - 1);
				rb.position.y = -2;
			}
			else
			{
				var spawnPos = GameSettings.current.stage.crateSpawns.Random();

				if (spawnPos.y > 0)
				{
					rb.position = spawnPos;
				}
				else
				{
					rb.position.x = spawnPos.x;
					rb.position.y = -2;
				}
			}
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