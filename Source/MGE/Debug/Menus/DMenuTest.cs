using System.Text;
using MGE.UI;

namespace MGE.Debug.Menus
{
	public class DMenuTest : DebugMenu
	{
		public override string name => "Test Menu";
		TextFeildData data = new TextFeildData();
		StringBuilder sb = new StringBuilder();

		public override void UpdateBG()
		{
			base.UpdateBG();

			gui.Text($"Position: {position}", offset, Colors.text);
			gui.Text($"Size: {size}", offset + new Vector2(0, allSize), Colors.text);

			gui.TextFeild(ref data, new Rect(0, 64, size.x, 32));
		}
	}
}