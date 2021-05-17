using System.Collections.Generic;

namespace GAME
{
	public class GameSettings
	{
		public static GameSettings current;

		public Stage stage;

		public float roundTime = 60 * 5.0f;
		public float maxOvertime = 60 * 1.0f;
		public float timeBtwCrates = 4.5f;
		public float timeBtwCrateLessPerPlayer = 0.5f;

		public List<Player> players = new List<Player>
		{
			new Player(-1, "_Default"),
		};

		public PlayerControls mainController = null;

		public Dictionary<EController, PlayerControls> controllers = new Dictionary<EController, PlayerControls>()
		{
			{ EController.ArrowKeys, new PlayerControls(-2) },
			{ EController.WASD, new PlayerControls(-1) },
			{ EController.Gamepad0, new PlayerControls(0) },
			{ EController.Gamepad1, new PlayerControls(1) },
			{ EController.Gamepad2, new PlayerControls(2) },
			{ EController.Gamepad3, new PlayerControls(3) },
		};
	}
}