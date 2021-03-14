using MGE.UI.Layouts;

namespace MGE.Debug.Menus
{
	public class DMenuAssets : DebugMenu
	{
		public override string name => "Assets";

		public override void UpdateBG()
		{
			base.UpdateBG();

			using (var layout = new StackLayout(offset, allSize, false))
			{
				gui.Text($"--- Preloaded ({Assets.preloadedAssets.Count}) ---", layout.AddElement(), Colors.text);

				foreach (var asset in Assets.preloadedAssets)
				{
					gui.Text($"{asset.Value.GetType()} - {asset.Key}", layout.AddElement(), Colors.text);
				}

				gui.Text($"--- Unloaded ({Assets.unloadedAssets.Count}) ---", layout.AddElement(), Colors.text);

				foreach (var asset in Assets.unloadedAssets)
				{
					gui.Text($"{asset.Key}", layout.AddElement(), Colors.text);
				}
			}
		}
	}
}