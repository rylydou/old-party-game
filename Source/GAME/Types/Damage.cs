using MGE;

namespace GAME
{
	public struct Damage
	{
		public Vector2 origin;
		public int damage;
		public float knockback;

		public Damage(Vector2 origin, int damage, float knockback)
		{
			this.origin = origin;
			this.damage = damage;
			this.knockback = knockback;
		}
	}
}