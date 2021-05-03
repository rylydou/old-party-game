using MGE;
using MGE.ECS;

namespace GAME
{
	public struct Damage
	{
		public Entity doneBy;
		public Vector2 origin;
		public int damage;
		public float knockback;

		public Damage(int damage, float knockback)
		{
			this.doneBy = null;
			this.origin = Vector2.zero;
			this.damage = damage;
			this.knockback = knockback;
		}
	}
}