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
		public bool pause = false;
		public bool die = false;
		public bool DEBUG_SPAWN_BOX = false;

		public PlayerControls(int index)
		{
			this.index = index;
		}

		public override void Update()
		{
			switch (index)
			{
				case -1:
					isConnected = true;
					move = (Input.GetButton(Inputs.D) ? 1 : 0) - (Input.GetButton(Inputs.A) ? 1 : 0);
					crouch = Input.GetButton(Inputs.S);
					jump = Input.GetButtonPress(Inputs.W);
					jumpRelease = Input.GetButtonRelease(Inputs.W);
					use = Input.GetButtonPress(Inputs.Space) | Input.GetButtonPress(Inputs.E);
					pause = Input.GetButtonPress(Inputs.Escape);
					die = Input.GetButtonPress(Inputs.G);
					DEBUG_SPAWN_BOX = Input.GetButtonPress(Inputs.Tab);
					break;
				case -2:
					isConnected = true;
					move = (Input.GetButton(Inputs.Right) ? 1 : 0) - (Input.GetButton(Inputs.Left) ? 1 : 0);
					crouch = Input.GetButton(Inputs.Down);
					jump = Input.GetButtonPress(Inputs.Up);
					jumpRelease = Input.GetButtonRelease(Inputs.Up);
					use = Input.GetButtonPress(Inputs.RightControl) | Input.GetButtonPress(Inputs.RightShift) | Input.GetButtonPress(Inputs.RightAlt);
					die = Input.GetButtonPress(Inputs.Pipe);
					break;
				default:
					if (Input.GamepadConnected(index))
					{
						isConnected = true;
						move = (Input.GetButton(Inputs.GamepadRight, index) ? 1 : 0) - (Input.GetButton(Inputs.GamepadLeft, index) ? 1 : 0);
						if (Input.GetLeftStick(index).x.Abs() > 0.33f)
							move = Input.GetLeftStick(index).x.Sign();
						crouch = Input.GetLeftStick(index).y < -0.5f || Input.GetButton(Inputs.GamepadDown, index);
						jump = Input.GetButtonPress(Inputs.GamepadA, index) | Input.GetButtonPress(Inputs.GamepadB, index);
						jumpRelease = Input.GetButtonRelease(Inputs.GamepadA, index) | Input.GetButtonRelease(Inputs.GamepadB, index);
						use = Input.GetButtonPress(Inputs.GamepadX, index) | Input.GetButtonPress(Inputs.GamepadY, index) | Input.GetButtonPress(Inputs.GamepadRT, index);
						pause = Input.GetButtonPress(Inputs.GamepadStart, index);
						die = Input.GetButtonPress(Inputs.GamepadSelect, index) | Input.GetButtonPress(Inputs.GamepadRS, index);
						DEBUG_SPAWN_BOX = Input.GetButtonPress(Inputs.GamepadLB, index);
					}
					else
						isConnected = false;
					break;
			}
		}
	}
}