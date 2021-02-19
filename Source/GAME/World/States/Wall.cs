namespace GAME.World
{
	public abstract class Wall : Solid
	{
		public override int density => int.MaxValue;
	}
}