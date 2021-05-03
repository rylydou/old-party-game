using MGE.ECS;
using MGE.Physics;

namespace MGE.Components
{
	public class CRigidbody : Component
	{
		public bool interpolate = true;

		public ICanRaycast raycaster;

		Vector2 _size = new Vector2(0.8f, 1.0f);
		public Vector2 size
		{
			get => _size;
			set
			{
				_size = value;
				CalcRaySpacing();
			}
		}
		public Vector2 effectiveSize { get; private set; } = Vector2.zero;

		public Vector2 position = Vector2.zero;
		public Vector2 effectivePosition
		{
			get => position + skinWidth;
			set => position = value - skinWidth;
		}

		public Vector2 velocity = Vector2.zero;

		public bool grounded = false;

		public float skinWidth = 0.1f;

		Vector2Int _raycastsCount = new Vector2Int(4, 4);
		public Vector2Int raycastsCount
		{
			get => _raycastsCount;
			set
			{
				_raycastsCount = value;
				CalcRaySpacing();
			}
		}

		public Vector2 raySpacing { get; private set; } = Vector2.zero;

		public CRigidbody() { }

		public CRigidbody(Vector2 size)
		{
			this.size = size;
		}

		public CRigidbody(Vector2 position, Vector2 size)
		{
			this.position = position;
			this.size = size;
		}

		public override void Init()
		{
			base.Init();

			if (raycaster is null) raycaster = entity.layer.raycaster;

			position = entity.position;

			CalcRaySpacing();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (raycaster is null)
			{
				Logger.LogWarning("Rigidbody has no raycaster!");
				return;
			}

			velocity += Physics.Physics.gravity * Time.deltaTime;

			var direction = velocity.sign;

			for (int i = 0; i < raycastsCount.x; i++)
			{
				var offset = direction.y > 0.0f ? effectiveSize.y - skinWidth : skinWidth;

				var rayPos = effectivePosition + new Vector2(raySpacing.y * i, offset);
				var rayDir = velocity.isolateY.sign;

				var hit = raycaster.Raycast(rayPos, rayDir);

				if (RaycastHit.WithinDistance(hit, Math.Abs(velocity.y) + skinWidth * 2))
				{
					effectivePosition = new Vector2(effectivePosition.x, hit.position.y - (direction.y > 0.0f ? effectiveSize.y + skinWidth : -skinWidth));
					velocity.y = 0.0f;
				}
			}

			for (int i = 0; i < raycastsCount.y; i++)
			{
				var offset = direction.x > 0.0f ? effectiveSize.x - skinWidth : skinWidth;

				var rayPos = effectivePosition + new Vector2(offset, raySpacing.x * i);
				var rayDir = velocity.isolateX.sign;

				var hit = raycaster.Raycast(rayPos, rayDir);

				if (RaycastHit.WithinDistance(hit, Math.Abs(velocity.x) + skinWidth * 2))
				{
					effectivePosition = new Vector2(hit.position.x - (direction.x > 0.0f ? effectiveSize.x + skinWidth : -skinWidth), effectivePosition.y);
					velocity.x = 0.0f;
				}
			}

			position += velocity;

			grounded = RaycastHit.WithinDistance(raycaster.Raycast(position + new Vector2(size.x / 2, size.y), Vector2.up), skinWidth);
		}

		public override void Update()
		{
			base.Update();

			entity.position = position;
		}

		public void CalcRaySpacing()
		{
			effectiveSize = size - skinWidth * 2;
			raySpacing = effectiveSize / (Vector2)raycastsCount;
		}
	}
}