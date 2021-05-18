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

		public static List<Player> players = new List<Player>();

		public static PlayerControls mainController = null;

		public static List<PlayerControls> controllers = new List<PlayerControls>()
		{
			new PlayerControls(EController.ArrowKeys),
			new PlayerControls(EController.WASD),
			new PlayerControls(EController.Gamepad0),
			new PlayerControls(EController.Gamepad1),
			new PlayerControls(EController.Gamepad2),
			new PlayerControls(EController.Gamepad3),
		};

		public static PlayerControls GetControls(EController controller) => controllers[(sbyte)controller + 2];
	}
}