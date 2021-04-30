using MGE.InputSystem;

namespace GAME
{
	public class PlayerControls : Controls
	{
		public float move = 0;
		public bool jump = false;

		public override void Update()
		{
			if (controller == ControllerIndex.Keyboard)
			{
				move = (Input.GetButton(Inputs.D) ? 1 : 0) - (Input.GetButton(Inputs.A) ? 1 : 0);
				jump = Input.GetButtonPress(Inputs.Space);
			}
		}
	}
}