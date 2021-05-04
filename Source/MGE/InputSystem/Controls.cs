namespace MGE.InputSystem
{
	public abstract class Controls
	{
		public int index = 0;
		public bool isEnabled = true;
		public bool isConnected = false;

		public abstract void Update();
	}
}