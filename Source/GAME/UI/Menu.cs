using System;
using System.Collections.Generic;
using MGE;

namespace GAME.UI
{
	public class Menu
	{
		public class Option
		{
			public bool enabled;

			public string text;

			public Action onClick;
		}

		const float optionFontSize = 4;
		const float optionSpaceBetween = 64;
		const float optionSideMargin = 64;
		static readonly Color optionColorNormal = new Color("#EEE");
		static readonly Color optionColorDisabled = new Color("#555");
		static readonly Color optionColorSelected = new Color("#FB3");
		static Font font;
		const float animDuration = 0.5f;

		static EController mainPlayer = EController.WASD;

		public List<Option> options = new List<Option>();

		public Vector2 position;

		public int cursorPosition = 0;

		float animTime;

		public void Update()
		{
			if (font is null) font = Config.font;

			var con = GameSettings.current.controllers[mainPlayer];

			if (con.select)
			{
				options[cursorPosition].onClick.Invoke();
			}
			else if (con.up)
			{
				if (cursorPosition > 0)
				{
					cursorPosition--;
					animTime = animDuration;
				}
			}
			else if (con.up)
			{
				if (cursorPosition < options.Count)
				{
					cursorPosition++;
					animTime = animDuration;
				}
			}
		}

		public void Draw()
		{
			var startPos = (Window.renderSize.y - options.Count * optionSpaceBetween) / 2 + position.y;

			var index = 0;
			foreach (var option in options)
			{
				font.DrawText(option.text, new Vector2(position.x, startPos + index * optionSpaceBetween), index == cursorPosition ? optionColorSelected : optionColorNormal, optionFontSize)

				index++;
			}
		}
	}
}