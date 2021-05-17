namespace MGE
{
	static class BoolExtensions
	{
		public static void Toggle(this bool state) => state = !state;
	}
}