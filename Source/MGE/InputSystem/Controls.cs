namespace MGE.InputSystem
{
	public abstract class Controls
	{
		public static bool playerOneUsesKeyboard = true;

		public int index;
		public bool isEnabled = true;

		public abstract void Update();
	}
}