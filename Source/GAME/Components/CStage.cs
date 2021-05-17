using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.InputSystem;
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
			stage.Draw(new Vector2(GFX.currentUnitsPerPixel * 2), new Color(0, 0.25f));
			stage.Draw(Vector2.zero, Color.white);

			// Config.font.DrawText(Stage.tilesets[1].Item2.GetConnections((int)Input.cameraMousePosition.x, (int)Input.cameraMousePosition.y, (x, y) => stage.tiles.Get(x, y) != 0).ToString(), Input.cameraMousePosition, Color.white, 0.025f);
		}

		public RaycastHit Raycast(Vector2 origin, Vector2 direction, int maxIterations = -1)
		{
			var hit = Physics.RayVsGrid(origin, direction, (x, y) =>
			{
				var tile = stage.tiles.Get(x, y);

				if (tile == 0) return false;

				return Stage.tilesets[tile].Item1.IsSolid(new Vector2Int(x, y), origin, direction);
			}, maxIterations);

			return hit;
		}

		public bool IsSolid(Vector2 position)
		{
			var tile = stage.tiles.Get(position);
			if (tile == 0) return false;
			return Stage.tilesets[tile].Item1.IsSolid(position, position, Vector2.zero);
		}
	}
}