using System.Collections.Generic;
using System;

namespace MGE.UI.Elements
{
	public class GUIStackLayout : GUIElement
	{
		List<GUIElement> elements = new List<GUIElement>();

		public readonly bool isHorizontal;
		public readonly int itemSize;

		public GUIStackLayout(List<GUIElement> elements, int itemSize, bool isHorizontal = false)
		{
			if (elements == null) throw new NullReferenceException("'elements' cannot be null!");

			this.elements = elements;
			this.itemSize = itemSize;
			this.isHorizontal = isHorizontal;
		}

		public override void Draw()
		{
			for (int i = 0; i < elements.Count; i++)
			{
				if (isHorizontal)
					elements[i].rect.position = rect.position + new Vector2(i * itemSize, 0);
				else
					elements[i].rect.position = rect.position + new Vector2(0, i * itemSize);

				elements[i].color *= color;
				elements[i].rect.width = rect.width;
				elements[i].rect.height = rect.height;

				GUI.uiDraws++;
				elements[i].Draw();
			}
		}
	}
}