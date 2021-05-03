using MGE;
using MGE.InputSystem;

namespace GAME
{
	public class PlayerControls : Controls
	{
		public float move = 0.0f;
		public bool crouch = false;
		public bool jump = false;
		public bool jumpRelease = false;
		public bool use = false;
		public bool DEBUG_SPAWN_BOX = false;

		public PlayerControls(int index)
		{
			this.index = index;
		}

		public override void Update()
		{
			if (playerOneUsesKeyboard && index == 0)
			{
				move = (Input.GetButton(Inputs.D) ? 1 : 0) - (Input.GetButton(Inputs.A) ? 1 : 0);
				crouch = Input.GetButton(Inputs.S);
				jump = Input.GetButtonPress(Inputs.Space);
				jumpRelease = Input.GetButtonRelease(Inputs.Space);
				use = Input.GetButtonPress(Inputs.E);
				DEBUG_SPAWN_BOX = Input.GetButtonPress(Inputs.D1);
			}
			else if (Input.GamepadConnected(index))
			{
				var i = index - (playerOneUsesKeyboard ? 1 : 0);

				move = (Input.GetButton(Inputs.GamepadRight, i) ? 1 : 0) - (Input.GetButton(Inputs.GamepadLeft, i) ? 1 : 0);
				if (Input.GetLeftStick(i).x.Abs() > 0.33f)
					move = Input.GetLeftStick(i).x.Sign();
				crouch = Input.GetLeftStick(i).y < -0.5f || Input.GetButton(Inputs.GamepadDown, i);
				jump = Input.GetButtonPress(Inputs.GamepadA, i) | Input.GetButtonPress(Inputs.GamepadB, i);
				jumpRelease = Input.GetButtonRelease(Inputs.GamepadA, i) | Input.GetButtonRelease(Inputs.GamepadB, i);
				use = Input.GetButtonPress(Inputs.GamepadX, i) | Input.GetButtonPress(Inputs.GamepadY, i) | Input.GetButtonPress(Inputs.GamepadRT, i);
				DEBUG_SPAWN_BOX = Input.GetButtonPress(Inputs.GamepadLB, i);
			}
		}
	}
}