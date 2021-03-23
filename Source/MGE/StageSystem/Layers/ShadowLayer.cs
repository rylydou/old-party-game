using System;
using MGE.Debug;
using MGE.Debug.Menus;
using MGE.Graphics;
using MGE.UI;
using MGE.UI.Layouts;

namespace MGE.StageSystem.Layers
{
	[System.Serializable]
	public class ShadowLayer : StageLayer
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

		public Color color;
		public bool useSmartDrawing;

		public bool[] shadowCache;

		protected override void Editor_Create()
		{
			name = "Shadow Layer";

			color = new Color(0, 0.25f);
			useSmartDrawing = true;

			shadowCache = new bool[stage.size.sqrMagnitude];
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
						Reload();
						break;
					case PointerInteraction.RClick:
						refIntGrid++;
						Reload();
						break;
				}

				switch (
					gui.ColoredButton(
					$"Color: {refIntGridIndex}",
					new Rect(layout.newElement, gui.rect.width, layout.currentSize),
					isRefIntGridIndexValid ? intGrid.colors[refIntGridIndex] : Color.clear,
					isRefIntGridIndexValid ? intGrid.colors[refIntGridIndex].readableColor : Colors.error
				))
				{
					case PointerInteraction.LClick:
						refIntGridIndex--;
						Reload();
						break;
					case PointerInteraction.RClick:
						refIntGridIndex++;
						Reload();
						break;
				}

				if (gui.ColoredButton("Color", new Rect(0, layout.newElement.y, gui.rect.width, layout.currentSize), color) == PointerInteraction.LClick)
				{
					Menuing.OpenMenu(new DMenuTextInput("Enter Color Hex Code...", color.ToHex(), (x) => color = new Color(x), null, TextFeildRule.colorCode));
				}

				if (gui.Toggle("Use Smart Drawing?", new Rect(0, layout.newElement.y, gui.rect.width, layout.currentSize), ref useSmartDrawing))
					Reload();
			}
		}

		public override void Editor_Draw(Vector2 pan, float zoom)
		{
			if (!isRefIntGridValid || !isRefIntGridIndexValid) return;

			if (intGrid.lastChanged == Time.unscaledTime)
			{
				Reload();
			}

			for (int y = 0; y < stage.size.y; y++)
			{
				for (int x = 0; x < stage.size.x; x++)
				{
					if (shadowCache[y * stage.size.x + x])
						GFX.DrawBox(Scale(new Rect(x - 0.125f, y + 0.125f, 1, 1)), color);
				}
			}
		}

		public void Reload()
		{
			intGrid.tiles.For((x, y, tile) =>
				{
					shadowCache[y * stage.size.x + x] =
						useSmartDrawing ?
							intGrid.tiles.Get(x, y) == refIntGridIndex && (intGrid.tiles.Get(x - 1, y) == 0 || intGrid.tiles.Get(x - 1, y + 1) == 0 || intGrid.tiles.Get(x, y + 1) == 0) :
							intGrid.tiles.Get(x, y) == refIntGridIndex;
				});
		}
	}
}