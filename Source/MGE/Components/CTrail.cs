using System.Collections.Generic;
using System.Linq;
using MGE.ECS;
using MGE.Graphics;

namespace MGE.Components
{
	public class CTrail : Component
	{
		public Color trailColor = Color.red;
		public float trailThickness = 2.0f;
		// TODO: Add Gradients
		public bool fadeOut = true;
		public float minTimeBtwVertices = 0.025f;
		public float minDistanceBtwVertices = 0.05f;
		public int maxAmountOfVertices = 32;
		public Vector2 trailOffset = Vector2.one / 2;

		float timeSinceLastVertex = 0.0f;

		public List<Vector2> pastPositions = new List<Vector2>();

		public CTrail() { }

		public CTrail(Color trailColor, float trailThickness = 2.0f, bool fadeOut = true, Vector2? trailOffset = null, float minTimeBtwVertices = 0.05f, float minDistanceBtwVertices = 0.25f, int maxAmountOfVertices = 32)
		{
			this.trailColor = trailColor;
			this.trailThickness = trailThickness;
			this.fadeOut = false;
			this.trailOffset = Vector2.one / 2;
			this.minTimeBtwVertices = minTimeBtwVertices;
			this.minDistanceBtwVertices = minDistanceBtwVertices;
			this.maxAmountOfVertices = maxAmountOfVertices;
		}

		public override void Init()
		{
			base.Init();

			pastPositions.Add(entity.position);
		}

		public override void Update()
		{
			base.Update();

			timeSinceLastVertex += Time.deltaTime;

			if (timeSinceLastVertex > minTimeBtwVertices && Vector2.DistanceGT(pastPositions.Last(), entity.position, minDistanceBtwVertices))
				pastPositions.Add(entity.position);

			while (pastPositions.Count > maxAmountOfVertices)
				pastPositions.RemoveAt(0);
		}

		public override void Draw()
		{
			base.Draw();

			var points = pastPositions.ToArray();

			for (int i = 1; i < points.Length; i++)
				GFX.DrawLine(points[i] + trailOffset, (i + 1 >= points.Length ? entity.position : points[i + 1]) + trailOffset, fadeOut ? Color.Lerp(trailColor, Color.clear, 1 - (float)i / points.Length) : trailColor, trailThickness);
		}
	}
}