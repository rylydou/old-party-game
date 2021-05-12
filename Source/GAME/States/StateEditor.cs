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
		Vector2 cursorPos;
		Vector2Int cursorTilePos;

		CFog fog;
		Stage stage;

		public override void Init()
		{
			base.Init();

			stage = GameSettings.current.stage;

			SceneManager.QueueScene(
				new Scene(
					new Layer(
						false,
						new Entity(new CBackground())
					),
					new Layer(
						false,
						new Entity(new CStage())
					),
					new Layer(
						false,
						new Entity(new CFog())
					)
				)
			);

			fog = SceneManager.activeScene.layers[2].GetEntityWithComponent<CFog>().GetComponent<CFog>();
		}

		public override void Update()
		{
			base.Update();

			cursorPos = Input.cameraMousePosition;
			cursorTilePos = cursorPos;

			selectedTile = (byte)Math.Clamp(selectedTile + Input.scroll, 1, Stage.tilesets.Length - 1);

			var shift = Input.GetButton(Inputs.LeftShift) | Input.GetButton(Inputs.RightShift);
			var ctrl = Input.GetButton(Inputs.LeftControl) | Input.GetButton(Inputs.RightControl);
			var alt = Input.GetButton(Inputs.LeftAlt) | Input.GetButton(Inputs.RightAlt);

			if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.G))
				showGrid = !showGrid;
			if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.F))
				fog.visible = !fog.visible;
			else if (!shift && ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
				selectedTile = stage.tiles.Get(cursorTilePos);
			else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
				stage.tiles.Set(cursorTilePos, selectedTile);
			else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
				stage.tiles.Set(cursorTilePos, 0);
		}

		public override void Draw()
		{
			base.Draw();

			if (showGrid)
			{
				var lineColor = new Color(1, 0.025f);

				for (int x = 0; x < stage.tiles.width + 1; x++)
				{
					GFX.DrawBox(new Rect(x, 0, GFX.currentUnitsPerPixel, stage.tiles.height), lineColor);
				}

				for (int y = 0; y < stage.tiles.height + 1; y++)
				{
					GFX.DrawBox(new Rect(0, y, stage.tiles.width, GFX.currentUnitsPerPixel), lineColor);
				}

				GFX.DrawBox(new Rect((float)stage.tiles.width / 2, 0, GFX.currentUnitsPerPixel, stage.tiles.height), new Color(1, 0.1f));
				GFX.DrawBox(new Rect(0, (float)stage.tiles.height / 2, stage.tiles.width, GFX.currentUnitsPerPixel), new Color(1, 0.1f));

				GFX.DrawBox(new Rect(Input.cameraMousePosition.x, 0, GFX.currentUnitsPerPixel, Window.renderSize.y), new Color(1, 0.1f));
				GFX.DrawBox(new Rect(0, Input.cameraMousePosition.y, Window.renderSize.x, GFX.currentUnitsPerPixel), new Color(1, 0.1f));
			}
		}

		public override void DrawUI()
		{
			base.DrawUI();

			using (var layout = new MGE.UI.Layouts.StackLayout(new Vector2(16), 24, false))
			{
				Config.font.DrawText(stage.name, layout.newElement, Color.white);
				layout.AddElement();

				if (selectedTile - 1 < 0)
					layout.AddElement();
				else
					Config.font.DrawText("     " + Stage.tilesets[selectedTile - 1].Item2?.name, layout.newElement, Color.white);

				var tilesetText = $"{selectedTile.ToString("D2")} - {Stage.tilesets[selectedTile].Item2.name}";
				Config.font.DrawText(tilesetText, layout.newElement + 2, Stage.tilesets[selectedTile].Item2.color);
				Config.font.DrawText(tilesetText, layout.currentElement, Stage.tilesets[selectedTile].Item2.color.readableColor);

				if (selectedTile + 1 >= Stage.tilesets.Length)
					layout.AddElement();
				else
					Config.font.DrawText("     " + Stage.tilesets[selectedTile + 1].Item2.name, layout.newElement, Color.white);
			}
		}
	}
}