using MGE.ECS;
using MGE.Physics;

namespace MGE.Components
{
	public class CRigidbody : Component
	{
		public bool interpolate = true;

		public ICanRaycast raycaster;

		Vector2 _size = new Vector2(0.8f, 0.8f);
		public Vector2 size
		{
			get => _size;
			set
			{
				_size = value;
				CalcRaySpacing();
			}
		}

		public Vector2 position = Vector2.zero;
		public Vector2 effectivePosition
		{
			get => position + ((Vector2.one - size) / 2);
			set => position = value - ((Vector2.one - size) / 2);
		}

		public Rect rect { get => new Rect(effectivePosition, size); }

		public Vector2 velocity = Vector2.zero;

		public bool grounded = false;

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
				var offset = direction.y > 0.0f ? size.y : 0;

				var rayPos = effectivePosition + new Vector2(raySpacing.y * i, offset);
				var rayDir = velocity.isolateY.sign;

				var hit = raycaster.Raycast(rayPos, rayDir);

				if (RaycastHit.WithinDistance(hit, Math.Abs(velocity.y)))
				{
					effectivePosition = new Vector2(effectivePosition.x, hit.position.y - (direction.y > 0.0f ? size.y : 0));
					velocity.y = 0.0f;
				}
			}

			for (int i = 0; i < raycastsCount.y; i++)
			{
				var offset = direction.x > 0.0f ? size.x : 0;

				var rayPos = effectivePosition + new Vector2(offset, raySpacing.x * i);
				var rayDir = velocity.isolateX.sign;

				var hit = raycaster.Raycast(rayPos, rayDir);

				if (RaycastHit.WithinDistance(hit, Math.Abs(velocity.x)))
				{
					effectivePosition = new Vector2(hit.position.x - (direction.x > 0.0f ? size.x : 0), effectivePosition.y);
					velocity.x = 0.0f;
				}
			}

			grounded = false;
			if (!(velocity.y < 0))
			{
				grounded = RaycastHit.WithinDistance(raycaster.Raycast(rect.bottomLeft, Vector2.up), 1f / 16 * 2);
				if (!grounded)
					grounded = RaycastHit.WithinDistance(raycaster.Raycast(rect.bottomRight, Vector2.up), 1f / 16 * 2);
			}

			position += velocity;
		}

		public override void Update()
		{
			base.Update();

			entity.position = position;
		}

		public override void Draw()
		{
			base.Draw();

			// Graphics.GFX.DrawLine(position + Vector2.one / 2, position + Vector2.one / 2 + velocity, Color.blue, 0.1f);

			// Graphics.GFX.DrawRect(new Rect(effectivePosition, size), Color.green, 1f / 16);

			// var direction = velocity.sign;

			// for (int i = 0; i < raycastsCount.x; i++)
			// {
			// 	var offset = direction.y > 0.0f ? size.y : 0;

			// 	var rayPos = effectivePosition + new Vector2(raySpacing.y * i, offset);
			// 	var rayDir = velocity.isolateY.sign;

			// 	Graphics.GFX.DrawLine(rayPos, rayPos + rayDir, new Color(1, 0, 0, 0.25f), 1f / 16);
			// }

			// for (int i = 0; i < raycastsCount.y; i++)
			// {
			// 	var offset = direction.x > 0.0f ? size.x : 0;

			// 	var rayPos = effectivePosition + new Vector2(offset, raySpacing.x * i);
			// 	var rayDir = velocity.isolateX.sign;

			// 	Graphics.GFX.DrawLine(rayPos, rayPos + rayDir, new Color(1, 0, 0, 0.25f), 1f / 16);
			// }

			// Graphics.GFX.DrawLine(rect.bottomLeft, rect.bottomLeft + Vector2.up, new Color(1, 0, 0, 0.25f), 1f / 16);
			// Graphics.GFX.DrawLine(rect.bottomRight, rect.bottomRight + Vector2.up, new Color(1, 0, 0, 0.25f), 1f / 16);
		}

		public void CalcRaySpacing()
		{
			// FIXME: Make not jank
			raySpacing = size / (Vector2)raycastsCount + new Vector2(1f / 16);
		}
	}
}