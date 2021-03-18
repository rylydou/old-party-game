using System.Collections.Generic;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.UI.Elements;

namespace MGE.UI
{
	public class GUI
	{
		static GUI _gui;
		public static GUI gui
		{
			get
			{
				if (_gui == null)
					_gui = new GUI(new Rect(Vector2.zero, Window.windowedSize), Input.mouseIsInWindow);
				return _gui;
			}
		}

		public readonly Rect rect;
		List<GUIElement> elements = new List<GUIElement>();

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

		public PointerInteraction Button(string text, Rect rect, TextAlignment alignment = TextAlignment.Center)
		{
			var interaction = MouseInteraction(rect);

			switch (interaction)
			{
				case PointerInteraction.Hover:
					Image(rect, Colors.highlight);
					break;
				case PointerInteraction.LClick:
					Image(rect, Colors.accent2);
					break;
			}

			Text(text, rect, Colors.text, 1, alignment);

			return interaction;
		}

		public bool ButtonClicked(string text, Rect rect, TextAlignment alignment = TextAlignment.Center) =>
			Button(text, rect, alignment) == PointerInteraction.LClick;

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