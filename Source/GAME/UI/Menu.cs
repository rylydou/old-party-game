using System;
using System.Collections.Generic;
using MGE;
using MGE.UI;

namespace GAME.UI
{
	public class Menu
	{
		public class Option
		{
			public Func<string> textGen;
			string _text;
			public string text
			{
				get
				{
					if (string.IsNullOrEmpty(_text))
						_text = textGen.Invoke();
					return _text;
				}
				internal set => _text = value;
			}
			public Action onClick;

			public Option(Func<string> text, Action onClick)
			{
				this.textGen = text;
				this.onClick = onClick;
			}
		}

		const float fontSize = 1.5f;
		const float spaceBetweenOptions = 42;
		const float leftMarginForOptions = 24;
		static readonly Color normalColor = new Color("#EEE");
		static readonly Color selectedColor = new Color("#FB3");
		static readonly Color disabledColor = new Color("#999");
		static readonly Color disabledSelectedColor = new Color("#AAA");
		static Font font { get => Config.font; }

		public string title = string.Empty;
		public Option[] options;
		public Action<Menu> onClose = (m) => { };

		public int cursorPosition = 0;

		public Menu(string title, params (Func<string>, Action)[] options)
		{
			this.title = title;

			var optionsList = new List<Option>();

			foreach (var option in options)
				optionsList.Add(new Option(option.Item1, option.Item2));

			this.options = optionsList.ToArray();
		}

		public void Update()
		{
			if (GameSettings.mainController.back)
			{
				onClose.Invoke(this);
				MenuManager.GoBack();
			}
			else if (GameSettings.mainController.select)
			{
				var option = options[cursorPosition];
				if (option.onClick is object)
				{
					options[cursorPosition].onClick.Invoke();
					MenuManager.onOptionSelect.Invoke(option);
					option.text = string.Empty;
				}
				else
				{
					MenuManager.onOptionError.Invoke(option);
				}
			}
			else if (GameSettings.mainController.up)
			{
				if (cursorPosition > 0)
				{
					cursorPosition--;
					MenuManager.onOptionChange.Invoke(options[cursorPosition]);
				}
			}
			else if (GameSettings.mainController.down)
			{
				if (cursorPosition < options.Length - 1)
				{
					cursorPosition++;
					MenuManager.onOptionChange.Invoke(options[cursorPosition]);
				}
			}
		}

		public void Draw()
		{
			var startPos = (GUI.canvasSize.y - options.Length * spaceBetweenOptions) / 2;

			var index = 0;
			foreach (var option in options)
			{
				var text = (index == cursorPosition ? "- " : "  ") + option.text;
				var pos = new Vector2(leftMarginForOptions, startPos + index * spaceBetweenOptions);

				var color = Color.nullColor;

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

			if (string.IsNullOrEmpty(title)) return;

			var titleText = $"  [ {title} ]";

			font.DrawText(titleText, new Vector2(leftMarginForOptions, startPos - spaceBetweenOptions) + 2, new Color(0, 0.25f), fontSize);
			font.DrawText(titleText, new Vector2(leftMarginForOptions, startPos - spaceBetweenOptions), selectedColor, fontSize);
		}
	}
}