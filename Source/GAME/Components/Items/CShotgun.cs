using MGE;
using MGE.ECS;

namespace GAME.Components.Items
{
	public class CShotgun : CItem
	{
		public override ItemType type => ItemType.Weapon;

		Sound shootSound;

		public override void Init()
		{
			base.Init();

			shootSound = GetAsset<Sound>("Shoot");
		}

		public override void Use()
		{
			base.Use();

			for (int i = -1; i <= 1; i++)
			{
				Spawn(new Entity(new Projectile()), entity.position + new Vector2(0, i * 0.1f));
			}

			shootSound.Play(entity.position, 0.1f);
		}
	}
}