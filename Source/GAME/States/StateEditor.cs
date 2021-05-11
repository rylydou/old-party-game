using GAME.Components;
using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.InputSystem;

namespace GAME.States
{
	public class StateEditor : GameState
	{
		public byte selectedTile = 1;

		bool showGrid = false;

		Color lineColor;

		public override void Init()
		{
			base.Init();

			SceneManager.QueueScene(
				new Scene(
					new Layer(
						false,
						new Entity(new CBackground())
					),
					new Layer(
						false,
						new Entity(new CStage())
					)
				)
			);

			lineColor = new Color("#FB3").ChangeAlpha(0.25f);
		}

		public override void Update()
		{
			base.Update();

			selectedTile = (byte)Math.Clamp(selectedTile - Input.scroll, 1, Stage.tilesets.Count - 1);

			var shift = Input.GetButton(Inputs.LeftShift) | Input.GetButton(Inputs.RightShift);
			var ctrl = Input.GetButton(Inputs.LeftControl) | Input.GetButton(Inputs.RightControl);
			var alt = Input.GetButton(Inputs.LeftAlt) | Input.GetButton(Inputs.RightAlt);

			if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.G))
				showGrid = !showGrid;
			else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
				GameSettings.current.stage.tiles.Set((Vector2Int)Input.cameraMousePosition, selectedTile);
			else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
				GameSettings.current.stage.tiles.Set((Vector2Int)Input.cameraMousePosition, 0);
		}

		public override void Draw()
		{
			base.Draw();

			if (showGrid)
			{
				for (int x = 0; x < GameSettings.current.stage.tiles.width + 1; x++)
				{
					GFX.DrawBox(new Rect(x, 0, GFX.currentUnitsPerPixel, GameSettings.current.stage.tiles.height), lineColor);
				}

				for (int y = 0; y < GameSettings.current.stage.tiles.height + 1; y++)
				{
					GFX.DrawBox(new Rect(0, y, GameSettings.current.stage.tiles.width, GFX.currentUnitsPerPixel), lineColor);
				}

				GFX.DrawBox(new Rect(40 / 2, 0, GFX.currentUnitsPerPixel * 2, GameSettings.current.stage.tiles.height), lineColor);
				GFX.DrawBox(new Rect(0, (float)23 / 2, GameSettings.current.stage.tiles.width, GFX.currentUnitsPerPixel * 2), lineColor);
			}
		}

		public override void DrawUI()
		{
			base.DrawUI();

			if (showGrid)
			{
				GFX.DrawBox(new Rect(Input.windowMousePosition.x, 0, GFX.currentUnitsPerPixel, Window.renderSize.y), lineColor);
				GFX.DrawBox(new Rect(0, Input.windowMousePosition.y, Window.renderSize.x, GFX.currentUnitsPerPixel), lineColor);
			}

			using (var layout = new MGE.UI.Layouts.StackLayout(new Vector2(16), 24, false))
			{
				Config.font.DrawText(GameSettings.current.stage.name, layout.newElement, Color.white);
				layout.AddElement();

				var tilesetText = $"{selectedTile}. {Stage.tilesets[selectedTile].name}";

				Config.font.DrawText(tilesetText, layout.newElement + 2, Stage.tilesets[selectedTile].color);
				Config.font.DrawText(tilesetText, layout.currentElement, Stage.tilesets[selectedTile].color.inverted.readableColor);
			}
		}
	}
}