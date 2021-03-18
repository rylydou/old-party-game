namespace MGE.StageSystem.Layers
{
	public class AutoLayer : StageLayer
	{
		public Grid<Rect> tiles;

		protected override void OnInit(Vector2Int size)
		{
			name = "Auto Layer";

			tiles = new Grid<Rect>(size);
		}
	}
}