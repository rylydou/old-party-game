using MGE.InputSystem;
using MGE.UI.Layouts;

namespace MGE.Debug.Menus
{
	public class DMenuInput : DebugMenu
	{
		public override string name => "Input";

		public override void UpdateBG()
		{
			base.UpdateBG();

			using (var layout = new StackLayout(offset, allSize, false))
			{
				gui.Text($"--- Mouse ---", layout.AddElement(), Colors.text);
				gui.Text($"Win Pos: {Input.windowMousePosition}", layout.AddElement(), Colors.text);
				gui.Text($"Cam Pos: {Input.cameraMousePosition}", layout.AddElement(), Colors.text);
				gui.Text($"Scroll: {Input.scroll} | {Input.scrollRaw}", layout.AddElement(), Colors.text);
				gui.Text($"--- Keyboard ---", layout.AddElement(), Colors.text);
				gui.Text($"Keyboard String: {Util.RemoveBadChars(Input.keyboardString)}", layout.AddElement(), Colors.text);
				gui.Text($"Keyboard Char: {Util.RemoveBadChars(Input.keyboardLastChar.ToString())}", layout.AddElement(), Colors.text);
				gui.Text($"--- Gamepad ---", layout.AddElement(), Colors.text);
				for (int i = 0; i < Input.maxGamepadCount; i++)
				{
					gui.Text($"--- Gamepad #{i} ---", layout.AddElement(), Colors.text);

					if (Input.GamepadConnected(i))
					{
						gui.Text($"LStick: {Input.GetLeftStick(i)}", layout.AddElement(), Colors.text);
						gui.Text($"RStick: {Input.GetRightStick(i)}", layout.AddElement(), Colors.text);

						gui.Text($"LTrigger: {Input.GetLeftTrigger(i)}", layout.AddElement(), Colors.text);
						gui.Text($"RTrigger: {Input.GetRightTrigger(i)}", layout.AddElement(), Colors.text);

						gui.Text($"Gamepad A: {Input.GetButton(Inputs.GamepadA)}", layout.AddElement(), Colors.text);
						gui.Text($"Gamepad B: {Input.GetButton(Inputs.GamepadB)}", layout.AddElement(), Colors.text);
						gui.Text($"Gamepad X: {Input.GetButton(Inputs.GamepadX)}", layout.AddElement(), Colors.text);
						gui.Text($"Gamepad Y: {Input.GetButton(Inputs.GamepadY)}", layout.AddElement(), Colors.text);

						gui.Text($"Gamepad Up: {Input.GetButton(Inputs.GamepadUp)}", layout.AddElement(), Colors.text);
						gui.Text($"Gamepad Right: {Input.GetButton(Inputs.GamepadRight)}", layout.AddElement(), Colors.text);
						gui.Text($"Gamepad Down: {Input.GetButton(Inputs.GamepadDown)}", layout.AddElement(), Colors.text);
						gui.Text($"Gamepad Left: {Input.GetButton(Inputs.GamepadLeft)}", layout.AddElement(), Colors.text);

						gui.Text($"Gamepad LB: {Input.GetButton(Inputs.GamepadLB)}", layout.AddElement(), Colors.text);
						gui.Text($"Gamepad RB: {Input.GetButton(Inputs.GamepadRB)}", layout.AddElement(), Colors.text);
					}
					else
						gui.Text($"No Gamepad Connected", layout.AddElement(), Colors.text);
				}
			}
		}
	}
}