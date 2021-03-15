namespace MGE
{
	static class IntExtensions
	{
		public static float Clamp(this int value, float max) => Math.Clamp(value, 0, max);
		public static float Clamp(this int value, float min, float max) => Math.Clamp(value, min, max);

		public static int Clamp(this int value, int max) => Math.Clamp(value, 0, max);
		public static int Clamp(this int value, int min, int max) => Math.Clamp(value, min, max);
	}
}