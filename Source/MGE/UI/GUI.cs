using System.Collections.Generic;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.UI.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.UI
{
	public class GUI
	{
		static List<GUIElement> elements = new List<GUIElement>();

		public static int uiDraws { get; internal set; }

		internal static void Update()
		{
			elements = new List<GUIElement>();
		}

		internal static void Draw()
		{
			using (new DrawBatch(transform: null))
			{
				uiDraws = 0;

				foreach (var draw in elements)
				{
					uiDraws++;
					draw.Draw();
				}

				elements = new List<GUIElement>();
			}
		}

		public static void Image(Rect rect, Color color)
		{
			AddElement(new GUIImage() { rect = rect, color = color });
		}

		public static void Image(Rect rect, Texture2D texture, Color color)
		{
			AddElement(new GUIImage(texture) { rect = rect, color = color });
		}

		public static void Text(string text, Vector2 position, Color color)
		{
			AddElement(new GUIText(text) { rect = new Rect(position, Vector2.zero), color = color });
		}

		public static bool MouseInside(Rect rect)
		{
			return rect.Contains(Input.windowMousePosition);
		}

		public static bool MouseClick(Rect rect)
		{
			return rect.Contains(Input.windowMousePosition) && Input.GetButtonPress(Inputs.MouseLeft);
		}

		public static void AddElement(GUIElement element)
		{
			elements.Add(element);
		}
	}
}