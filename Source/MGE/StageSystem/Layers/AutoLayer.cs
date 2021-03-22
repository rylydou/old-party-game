using System.Collections.Generic;
using System.IO;
using MGE.Debug;
using MGE.Debug.Menus;
using MGE.FileIO;
using MGE.UI;
using MGE.UI.Layouts;

namespace MGE.StageSystem.Layers
{
	[System.Serializable]
	public class AutoLayer : StageLayer
	{
		public int refIntGrid = 0;
		public bool isRefIntGridValid
		{
			get => refIntGrid >= 0 && refIntGrid < stage.layers.Count && stage.layers[refIntGrid] is IntLayer;
		}

		public int refIntGridIndex = 1;
		public bool isRefIntGridIndexValid
		{
			get => isRefIntGridValid && refIntGridIndex >= 0 && refIntGridIndex < (stage.layers[refIntGrid] as IntLayer).colors.Count;
		}

		public IntLayer intGrid
		{
			get => isRefIntGridValid && isRefIntGridIndexValid ? stage.layers[refIntGrid] as IntLayer : null;
		}

		public string tilesetPath = "Sprites/Tilesets/Basic";
		[System.NonSerialized] public Tileset tileset;

		public Grid<RectInt?> tiles;

		protected override void Editor_Create()
		{
			name = "Auto Layer";

			ReloadTileset(false);
		}

		public override void OnDeserilize()
		{
			base.OnDeserilize();

			ReloadTileset(false);
		}

		public override void Editor_Update(ref GUI gui)
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
					isRefIntGridIndexValid ? intGrid.colors[refIntGridIndex] : Color.clear,
					isRefIntGridIndexValid ? intGrid.colors[refIntGridIndex].readableColor : Colors.error
				))
				{
					case PointerInteraction.LClick:
						refIntGridIndex--;
						break;
					case PointerInteraction.RClick:
						refIntGridIndex++;
						break;
				}

				if (gui.ButtonClicked(tilesetPath, new Rect(layout.newElement, gui.rect.width, layout.currentSize)))
				{
					Menuing.OpenMenu(new DMenuFileSelect(
						"Select a Tileset",
						new FileInfo(IO.ParsePath("//" + tilesetPath, true)).Directory.FullName + "/",
						(x) =>
						{
							tilesetPath = Folder.assets.GetRelitivePath(x).Replace(".tileset.psd", string.Empty);
							ReloadTileset(true);
						},
						"*.tileset.psd"
					));
				}
			}
		}

		public override void Editor_Draw(Vector2 pan, float zoom)
		{
			if (!isRefIntGridValid || !isRefIntGridIndexValid || tileset == null) return;

			tiles = new Grid<RectInt?>(stage.size);

			tileset.GetTiles(ref tiles, (x, y) => intGrid.tiles.Get(x, y) == refIntGridIndex);

			tileset.DrawTiles(in tiles, pan, zoom * stage.tileSize, Color.white);
		}

		public void ReloadTileset(bool isFromUser)
		{
			tileset = Assets.GetAsset<Tileset>(tilesetPath);
		}
	}
}