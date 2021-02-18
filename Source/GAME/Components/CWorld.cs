using MGE;
using MGE.ECS;
using MGE.InputSystem;
using GAME.World;
using GAME.World.Generation;

namespace GAME.Components
{
	public class CWorld : Component
	{
		public Grid grid;

		public override void Init()
		{
			grid = new Grid(new Vector2Int(64 * 4, 64 * 2), new GenTest());
		}

		public override void Update()
		{
			if (Input.GetButton(Inputs.MouseLeft))
			{
				grid.SetTileID(grid.CamToTile(Input.cameraMousePosition), 0);
			}
		}

		public override void Draw()
		{
			grid.Draw();
		}
	}
}