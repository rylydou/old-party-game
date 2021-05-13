using System.Text.RegularExpressions;
using GAME.Components;
using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.InputSystem;

namespace GAME.States
{
	public class StateEditor : GameState
	{
		public enum Mode
		{
			Ground,
			Background,
			Foreground,
			PlayerSpawn,
			Params
		}

		public Mode mode = Mode.Ground;

		public byte selectedTile = 1;

		bool showGrid = false;
		bool showPlayerSpawnPoints = true;

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

			var shift = Input.GetButton(Inputs.LeftShift) | Input.GetButton(Inputs.RightShift);
			var ctrl = Input.GetButton(Inputs.LeftControl) | Input.GetButton(Inputs.RightControl);
			var alt = Input.GetButton(Inputs.LeftAlt) | Input.GetButton(Inputs.RightAlt);

			cursorPos = Input.cameraMousePosition;
			cursorTilePos = cursorPos;

			if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.G))
				showGrid = !showGrid;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.F))
				fog.visible = !fog.visible;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.P))
				showPlayerSpawnPoints = !showPlayerSpawnPoints;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D1))
				mode = Mode.Ground;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D2))
				mode = Mode.Background;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D3))
				mode = Mode.Foreground;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D4))
				mode = Mode.PlayerSpawn;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D0))
				mode = Mode.Params;

			switch (mode)
			{
				case Mode.Ground:

					if (!shift && ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
					{
						var tile = stage.tiles.Get(cursorTilePos);
						if (tile != 0)
							selectedTile = tile;
					}
					else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
						stage.tiles.Set(cursorTilePos, selectedTile);
					else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
						stage.tiles.Set(cursorTilePos, 0);
					else if (!shift && !ctrl && !alt)
						selectedTile = (byte)Math.Clamp(selectedTile + Input.scroll, 1, Stage.tilesets.Length - 1);

					break;
				case Mode.PlayerSpawn:

					if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
					{
						if (!stage.playerSpawnPoints.Contains(cursorTilePos))
							stage.playerSpawnPoints.Add(cursorTilePos);
					}
					else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
						stage.playerSpawnPoints.Remove(cursorTilePos);

					break;
			}
		}

		public override void Draw()
		{
			base.Draw();

			if (showPlayerSpawnPoints)
			{
				foreach (var playerSpawnPoint in stage.playerSpawnPoints)
				{
					if (playerSpawnPoint.y == Stage.size.y - 1)
						GFX.DrawBox(new Rect((Vector2)playerSpawnPoint + GFX.currentUnitsPerPixel, 1.0f - GFX.currentUnitsPerPixel * 2, 0.5f - GFX.currentUnitsPerPixel), new Color("#FB38"));
					else
						GFX.DrawBox(new Rect((Vector2)playerSpawnPoint + GFX.currentUnitsPerPixel, 1.0f - GFX.currentUnitsPerPixel * 2), new Color("#FB38"));
				}
			}

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

				var modeName = Regex.Replace(mode.ToString(), @"\p{Lu}", c => " " + c.Value.ToUpperInvariant()).Remove(0, 1);

				Config.font.DrawText($"Mode - {modeName}", layout.newElement, Color.white);

				layout.AddElement();

				switch (mode)
				{
					case Mode.Ground:

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

						break;
				}
			}
		}
	}
}