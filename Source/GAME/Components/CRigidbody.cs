using MGE;
using MGE.ECS;
using MGE.InputSystem;
using MGE.Physics;

namespace GAME.Components
{
	public class CRigidbody : Component
	{
		public bool interpolate = true;

		public ICanRaycast raycaster;

		Vector2 _size = new Vector2(6);
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

		public float skinWidth = 1;

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

		public override void Init()
		{
			CalcRaySpacing();
		}

		public override void FixedUpdate()
		{
			if (raycaster == null) raycaster = entity.layer.FindEntityByComponent<CWorld>().GetComponent<CWorld>();

			velocity += Physics.gravity * Time.deltaTime;

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
		}

		public override void Update()
		{
			if (Input.GetButtonPress(Inputs.MouseMiddle))
			{
				position = Input.cameraMousePosition;
				velocity = Vector2.zero;
			}

			entity.position = position;
		}

		public void CalcRaySpacing()
		{
			effectiveSize = size - skinWidth;
			raySpacing = effectiveSize / (Vector2)raycastsCount;
		}
	}
}