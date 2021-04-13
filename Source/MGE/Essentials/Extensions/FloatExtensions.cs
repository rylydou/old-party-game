namespace MGE
{
	static class FloatExtensions
	{
		public static float Abs(this float value) => Math.Abs(value);
		public static float Sign(this float value) => Math.Sign(value);

		public static float Clamp(this float value, float max) => Math.Clamp(value, 0, max);
		public static float Clamp(this float value, float min, float max) => Math.Clamp(value, min, max);

		public static int Clamp(this float value, int max) => (int)Math.Clamp(value, 0, max);
		public static int Clamp(this float value, int min, int max) => (int)Math.Clamp(value, min, max);
	}
}