namespace MGE.Physics
{
	public class RaycastHit
	{
		public Vector2 origin;

		public float distance;
		public Vector2 direction;

		Vector2? _normal;
		public Vector2 normal
		{
			get
			{
				if (!_normal.HasValue)
					_normal = ((Vector2)_normal).normalized;
				return _normal.Value;
			}
		}

		Vector2? _position;
		public Vector2 position
		{
			get
			{
				if (!_position.HasValue)
					_position = origin + direction * distance;
				return _position.Value;
			}
		}

		public RaycastHit(Vector2 normal)
		{
			_normal = normal;
		}

		public static implicit operator bool(RaycastHit raycastHit)
		{
			if (raycastHit == null)
				return false;

			if (raycastHit.normal == Vector2.zero)
				return false;

			return true;
		}
	}
}