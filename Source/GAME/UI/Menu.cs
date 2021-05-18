using System;
using System.Collections.Generic;
using MGE;

namespace GAME.UI
{
	public class Menu
	{
		public class Option
		{
			public Func<string> text;
			public Action onClick;

			public Option(Func<string> text, Action onClick)
			{
				this.text = text;
				this.onClick = onClick;
			}
		}

		const float fontSize = 1.5f;
		const float spaceBetweenOptions = 42;
		const float leftMarginForOptions = 24;
		static readonly Color normalColor = new Color("#EEE");
		static readonly Color selectedColor = new Color("#FB3");
		static readonly Color disabledColor = new Color("#444");
		static readonly Color disabledSelectedColor = new Color("#888");
		static Font font { get => Config.font; }

		public string title = string.Empty;
		public List<Option> options = new List<Option>();

		public int cursorPosition = 0;

		public Menu(params (Func<string>, Action)[] options)
		{
			foreach (var option in options)
				this.options.Add(new Option(option.Item1, option.Item2));
		}

		public Menu(string title, params (Func<string>, Action)[] options)
		{
			this.title = title;

			foreach (var option in options)
				this.options.Add(new Option(option.Item1, option.Item2));
		}

		public void Update()
		{
			if (GameSettings.mainController.back)
				MenuManager.GoBack();
			else if (GameSettings.mainController.select)
			{
				var option = options[cursorPosition];
				if (option.onClick is object)
					options[cursorPosition].onClick.Invoke();
			}
			else if (GameSettings.mainController.up)
			{
				if (cursorPosition > 0)
					cursorPosition--;
			}
			else if (GameSettings.mainController.down)
			{
				if (cursorPosition < options.Count - 1)
					cursorPosition++;
			}
		}

		public void Draw()
		{
			var startPos = (Window.renderSize.y - options.Count * spaceBetweenOptions) / 2;

			var index = 0;
			foreach (var option in options)
			{
				var text = (index == cursorPosition ? "- " : "  ") + option.text?.Invoke();
				var pos = new Vector2(leftMarginForOptions, startPos + index * spaceBetweenOptions);

				var color = Color.red;

				if (index == cursorPosition)
				{
					if (option.onClick is object)
						color = selectedColor;
					else
						color = disabledSelectedColor;
				}
				else
				{
					if (option.onClick is object)
						color = normalColor;
					else
						color = disabledColor;
				}

				font.DrawText(text, pos + 2, new Color(0, 0.25f), fontSize);
				font.DrawText(text, pos, color, fontSize);

				index++;
			}

			font.DrawText($"[ {title} ]", new Vector2(leftMarginForOptions, startPos - spaceBetweenOptions) + 2, new Color(0, 0.25f), fontSize);
			font.DrawText($"[ {title} ]", new Vector2(leftMarginForOptions, startPos - spaceBetweenOptions), selectedColor, fontSize);
		}
	}
}