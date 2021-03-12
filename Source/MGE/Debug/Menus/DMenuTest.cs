namespace MGE.Debug.Menus
{
	public class DMenuTest : DebugMenu
	{
		public override string name => "Test Menu";

		public override void UpdateBG()
		{
			base.UpdateBG();

			gui.Text($"Position: {position}", offset, Colors.text);
			gui.Text($"Size: {size}", offset + new Vector2(0, allSize), Colors.text);
		}
	}
}