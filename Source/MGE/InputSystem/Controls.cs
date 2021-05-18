namespace MGE.InputSystem
{
	public abstract class Controls
	{
		public bool isEnabled = true;
		public bool isConnected = false;

		public abstract void Update();
	}
}