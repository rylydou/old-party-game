using System;
using MGE.Components;
using MGE.StageSystem;
using MGE.UI.Layouts;

namespace MGE.Debug.Menus
{
	public class DMenuNewLayer : DebugMenu
	{
		public override string name => "Add New Layer";

		public override void UpdateBG()
		{
			base.UpdateBG();

			using (var layout = new StackLayout(Vector2Int.zero, allSize, false))
			{
				foreach (var layer in Config.availableLayers)
				{
					var text = layer.ToString();
					if (gui.ButtonClicked(text.Substring(text.LastIndexOf('.') + 1), new Rect(layout.newElement, new Vector2(size.x, layout.currentSize))))
					{
						CEditor.current.level.LayerAdd((LevelLayer)Activator.CreateInstance(layer));
						Close();
					}
				}
			}
		}
	}
}