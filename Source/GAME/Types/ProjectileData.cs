namespace GAME
{
	public struct ProjectileData
	{
		public Damage damage;
		public float speed;
		public float radius;
		public readonly float startingLifetime;
		public float lifetime;

		public ProjectileData(Damage damage, float speed, float radius, float lifetime)
		{
			this.damage = damage;
			this.speed = speed;
			this.radius = radius;
			this.startingLifetime = lifetime;
			this.lifetime = lifetime;
		}
	}
}