using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MGE.InputSystem
{
	public class Input : EssentialVars
	{
		#region Mouse
		public static Vector2 mousePosition { get; set; } = Vector2.zero;

		static List<Inputs> _mouseButtons = new List<Inputs>();
		static List<Inputs> _oldMouseButtons = new List<Inputs>();

		public static int scroll { get; set; }
		static int _mouseWheelAdditionPrev;
		#endregion

		#region Keyboard
		public static string keyboardString { get; set; }

		public static Keys keyboardKey { get; set; } = Keys.None;

		public static Keys keyboardLastKey { get; set; } = Keys.None;

		public static char keyboardLastChar { get; set; } = ' ';

		static StringBuilder _keyboardBuffer = new StringBuilder();
		static Keys _keyboardLastKeyBuffer = Keys.None;
		static List<Keys> _currentKeys = new List<Keys>();
		static List<Keys> _oldKeys = new List<Keys>();

		#endregion

		#region Gamepad
		public static double gamepadTriggersDeadzone = 0.5;

		public static GamePadDeadZone gamepadDeadzoneType = GamePadDeadZone.Circular;

		static int _maxGamepadCount = 2;

		public static int maxGamepadCount
		{
			get => _maxGamepadCount;
			set
			{
				_maxGamepadCount = Math.Min(GamePad.MaximumGamePadCount, value);
				GamepadInit();
			}
		}

		static List<Inputs>[] _gamepadButtons = new List<Inputs>[_maxGamepadCount];
		static List<Inputs>[] _oldGamepadButtons = new List<Inputs>[_maxGamepadCount];

		static GamePadState[] _gamepadState = new GamePadState[_maxGamepadCount];
		static GamePadState[] _oldGamepadState = new GamePadState[_maxGamepadCount];
		#endregion

		const int _keyboardMaxCode = 1000;
		const int _mouseMaxCode = 2000;
		const int _gamepadMaxCode = 3000;

		static bool _mouseCleared, _keyboardCleared, _gamepadCleared;

		public static void Update()
		{
			_mouseCleared = false;
			_keyboardCleared = false;
			_gamepadCleared = false;

			#region Mouse
			MouseState mouseState = Mouse.GetState();

			mousePosition = camera.WindowToCameraPos(mouseState.Position);

			_oldMouseButtons = _mouseButtons;
			_mouseButtons = new List<Inputs>();

			if (mouseState.LeftButton == ButtonState.Pressed)
				_mouseButtons.Add(Inputs.MouseLeft);

			if (mouseState.RightButton == ButtonState.Pressed)
				_mouseButtons.Add(Inputs.MouseRight);

			if (mouseState.MiddleButton == ButtonState.Pressed)
				_mouseButtons.Add(Inputs.MouseMiddle);

			scroll = _mouseWheelAdditionPrev - mouseState.ScrollWheelValue;
			_mouseWheelAdditionPrev = mouseState.ScrollWheelValue;
			#endregion

			#region Keyboard
			keyboardString = _keyboardBuffer.ToString();
			_keyboardBuffer.Clear();

			if (keyboardString.Length > 0)
			{
				keyboardLastChar = keyboardString[keyboardString.Length - 1];
			}

			keyboardLastKey = _keyboardLastKeyBuffer;

			_oldKeys.Clear();
			_oldKeys.AddRange(_currentKeys);
			_currentKeys.Clear();
			_currentKeys.AddRange(Keyboard.GetState().GetPressedKeys());

			if (_currentKeys.Count > 0)
			{
				keyboardKey = _currentKeys[_currentKeys.Count - 1];
			}
			else
			{
				keyboardKey = Keys.None;
			}
			#endregion

			#region Gamepad
			for (var i = 0; i < maxGamepadCount; i += 1)
			{
				_oldGamepadState[i] = _gamepadState[i];

				_gamepadState[i] = GamePad.GetState(i, gamepadDeadzoneType);

				_oldGamepadButtons[i] = _gamepadButtons[i];
				_gamepadButtons[i] = new List<Inputs>();

				if (_gamepadState[i].DPad.Left == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadLeft);
				if (_gamepadState[i].DPad.Right == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadRight);
				if (_gamepadState[i].DPad.Up == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadUp);
				if (_gamepadState[i].DPad.Down == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadDown);

				if (_gamepadState[i].Buttons.A == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadA);
				if (_gamepadState[i].Buttons.B == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadB);
				if (_gamepadState[i].Buttons.X == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadX);
				if (_gamepadState[i].Buttons.Y == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadY);

				if (_gamepadState[i].Buttons.LeftShoulder == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadLB);
				if (_gamepadState[i].Buttons.RightShoulder == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadRB);

				if (_gamepadState[i].Triggers.Left > gamepadTriggersDeadzone)
					_gamepadButtons[i].Add(Inputs.GamepadLT);
				if (_gamepadState[i].Triggers.Right > gamepadTriggersDeadzone)
					_gamepadButtons[i].Add(Inputs.GamepadRT);

				if (_gamepadState[i].Buttons.LeftStick == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadLS);
				if (_gamepadState[i].Buttons.RightStick == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadRS);

				if (_gamepadState[i].Buttons.Start == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.GamepadStart);
				if (_gamepadState[i].Buttons.Back == ButtonState.Pressed)
					_gamepadButtons[i].Add(Inputs.Select);
			}

			#endregion Gamepad.
		}

		#region Button checks
		public static bool CheckButton(Inputs button, int index = 0)
		{
			var buttonCode = (int)button;

			return
				index < _gamepadButtons.Length && !_gamepadCleared && _gamepadButtons[index].Contains(button)
				|| buttonCode < _keyboardMaxCode && !_keyboardCleared && _currentKeys.Contains((Keys)button)
				|| buttonCode < _mouseMaxCode && !_mouseCleared && _mouseButtons.Contains(button);
		}

		public static bool CheckButtonPress(Inputs button, int index = 0)
		{
			var buttonCode = (int)button;

			return
				index < _gamepadButtons.Length && !_gamepadCleared && _gamepadButtons[index].Contains(button) && !_oldGamepadButtons[index].Contains(button)
				|| buttonCode < _keyboardMaxCode && !_keyboardCleared && _currentKeys.Contains((Keys)button) && !_oldKeys.Contains((Keys)button)
				|| buttonCode < _mouseMaxCode && !_mouseCleared && _mouseButtons.Contains(button) && !_oldMouseButtons.Contains(button);
		}

		public static bool CheckButtonRelease(Inputs button, int index = 0)
		{
			var buttonCode = (int)button;

			return
			index < _gamepadButtons.Length && !_gamepadCleared && !_gamepadButtons[index].Contains(button) && _oldGamepadButtons[index].Contains(button)
				|| buttonCode < _keyboardMaxCode && !_keyboardCleared && !_currentKeys.Contains((Keys)button) && _oldKeys.Contains((Keys)button)
				|| buttonCode < _mouseMaxCode && !_mouseCleared && !_mouseButtons.Contains(button) && _oldMouseButtons.Contains(button);
		}
		#endregion

		#region Mouse
		public static void ClearMouseInput() => _mouseCleared = true;
		#endregion

		#region Keyboard
		public static bool KeyboardCheckAnyKey() => !_keyboardCleared && _currentKeys.Count > 0;

		public static bool KeyboardCheckAnyKeyPress() => !_keyboardCleared && _currentKeys.Count > 0 && _oldKeys.Count == 0;

		public static bool KeyboardCheckAnyKeyRelease() => !_keyboardCleared && _currentKeys.Count == 0 && _oldKeys.Count > 0;

		public static void ClearKeyboardInput() => _keyboardCleared = true;

		public static void TextInput(TextInputEventArgs args)
		{
			_keyboardBuffer.Append(args.Character);
			_keyboardLastKeyBuffer = args.Key;
		}

		#endregion

		#region Gamepad
		internal static void GamepadInit()
		{
			_gamepadButtons = new List<Inputs>[_maxGamepadCount];
			for (var i = 0; i < _gamepadButtons.Length; i += 1)
				_gamepadButtons[i] = new List<Inputs>();
			_oldGamepadButtons = new List<Inputs>[_maxGamepadCount];
			for (var i = 0; i < _oldGamepadButtons.Length; i += 1)
				_oldGamepadButtons[i] = new List<Inputs>();

			_gamepadState = new GamePadState[_maxGamepadCount];
			for (var i = 0; i < _gamepadState.Length; i += 1)
				_gamepadState[i] = new GamePadState();
			_oldGamepadState = _gamepadState;
		}

		public static bool GamepadConnected(int index)
		{
			if (!_gamepadCleared && index < _gamepadState.Length)
			{
				return _gamepadState[index].IsConnected;
			}
			return false;
		}

		public static Vector2 GamepadGetLeftStick(int index)
		{
			if (!_gamepadCleared && index < _gamepadState.Length)
			{
				return _gamepadState[index].ThumbSticks.Left;
			}
			return Vector2.zero;
		}

		public static Vector2 GamepadGetRightStick(int index)
		{
			if (!_gamepadCleared && index < _gamepadState.Length)
			{
				return _gamepadState[index].ThumbSticks.Right;
			}
			return Vector2.zero;
		}

		public static float GamepadGetLeftTrigger(int index)
		{
			if (!_gamepadCleared && index < _gamepadState.Length)
			{
				return _gamepadState[index].Triggers.Left;
			}
			return 0;
		}

		public static float GamepadGetRightTrigger(int index)
		{
			if (!_gamepadCleared && index < _gamepadState.Length)
			{
				return _gamepadState[index].Triggers.Right;
			}
			return 0;
		}

		public static void GamepadSetVibration(int index, float leftMotor, float rightMotor) => GamePad.SetVibration(index, leftMotor, rightMotor);

		public static void ClearGamepadInput() => _gamepadCleared = true;
		#endregion

		public static void ClearInput()
		{
			ClearMouseInput();
			ClearKeyboardInput();
			ClearGamepadInput();
		}
	}
}