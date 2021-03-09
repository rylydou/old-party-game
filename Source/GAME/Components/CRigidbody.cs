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

		public double skinWidth = 0.125;

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
			velocity += Physics.gravity * Time.deltaTime;

			var direction = velocity.sign;

			for (int i = 0; i < raycastsCount.x; i++)
			{
				var offset = direction.y > 0.0 ? size.y - skinWidth : skinWidth * 2;

				var rayPos = position + new Vector2(raySpacing.y * i, offset);
				var rayDir = velocity.isolateY.sign;

				var hit = CStage.current.Raycast(rayPos, rayDir);

				rays.Add(rayPos);
				rays.Add(rayDir);

				if (hit && Math.Abs(hit.distance) < (rayDir.y + skinWidth) * 4)
				{
					if (hit.distance < velocity.y)
					{
						position.y = hit.position.y - (direction.y > 0.0 ? size.y + skinWidth : -skinWidth);
						velocity.y = 0.0;
					}
				}
			}

			for (int i = 0; i < raycastsCount.y; i++)
			{
				var offset = direction.x > 0.0 ? size.x - skinWidth : skinWidth * 2;

				var rayPos = position + new Vector2(offset, raySpacing.x * i);
				var rayDir = velocity.isolateX.sign;

				var hit = CStage.current.Raycast(rayPos, rayDir);

				rays.Add(rayPos);
				rays.Add(rayDir);

				if (hit && Math.Abs(hit.distance) < (rayDir.x + skinWidth) * 4)
				{
					if (hit.distance < velocity.x)
					{
						position.x = hit.position.x - (direction.x > 0.0 ? size.x + skinWidth : -skinWidth);
						velocity.x = 0.0;
					}
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

		public override void Draw()
		{
			using (new DrawBatch())
			{
				for (int i = 0; i < rays.Count / 2; i++)
				{
					GFX.DrawLine(
						rays[i * 2],
						(rays[i * 2] + rays[i * 2 + 1]),
						new Color(1, 0, 0, 1.0f / 3.0f),
						0.5f
					);
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