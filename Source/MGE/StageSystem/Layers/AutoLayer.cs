using MGE.UI;
using MGE.UI.Layouts;

namespace MGE.StageSystem.Layers
{
	[System.Serializable]
	public class AutoLayer : StageLayer
	{
		public int refIntGrid = -1;
		public bool isRefIntGridValid
		{
			get => refIntGrid >= 0 && refIntGrid < stage.layers.Count && stage.layers[refIntGrid] is IntLayer;
		}

		public int refIntGridIndex = -1;
		public bool isRefIntGridIndexValid
		{
			get => isRefIntGridValid && refIntGridIndex >= 0 && refIntGridIndex < (stage.layers[refIntGrid] as IntLayer).colors.Count;
		}

		public IntLayer intGrid
		{
			get => isRefIntGridValid && isRefIntGridIndexValid ? stage.layers[refIntGrid] as IntLayer : null;
		}

		public Tileset tileset;

		public Grid<RectInt?> tiles;

		protected override void OnInit()
		{
			name = "Auto Layer";

			tileset = Assets.GetAsset<Tileset>("Sprites/Tilesets/Grass");
		}

		public override void Update(ref GUI gui)
		{
			using (var layout = new StackLayout(new Vector2(0, offset), itemSize, false))
			{
				switch (gui.Button(
					$"Grid: {refIntGrid}",
					layout.newElement.y,
					layout.currentSize,
					isRefIntGridValid ? Colors.text : Colors.error
				))
				{
					case PointerInteraction.LClick:
						refIntGrid--;
						break;
					case PointerInteraction.RClick:
						refIntGrid++;
						break;
				}

				switch (
					gui.ColoredButton(
					$"Color: {refIntGridIndex}",
					new Rect(layout.newElement, new Vector2(gui.rect.width, layout.currentSize)),
					isRefIntGridIndexValid ? intGrid.colors[refIntGridIndex] : Colors.error
				))
				{
					case PointerInteraction.LClick:
						refIntGridIndex--;
						break;
					case PointerInteraction.RClick:
						refIntGridIndex++;
						break;
				}
			}
		}

		public override void Draw(Vector2 pan, float zoom)
		{
			if (!isRefIntGridValid || !isRefIntGridIndexValid) return;

			tiles = new Grid<RectInt?>(stage.size);

			tileset.GetTiles(ref tiles, (x, y) => intGrid.tiles.Get(x, y) == refIntGridIndex);

			tileset.DrawTiles(in tiles, pan, zoom * stage.tileSize, Color.white);
		}
	}
}