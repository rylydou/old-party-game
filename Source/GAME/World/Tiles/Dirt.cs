using MGE;

namespace GAME.World
{
	public class Dirt : ITile
	{
		public string name => "Dirt";

		public Color color => new Color("#553322");

		public void Update()
		{
			Logger.Log("Updated " + name);
		}
	}
}