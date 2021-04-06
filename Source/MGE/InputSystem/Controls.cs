namespace MGE.InputSystem
{
	public abstract class Controls
	{
		public ControllerIndex controller = ControllerIndex.Keyboard;

		public bool isEnabled = true;

		public abstract void Update();
	}
}