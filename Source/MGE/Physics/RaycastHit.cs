namespace MGE.Physics
{
	public class RaycastHit
	{
		public Vector2 origin;

		public double distance;
		public Vector2 direction;

		Vector2 _normal;

		public Vector2 normal
		{
			get
			{
				if (_normal.sqrMagnitude > 1.0)
					_normal = ((Vector2)_normal).normalized;
				return _normal;
			}
		}

		public Vector2 position { get => origin + direction * distance; }

		public RaycastHit(Vector2 normal)
		{
			_normal = normal;
		}
	}
}