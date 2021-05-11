using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.Physics;

namespace GAME.Components
{
	public class CStage : Component, ICanRaycast
	{
		public static CStage current { get; private set; }

		Stage stage;

		public override void Init()
		{
			current = this;
			entity.layer.raycaster = this;

			stage = GameSettings.current.stage;
		}

		public override void Draw()
		{
			stage.Draw(new Vector2(GFX.currentUnitsPerPixel), new Color(0, 0.25f));
			stage.Draw(Vector2.zero, Color.white);
		}

		public RaycastHit Raycast(Vector2 origin, Vector2 direction, int maxIterations = -1)
		{
			origin = (origin - entity.position);

			var hit = Physics.RayVsGrid(origin, direction, (x, y) =>
			{
				var tile = stage.tiles.Get(x, y);

				if (tile == 0) return false;

				if (Stage.tilesets[tile].semiSolid)
					return origin.y <= y && direction.y > 0;

				return true;
			}, maxIterations);

			if (hit is object)
			{
				hit.origin = origin + entity.position;
			}

			return hit;
		}

		public bool IsSolid(Vector2 position)
		{
			var tile = stage.tiles.Get(position);
			return tile != 0 && !Stage.tilesets[tile].semiSolid;
		}
	}
}