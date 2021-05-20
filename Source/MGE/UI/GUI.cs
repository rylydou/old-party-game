using System.Collections.Generic;
using System.Text;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.UI.Elements;

namespace MGE.UI
{
	public class GUI
	{
		public const float padding = 4;

		static GUI _gui;
		public static GUI gui
		{
			get
			{
				if (_gui == null)
					_gui = new GUI(new Rect(Vector2.zero, Window.renderSize), Input.mouseIsInWindow);
				return _gui;
			}
		}

		public readonly Rect rect;
		public List<GUIElement> elements = new List<GUIElement>();

		bool active = true;

		internal static void Update()
		{
			gui.active = Input.mouseIsInWindow;
			gui.elements.Clear();
		}

		public GUI(Rect rect, bool active)
		{
			this.rect = rect;
			this.active = active;
		}

		public void Draw()
		{
			using (new DrawBatch(transform: null))
				foreach (var element in elements)
					element.Draw();
		}

		public void Rect(Rect rect, Color color, float thickness = 1)
		{
			AddElement(new GUIRect(thickness) { rect = rect, color = color });
		}

		public void Image(Rect rect, Color color)
		{
			AddElement(new GUIImage() { rect = rect, color = color });
		}

		public void Image(Rect rect, Texture texture, Color color)
		{
			AddElement(new GUIImage(texture) { rect = rect, color = color });
		}

		public void Text(string text, Vector2 position, Color color)
		{
			AddElement(new GUIText(text) { rect = new Rect(position.x, position.y, rect.size.x, 0), color = color, scale = 1 });
		}

		public void Text(string text, Rect rect, Color color)
		{
			AddElement(new GUIText(text) { rect = rect, color = color, scale = 1 });
		}

		public void Text(string text, Rect rect, Color color, float scale = 1.0f, TextAlignment alignment = TextAlignment.Left)
		{
			AddElement(new GUIText(text) { rect = rect, color = color, scale = scale, alignment = alignment });
		}

		public PointerInteraction ColoredButton(string text, Rect rect, Color bgColor, Color? textColor = null, bool highlight = false, float scale = 1, TextAlignment alignment = TextAlignment.Center)
		{
			if (!textColor.HasValue)
				textColor = Math.Approximately(bgColor.a, 0) ? Colors.text : bgColor.readableColor;

			var interaction = MouseInteraction(rect);

			Image(rect, bgColor);

			switch (interaction)
			{
				case PointerInteraction.Hover:
					Image(rect, Math.Approximately(bgColor.a, 0) ? Colors.highlight : new Color(0, 0.1f));
					break;
			}

			if (highlight)
				Image(new Rect(rect.center - new Vector2(rect.height / 2), new Vector2(rect.height)), bgColor.inverted.opaque);

			Text(text, rect, highlight ? textColor.Value.inverted : textColor.Value, scale, alignment);

			return interaction;
		}

		public PointerInteraction Button(string text, Rect rect, Color? color = null, TextAlignment alignment = TextAlignment.Center) =>
			ColoredButton(text, rect, Color.clear, color, false, alignment: alignment);

		public PointerInteraction Button(string text, float position, float size = 32, Color? color = null, TextAlignment alignment = TextAlignment.Center) =>
			Button(text, new Rect(0, position, rect.width, size), color, alignment);

		public bool ButtonClicked(string text, Rect rect, Color? color = null, TextAlignment alignment = TextAlignment.Center) =>
			Button(text, rect, color) == PointerInteraction.LClick;

		public bool ButtonClicked(string text, float position, float size = 32, Color? color = null, TextAlignment alignment = TextAlignment.Center) =>
			ButtonClicked(text, new Rect(0, position, rect.width, size), color, alignment);

		public bool Toggle(string text, Rect rect, ref bool state, Color? color = null)
		{
			if (ButtonClicked((state ? "[X] " : "[ ] ") + text, rect, color))
			{
				state = !state;
				return true;
			}
			return false;
		}

		public void TextFeild(ref TextFeildData data, Rect rect)
		{
			if (ButtonClicked(string.Empty, rect))
				data.isActive = !data.isActive;

			if (data.isActive)
			{
				var text = string.Empty;

				text = Input.keyboardString;

			// I know, I know
			UpdateInput:

				foreach (var letter in text)
				{
					switch (letter)
					{
						case (char)1:
							data.textBuilder.Clear();
							break;
						case (char)3:
							var sb = new StringBuilder();

							for (int i = data.selectionStart; i < data.selectionEnd; i++)
								sb.Append(data.textBuilder[i]);

							System.Windows.Forms.Clipboard.SetText(sb.ToString(), System.Windows.Forms.TextDataFormat.Text);
							break;
						case (char)13:
							goto case '\n';
						case (char)22:
							text = System.Windows.Forms.Clipboard.GetText(System.Windows.Forms.TextDataFormat.Text);
							goto UpdateInput;
						case (char)27:
							goto case '\n';
						case '\b':
							if (data.textBuilder.Length < 1) continue;
							data.textBuilder.Remove(data.cursorIndex - 1, 1);
							data.cursorIndex--;
							break;
						case '\n':
							data.isActive = false;
							return;
						default:
							data.textBuilder.Insert(data.cursorIndex, letter);
							data.cursorIndex++;
							break;
					}

					// Logger.Log((int)letter);

					data.Typed();
				}

				if (Input.GetButtonPress(Inputs.Left))
				{
					data.cursorIndex--;

					data.Typed();
				}
				else if (Input.GetButtonPress(Inputs.Right))
				{
					data.cursorIndex++;

					data.Typed();
				}

				data.cursorIndex = Math.Clamp(data.cursorIndex, 0, data.textBuilder.Length);

				float alpha = 1f;

				if (Time.unscaledTime - data.lastTimeTyped > 0.25f)
					alpha = 1 - Math.Tan(data.blinkOffset + Time.unscaledTime * 4f);

				Image(new Rect(padding + rect.position.x + data.cursorIndex * Config.font.charPaddingSize.x, rect.position.y, -2, rect.height), Colors.accent.ChangeAlpha(alpha));
			}

			Text(data.text, new Rect(rect.position.x + padding, rect.position.y, rect.width - padding * 2, rect.height), Color.white, 1);

			if (data.isActive)
				Rect(rect, Colors.accent.ChangeAlpha(Math.Abs(Math.Sin(Time.unscaledTime * 2)) + 0.5f));
		}

		public void AddElement(GUIElement element)
		{
			element.rect.position += rect.position;

			elements.Add(element);
		}

		public bool MouseInside(Rect rect)
		{
			if (!active) return false;
			rect.position += this.rect.position;
			return rect.Contains(Input.windowMousePosition);
		}

		public bool MouseClick(Rect rect)
		{
			if (!active) return false;
			rect.position += this.rect.position;
			return rect.Contains(Input.windowMousePosition) && Input.GetButtonPress(Inputs.MouseLeft);
		}

		public PointerInteraction MouseInteraction(Rect rect)
		{
			if (!active) return PointerInteraction.None;

			rect.position += this.rect.position;

			var interactions = PointerInteraction.None;

			if (rect.Contains(Input.windowMousePosition))
			{
				if (Input.GetButtonPress(Inputs.MouseLeft))
					interactions = PointerInteraction.LClick;
				else if (Input.GetButtonPress(Inputs.MouseRight))
					interactions = PointerInteraction.RClick;
				else if (Input.GetButtonPress(Inputs.MouseMiddle))
					interactions = PointerInteraction.MClick;
				else
					interactions = PointerInteraction.Hover;
			}

			return interactions;
		}
	}
}