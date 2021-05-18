using MGE;
using MGE.Components;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CCrate : CItem
	{
		public CItem item;

		public override void Init()
		{
			base.Init();

			item = (CItem)System.Activator.CreateInstance(Setup.baseCrateLootTable.Random());

			if (GameSettings.current.stage.crateSpawnsPoints.Count < 1)
			{
				rb.position.x = Random.Float(Window.sceneSize.x - 1);
				rb.position.y = -2;
			}
			else
			{
				var spawnPos = GameSettings.current.stage.crateSpawnsPoints.Random();

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