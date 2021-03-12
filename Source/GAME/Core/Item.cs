namespace GAME
{
	public abstract class Item
	{
		public const ushort maxStackAmount = 99;

		public abstract string name { get; }

		public ushort amount;
	}
}