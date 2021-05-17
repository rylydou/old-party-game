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

		const float fontSize = 1.0f;
		const float spaceBetweenOptions = 32;
		const float leftMarginForOptions = 16;
		static readonly Color normalColor = new Color("#EEE");
		static readonly Color disabledColor = new Color("#555");
		static readonly Color selectedColor = new Color("#FB3");
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
			if (GameSettings.current.mainController.back)
				MenuManager.GoBack();
			else if (GameSettings.current.mainController.select)
			{
				var option = options[cursorPosition];
				if (option.onClick is object)
					options[cursorPosition].onClick.Invoke();
			}
			else if (GameSettings.current.mainController.up)
			{
				if (cursorPosition > 0)
					cursorPosition--;
			}
			else if (GameSettings.current.mainController.down)
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

				if (index == cursorPosition)
					font.DrawText(text, pos + 2, selectedColor, fontSize);
				else
					font.DrawText(text, pos + 2, new Color(0, 0.25f), fontSize);

				font.DrawText(text, pos, option.onClick is object ? normalColor : disabledColor, fontSize);

				index++;
			}

			font.DrawText($"[ {title} ]", new Vector2(leftMarginForOptions, startPos - spaceBetweenOptions) + 2, normalColor, fontSize);
			font.DrawText($"[ {title} ]", new Vector2(leftMarginForOptions, startPos - spaceBetweenOptions), selectedColor, fontSize);
		}
	}
}