using System.Collections.Generic;
using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.Physics;

namespace GAME.Components
{
	public class CRigidbody : Component
	{
		public bool interpolate = true;

		Vector2 _size = Vector2.zero;
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
		public Vector2 velocity = Vector2.zero;

		public double skinWidth = 0.0;

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

		public Vector2 effectiveSize { get; private set; } = Vector2.zero;

		List<Vector2> rays = new List<Vector2>();

		public override void Init()
		{
			size = new Vector2(CStage.current.tileSize);
			CalcRaySpacing();
		}

		public override void FixedUpdate()
		{
			position += velocity;

			velocity += Physics.gravity * Time.deltaTime;

			var direction = velocity.sign;

			// if (velocity.y > 0)
			// {
			for (int i = 0; i < raycastsCount.x; i++)
			{
				var rayPos = position + new Vector2(raySpacing.y * i, -skinWidth + size.y) / CStage.current.tileSize;
				var hit = CStage.current.Raycast(rayPos, Vector2.up);
				rays.Add(rayPos);
				rays.Add(Vector2.up * velocity.y);

				if (hit /* && Math.Abs(hit.distance) < velocity.y */)
				{
					rays.Add(hit.position);
					rays.Add(hit.normal);
					if (hit.distance < velocity.y)
					{
						position.y = hit.position.y;
						velocity.y = 0.0;
					}
				}
			}
			// }
		}

		public override void Update()
		{
			if (Input.GetButtonPress(Inputs.MouseLeft))
				position = Input.cameraMousePosition;

			entity.position = position;
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				for (int i = 0; i < rays.Count / 2; i++)
				{
					GFX.DrawLine(rays[i * 2], rays[i * 2] + rays[i * 2 + 1], Color.red, 0.5f);
				}
			}

			rays.Clear();
		}

		public void CalcRaySpacing()
		{
			effectiveSize = size - skinWidth * 2;
			raySpacing = effectiveSize / (Vector2)raycastsCount;
		}
	}
}