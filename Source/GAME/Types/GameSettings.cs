using System.Collections.Generic;

namespace GAME
{
	public class GameSettings
	{
		public static GameSettings current;

		public Stage stage;

		public float roundTime = 60 * 5;
		public float maxOvertime = 60 * 1;
		public float timeBtwCrates = 4.5f;

		public List<Player> players = new List<Player>
		{
			new Player(-2, "Amogus"),
			new Player(-1, "Chicken"),
			new Player(0, "Goose"),
			// new Player(1, "Robot"),
		};

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