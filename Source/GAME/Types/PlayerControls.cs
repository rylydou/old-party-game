using MGE;
using MGE.InputSystem;

namespace GAME
{
	public class PlayerControls : Controls
	{
		public static float cursorSensitivity = 512;

		public Vector2 cursorPos = Vector2.zero;

		public float move = 0.0f;
		public bool crouch = false;
		public bool jump = false;
		public bool jumpRelease = false;
		public bool use = false;
		public bool pause = false;
		public bool die = false;

		public PlayerControls(sbyte index)
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
					jump = Input.GetButtonPress(Inputs.W) | Input.GetButtonPress(Inputs.Space);
					jumpRelease = Input.GetButtonRelease(Inputs.W) | Input.GetButtonRelease(Inputs.Space);
					use = Input.GetButtonPress(Inputs.LeftShift) | Input.GetButtonPress(Inputs.E);
					pause = Input.GetButtonPress(Inputs.Escape);
					die = Input.GetButtonPress(Inputs.G);

					cursorPos +=
						new Vector2((Input.GetButton(Inputs.D) ? 1 : 0) - (Input.GetButton(Inputs.A) ? 1 : 0), (Input.GetButton(Inputs.S) ? 1 : 0) - (Input.GetButton(Inputs.W) ? 1 : 0)).normalized
						* cursorSensitivity * Time.deltaTime;
					break;
				case -2:
					isConnected = true;
					move = (Input.GetButton(Inputs.Right) ? 1 : 0) - (Input.GetButton(Inputs.Left) ? 1 : 0);
					crouch = Input.GetButton(Inputs.Down);
					jump = Input.GetButtonPress(Inputs.Up);
					jumpRelease = Input.GetButtonRelease(Inputs.Up);
					use = Input.GetButtonPress(Inputs.RightControl) | Input.GetButtonPress(Inputs.RightShift) | Input.GetButtonPress(Inputs.RightAlt);
					die = Input.GetButtonPress(Inputs.Pipe);

					cursorPos +=
						new Vector2((Input.GetButton(Inputs.Right) ? 1 : 0) - (Input.GetButton(Inputs.Left) ? 1 : 0), (Input.GetButton(Inputs.Down) ? 1 : 0) - (Input.GetButton(Inputs.Up) ? 1 : 0)).normalized
						* cursorSensitivity * Time.deltaTime;
					break;
				default:
					if (Input.GamepadConnected(index))
					{
						isConnected = true;
						move = (Input.GetButton(Inputs.GamepadRight, index) ? 1 : 0) - (Input.GetButton(Inputs.GamepadLeft, index) ? 1 : 0);
						if (Input.GamepadLeftStick(index).x.Abs() > 0.33f)
							move = Input.GamepadLeftStick(index).x.Sign();
						crouch = Input.GamepadLeftStick(index).y < -0.5f || Input.GetButton(Inputs.GamepadDown, index);
						jump = Input.GetButtonPress(Inputs.GamepadA, index) | Input.GetButtonPress(Inputs.GamepadB, index);
						jumpRelease = Input.GetButtonRelease(Inputs.GamepadA, index) | Input.GetButtonRelease(Inputs.GamepadB, index);
						use = Input.GetButtonPress(Inputs.GamepadX, index) | Input.GetButtonPress(Inputs.GamepadY, index) | Input.GetButtonPress(Inputs.GamepadRT, index);
						pause = Input.GetButtonPress(Inputs.GamepadStart, index);
						die = Input.GetButtonPress(Inputs.GamepadSelect, index) | Input.GetButtonPress(Inputs.GamepadRS, index);

						if (Input.GamepadLeftStick(index).sqrMagnitude > 0.25f * 0.25f)
							cursorPos += Input.GamepadLeftStick(index).normalized * new Vector2(1, -1) * cursorSensitivity * Time.deltaTime;
					}
					else
						isConnected = false;
					break;
			}

			cursorPos.x = Math.Clamp(cursorPos.x, 0, Window.renderSize.x);
			cursorPos.y = Math.Clamp(cursorPos.y, 0, Window.renderSize.y);
		}
	}
}