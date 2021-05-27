using System.Collections.Generic;
using MGE.Graphics;

namespace MGE.UI
{
	public class GUIElement
	{
		public bool dirty { get; private set; }

		Rect _rect;
		public Rect rect
		{
			get => _rect; set
			{
				if (_rect != value)
				{
					_rect = value;
					dirty = true;
				}
			}
		}

		public Rect realRect { get; private set; }

		public Color backgroundColor;

		public GUIElement parent { get; internal set; }

		public List<GUIElement> elements { get; private set; }

		public GUIElement(Rect rect, params GUIElement[] elements)
		{
			this.rect = rect;
			this.elements = new List<GUIElement>();

			if (elements is object)
				foreach (var element in elements)
				{
					element.parent = this;
					this.elements.Add(element);
				}
		}

		public void Init()
		{
			foreach (var element in elements)
			{
				element.Init();
			}
		}

		public void Update()
		{
			if (!dirty) return;
			dirty = false;

			OnUpdate();

			foreach (var element in elements)
			{
				element.OnUpdate();
			}
		}

		protected virtual void OnUpdate() { }

		public void Draw()
		{
			GFX.DrawBox(realRect, backgroundColor);
			OnDraw();

			foreach (var element in elements)
			{
				element.Draw();
			}
		}
		protected virtual void OnDraw() { }

		public void DirtyChildren()
		{
			foreach (var element in elements)
			{
				dirty = true;
				element.DirtyChildren();
			}
		}
	}
}