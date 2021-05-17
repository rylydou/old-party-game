using MGE;

namespace GAME
{
	public class Palette
	{
		public Color backgroundA;
		public Color backgroundB;

		public Palette(string backgroundA, string backgroundB)
		{
			this.backgroundA = new Color(backgroundA);
			this.backgroundB = new Color(backgroundB);
		}
	}
}