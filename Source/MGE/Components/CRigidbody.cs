using MGE.ECS;
using MGE.Graphics;
using MGE.Physics;

namespace MGE.Components
{
	public class CRigidbody : Component
	{
		public bool interpolate = true;

		public ICanRaycast raycaster;

		public Vector2 bounceyness = Vector2.zero;

		public float skinWidth = 0.0f;

		Vector2 _size = new Vector2(0.8f, 0.8f);
		public Vector2 size
		{
			get => _size - skinWidth * 2;
			set
			{
				_size = value + skinWidth * 2;
				CalcRaySpacing();
			}
		}

		public Vector2 position = Vector2.zero;
		public Vector2 effectivePosition
		{
			get => position + ((Vector2.one - size) / 2) + skinWidth;
			set => position = value - ((Vector2.one - size) / 2) - skinWidth;
		}

		public Rect rect { get => new Rect(effectivePosition, size); }

		public Vector2 velocity = Vector2.zero;

		public bool grounded = false;

		public bool hitTop { get; private set; } = false;
		public bool hitRight { get; private set; } = false;
		public bool hitBottom { get; private set; } = false;
		public bool hitLeft { get; private set; } = false;
		public bool hitX { get; private set; } = false;
		public bool hitY { get; private set; } = false;

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

		public CRigidbody(Vector2 bounceyness = default)
		{
			this.bounceyness = bounceyness;
		}

		public override void Init()
		{
			base.Init();

			if (raycaster is null) raycaster = entity.layer.raycaster;

			position = entity.position;

			CalcRaySpacing();
		}

		public override void Tick()
		{
			base.Tick();

			if (raycaster is null)
			{
				Logger.LogError("Rigidbody has no raycaster!");
				return;
			}

			hitTop = false;
			hitRight = false;
			hitBottom = false;
			hitLeft = false;
			hitX = false;
			hitY = false;

			velocity += Physics.Physics.gravity * Time.fixedDeltaTime;

			var direction = velocity.sign;

			for (int i = 0; i < raycastsCount.x; i++)
			{
				var rayPos = effectivePosition + new Vector2(raySpacing.x * i, direction.y > 0 ? size.y + skinWidth : -skinWidth);
				var rayDir = velocity.isolateY.sign;

				var hit = raycaster.Raycast(rayPos, rayDir);

				if (RaycastHit.WithinDistance(hit, Math.Abs(velocity.y)))
				{
					effectivePosition = new Vector2(effectivePosition.x, hit.position.y - (direction.y > 0 ? size.y + skinWidth : -skinWidth));
					velocity.y = bounceyness.y * -velocity.y;
					position.y += velocity.y * 4;

					if (direction.y > 0)
						hitBottom = true;
					else
						hitTop = true;

					hitY = true;
				}
			}

			for (int i = 0; i < raycastsCount.y; i++)
			{
				var rayPos = effectivePosition + new Vector2(direction.x > 0 ? size.x + skinWidth : -skinWidth, raySpacing.y * i);
				var rayDir = velocity.isolateX.sign;

				var hit = raycaster.Raycast(rayPos, rayDir);

				if (RaycastHit.WithinDistance(hit, Math.Abs(velocity.x)))
				{
					effectivePosition = new Vector2(hit.position.x - (direction.x > 0 ? size.x + skinWidth : -skinWidth), effectivePosition.y);
					velocity.x = bounceyness.x * -velocity.x;
					position.x += velocity.x * 4;

					if (direction.x > 0)
						hitRight = true;
					else
						hitLeft = true;

					hitX = true;
				}
			}

			grounded = false;
			if (!(velocity.y < 0))
			{
				grounded = RaycastHit.WithinDistance(raycaster.Raycast(rect.bottomLeft, Vector2.up), GFX.currentUnitsPerPixel + skinWidth);
				if (!grounded)
					grounded = RaycastHit.WithinDistance(raycaster.Raycast(rect.bottomRight - new Vector2(GFX.currentUnitsPerPixel / 2, 0), Vector2.up), GFX.currentUnitsPerPixel + skinWidth);
			}

			position += velocity;
		}

		public override void Update()
		{
			base.Update();

			entity.position = position;
		}

#if INDEV
		public override void Draw()
		{
			base.Draw();

			if (!Physics.Physics.DEBUG) return;

			Graphics.GFX.DrawRect(new Rect(effectivePosition, size), grounded ? Color.magenta : Color.green);

			var direction = velocity.sign;

			for (int i = 0; i < raycastsCount.x; i++)
			{
				var offset = direction.y > 0 ? size.y : 0;

				var rayPos = effectivePosition + new Vector2(raySpacing.x * i, offset);
				var rayDir = velocity.isolateY.sign;

				Graphics.GFX.DrawLine(rayPos, rayPos + rayDir / 2, new Color(1, 0, 0, 0.5f));
			}

			for (int i = 0; i < raycastsCount.y; i++)
			{
				var offset = direction.x > 0 ? size.x : 0;

				var rayPos = effectivePosition + new Vector2(offset, raySpacing.y * i);
				var rayDir = velocity.isolateX.sign;

				Graphics.GFX.DrawLine(rayPos, rayPos + rayDir / 2, new Color(1, 0, 0, 0.5f));
			}

			Graphics.GFX.DrawLine(rect.bottomLeft, rect.bottomLeft + Vector2.up / 2, new Color(0, 0, 1, 0.5f));
			Graphics.GFX.DrawLine(rect.bottomRight, rect.bottomRight + Vector2.up / 2, new Color(0, 0, 1, 0.5f));

			Graphics.GFX.DrawLine(position + Vector2.one / 2, position + Vector2.one / 2 + velocity * 8, Color.blue, 2);
		}
#endif

		public void CalcRaySpacing()
		{
			// FIXME: Make not jank
			raySpacing = size / (Vector2)raycastsCount + new Vector2(1f / 16);
		}
	}
}