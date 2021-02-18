using System.Collections.Generic;
using MGE.Graphics;
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

		public static void AddElement(GUIElement element)
		{
			elements.Add(element);
		}
	}
}