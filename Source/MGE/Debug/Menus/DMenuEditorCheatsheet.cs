using MGE.UI.Layouts;

namespace MGE.Debug.Menus
{
	public class DMenuEditorCheatsheet : DebugMenu
	{
		public override string name => "Cheatsheet";

		public override void UpdateBG()
		{
			base.UpdateBG();

			using (var layout = new StackLayout(offset, allSize))
			{
				gui.Text("--- General ---", new Rect(layout.newElement, new Vector2(gui.rect.width, layout.currentSize)), Colors.text, 1, TextAlignment.Center);
				gui.Text("Select - Left Click", layout.newElement, Colors.text);
				gui.Text("Delete - Middle Click", layout.newElement, Colors.text);
				gui.Text("Save - Ctrl + S", layout.newElement, Colors.text);
				gui.Text("Load - Ctrl + L", layout.newElement, Colors.text);
				gui.Text("Toggle Isolate Active Layer - Right Click", layout.newElement, Colors.text);

				gui.Text("--- Layers (Left Panel) ---", new Rect(layout.newElement, new Vector2(gui.rect.width, layout.currentSize)), Colors.text, 1, TextAlignment.Center);
				gui.Text("Toggle Visibility - Right Click", layout.newElement, Colors.text);
				gui.Text("Move Layer Selection Up - Up Arrow", layout.newElement, Colors.text);
				gui.Text("Move Layer Selection Down - Down Arrow", layout.newElement, Colors.text);
				gui.Text("Move Selected Layer Up - Alt + Up Arrow", layout.newElement, Colors.text);
				gui.Text("Move Selected Layer Down - Alt + Down Arrow", layout.newElement, Colors.text);
				gui.Text("Copy Selected Layer Up - Alt + Shift + Up Arrow", layout.newElement, Colors.text);
				gui.Text("Copy Selected Layer Down - Alt + Shift + Down Arrow", layout.newElement, Colors.text);
			}
		}
	}
}