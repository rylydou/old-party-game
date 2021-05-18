using MGE;
using MGE.InputSystem;

namespace GAME
{
	public class PlayerControls : Controls
	{
		public EController id;
		public sbyte index { get => (sbyte)id; }

		public bool select = false;
		public bool back = false;

		public bool up = false;
		public bool left = false;
		public bool down = false;
		public bool right = false;

		public float move = 0.0f;
		public bool crouch = false;
		public bool jump = false;
		public bool jumpRelease = false;
		public bool use = false;
		public bool pause = false;
		public bool die = false;

		public PlayerControls(EController id)
		{
			this.id = id;
		}

		public override void Update()
		{
			switch (id)
			{
				case EController.WASD:
					isConnected = true;

					select = Input.GetButtonPress(Inputs.Space);
					back = Input.GetButtonPress(Inputs.Escape) | Input.GetButtonPress(Inputs.Q);

					up = Input.GetButtonPress(Inputs.W);
					left = Input.GetButtonPress(Inputs.A);
					down = Input.GetButtonPress(Inputs.S);
					right = Input.GetButtonPress(Inputs.D);

					move = (Input.GetButton(Inputs.D) ? 1 : 0) - (Input.GetButton(Inputs.A) ? 1 : 0);
					crouch = Input.GetButton(Inputs.S);
					jump = Input.GetButtonPress(Inputs.W);
					jumpRelease = Input.GetButtonRelease(Inputs.W);
					use = Input.GetButtonPress(Inputs.LeftShift) | Input.GetButtonPress(Inputs.E) | Input.GetButtonPress(Inputs.Q) | Input.GetButtonPress(Inputs.F) | Input.GetButtonPress(Inputs.Space); ;
					pause = Input.GetButtonPress(Inputs.Escape);
					die = Input.GetButtonPress(Inputs.G);
					break;
				case EController.ArrowKeys:
					isConnected = true;

					select = Input.GetButtonPress(Inputs.Enter);
					back = Input.GetButtonPress(Inputs.Delete) | Input.GetButtonPress(Inputs.Back);

					up = Input.GetButtonPress(Inputs.Up);
					left = Input.GetButtonPress(Inputs.Left);
					down = Input.GetButtonPress(Inputs.Down);
					right = Input.GetButtonPress(Inputs.Right);

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

						select = Input.GetButtonPress(Inputs.GamepadB, index);
						back = Input.GetButtonPress(Inputs.GamepadA, index);

						up = Input.GetButtonPress(Inputs.GamepadUp, index);
						left = Input.GetButtonPress(Inputs.GamepadLeft, index);
						down = Input.GetButtonPress(Inputs.GamepadDown, index);
						right = Input.GetButtonPress(Inputs.GamepadRight, index);

						move = (Input.GetButton(Inputs.GamepadRight, index) ? 1 : 0) - (Input.GetButton(Inputs.GamepadLeft, index) ? 1 : 0);
						if (Input.GamepadLeftStick(index).x.Abs() > 0.33f)
							move = Input.GamepadLeftStick(index).x.Sign();
						crouch = Input.GamepadLeftStick(index).y < -0.5f || Input.GetButton(Inputs.GamepadDown, index);
						jump = Input.GetButtonPress(Inputs.GamepadA, index) | Input.GetButtonPress(Inputs.GamepadB, index);
						jumpRelease = Input.GetButtonRelease(Inputs.GamepadA, index) | Input.GetButtonRelease(Inputs.GamepadB, index);
						use = Input.GetButtonPress(Inputs.GamepadX, index) | Input.GetButtonPress(Inputs.GamepadY, index) | Input.GetButtonPress(Inputs.GamepadRT, index);
						pause = Input.GetButtonPress(Inputs.GamepadStart, index);
						die = Input.GetButtonPress(Inputs.GamepadSelect, index) | Input.GetButtonPress(Inputs.GamepadRS, index);
					}
					else
						isConnected = false;
					break;
			}
		}
	}
}