using MGE;
using MGE.ECS;
using MGE.InputSystem;
using GAME.World;
using GAME.World.Generation;

namespace GAME.Components
{
	public class CWorld : Component
	{
		public static Grid grid;

		public override void Init()
		{
			grid = new Grid(new Vector2Int(64 * 4, 64 * 2), new GenTest());
		}

		public override void Update()
		{
			if (Input.GetButton(Inputs.MouseRight))
				grid.SetTileID(grid.CamToTile(Input.cameraMousePosition), 0);

			if (Input.GetButton(Inputs.MouseLeft))
				grid.SetTileID(grid.CamToTile(Input.cameraMousePosition), 3);

			if (Input.GetButton(Inputs.MouseMiddle))
				grid.SetTileID(grid.CamToTile(Input.cameraMousePosition), 4);

			if (Input.GetButton(Inputs.Space))
				grid.Update();
		}

		public override void Draw()
		{
			grid.Draw();
		}
	}
}