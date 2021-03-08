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

		Vector2 _size = new Vector2(1, 1);
		public Vector2 size
		{
			get => _size;
			set
			{
				_size = size;
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
				_raycastsCount = raycastsCount;
				CalcRaySpacing();
			}
		}

		public Vector2 raySpacing { get; private set; } = Vector2.zero;

		public Vector2 effectiveSize { get; private set; } = Vector2.zero;

		List<Vector2> rays = new List<Vector2>();

		public override void Init()
		{
			CalcRaySpacing();
		}

		public override void FixedUpdate()
		{
			velocity += Physics.gravity;

			var direction = velocity.sign;

			for (int i = 0; i < raycastsCount.x; i++)
			{
				var rayPos = position + new Vector2(skinWidth, i * raySpacing.y);
				var hit = CStage.current.Raycast(rayPos, direction.isolateY, 1);
				rays.Add(rayPos);
				rays.Add(direction.isolateY);

				if (hit)
				{
					position.y = hit.position.y - (direction.x > 0.0 ? effectiveSize.y : 0.0);
					velocity.y = 0.0;
				}
			}

			for (int i = 0; i < raycastsCount.y; i++)
			{
				var rayPos = position + new Vector2(skinWidth, i * raySpacing.y);
				var hit = CStage.current.Raycast(rayPos, direction.isolateX, 1);
				rays.Add(rayPos);
				rays.Add(direction.isolateY);

				if (hit)
				{
					position.x = hit.position.x - (direction.y > 0.0 ? effectiveSize.x : 0.0);
					velocity.x = 0.0;
				}
			}

			position += velocity;

			Logger.Log($"Dir: {direction}, Vel: {velocity}");
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
					GFX.DrawLine(rays[i * 2], rays[i * 2] + rays[i * 2 + 1], Color.red, 0.25f);
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