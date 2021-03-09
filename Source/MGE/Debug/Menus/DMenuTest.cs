namespace MGE.Debug.Menus
{
	public class DMenuTest : DebugMenu
	{
		const int offset = 16;

		public override string name => "Test Menu";

		public override void UpdateBG()
		{
			base.UpdateBG();

			gui.Text($"Position: {position}", new Vector2(offset, offset), Colors.text);
			gui.Text($"Size: {size}", new Vector2(offset, offset + 32), Colors.text);
		}
	}
}