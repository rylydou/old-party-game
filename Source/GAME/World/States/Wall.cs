namespace GAME.World
{
	public abstract class Wall : Solid
	{
		public override short density => short.MaxValue;
	}
}