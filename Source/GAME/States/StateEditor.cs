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
			PlayerSpawns,
			CrateSpawns,
			Params
		}

		const int tileSize = 16;

		public static Mode mode = Mode.Ground;

		public static byte selectedTile = 1;
		public static byte selectedCrateIndex = 1;

		public static (byte, byte) selectedBgTile = (1, 2);
		public static bool showBgTilePicker;
		public static Vector2 bgTilePickerPosition;
		public static Vector2 bgTilePickerDragStartPosition;
		public static Vector2 bgTilePickerMouseDragStartPosition;
		public static bool closingOutOfBgTilePicker;

		static bool showGrid = false;
		static bool showPlayerSpawnPoints = true;
		static bool showCrateSpawnPoints = true;

		Vector2 cursorPos;
		Vector2Int cursorTilePos;

		Texture bgTileset;

		CFog fog;
		Stage stage;

		public override void Init()
		{
			base.Init();

			stage = GameSettings.stage;

			SceneManager.QueueScene(
				new Scene(
					new Layer(
						"Background",
						new Entity(new CBackground())
					),
					new Layer(
						"Stage Background",
						new Entity(new CStageBackground())
					),
					new Layer(
						"Gameplay",
						new Entity(new CStage())
					),
					new Layer(
						"Foreground",
						new Entity(new CFog())
					)
				)
			);

			fog = SceneManager.activeScene.GetLayer("Foreground").GetEntityWithComponent<CFog>().GetComponent<CFog>();

			bgTileset = Assets.GetAsset<Texture>("Tilesets/_Background");
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
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.C))
				showCrateSpawnPoints = !showCrateSpawnPoints;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D1))
				mode = Mode.Ground;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D2))
				mode = Mode.Background;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D3))
				mode = Mode.Foreground;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D4))
				mode = Mode.PlayerSpawns;
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.D5))
				mode = Mode.CrateSpawns;
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
				case Mode.PlayerSpawns:

					if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
					{
						if (!stage.playerSpawnPoints.Contains(cursorTilePos))
							stage.playerSpawnPoints.Add(cursorTilePos);
					}
					else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
						stage.playerSpawnPoints.Remove(cursorTilePos);

					break;
				case Mode.CrateSpawns:

					if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
					{
						if (!stage.crateSpawnsPoints.Contains(cursorTilePos))
							stage.crateSpawnsPoints.Add(cursorTilePos);
					}
					else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
						stage.crateSpawnsPoints.Remove(cursorTilePos);

					break;
				case Mode.Background:

					if (closingOutOfBgTilePicker && !Input.GetButton(Inputs.MouseLeft))
						closingOutOfBgTilePicker = false;

					if (!shift && !ctrl && !alt && showBgTilePicker && Input.GetButtonPress(Inputs.MouseRight))
					{
						bgTilePickerDragStartPosition = bgTilePickerPosition;
						bgTilePickerMouseDragStartPosition = cursorPos;
					}
					else if (!shift && !ctrl && !alt && showBgTilePicker && Input.GetButton(Inputs.MouseRight))
						bgTilePickerPosition = bgTilePickerDragStartPosition + cursorPos - bgTilePickerMouseDragStartPosition;
					else if (!shift && !ctrl && !alt && showBgTilePicker && Input.GetButtonPress(Inputs.MouseLeft))
					{
						var pos = ((byte)(cursorPos.x - bgTilePickerPosition.x), (byte)(cursorPos.y - bgTilePickerPosition.y));
						if (pos.Item1 < 0 || pos.Item2 < 0 || pos.Item1 >= byte.MaxValue || pos.Item2 >= byte.MaxValue) return;
						selectedBgTile = pos;
						showBgTilePicker = false;
						closingOutOfBgTilePicker = true;
					}
					else if (!shift && ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
						selectedBgTile = stage.tilesBackground.Get(cursorTilePos);
					else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
					{
						if (!closingOutOfBgTilePicker)
							stage.tilesBackground.Set(cursorTilePos, selectedBgTile);
					}
					else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
						stage.tilesBackground.Set(cursorTilePos, (0, 0));
					else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Space))
						showBgTilePicker = !showBgTilePicker;

					break;
			}
		}

		public override void Draw()
		{
			base.Draw();

			if (showCrateSpawnPoints)
			{
				foreach (var crateSpawnPoint in stage.crateSpawnsPoints)
				{
					GFX.DrawBox(new Rect((Vector2)crateSpawnPoint + GFX.currentUnitsPerPixel, 1.0f - GFX.currentUnitsPerPixel * 2), new Color("#3BF6"));
				}
			}

			if (showPlayerSpawnPoints)
			{
				foreach (var playerSpawnPoint in stage.playerSpawnPoints)
				{
					GFX.DrawBox(new Rect((Vector2)playerSpawnPoint + GFX.currentUnitsPerPixel * 6, 1.0f - GFX.currentUnitsPerPixel * 12), new Color("#FB3B"));
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

			if (showBgTilePicker)
			{
				if (mode != Mode.Background)
					showBgTilePicker = false;
				else
				{
					GFX.DrawBox(new Rect(0, 0, Window.sceneSize), new Color(0, 0.1f));
					GFX.DrawBox(new Rect(bgTilePickerPosition, byte.MaxValue), new Color(0, 0.5f));
					GFX.Draw(bgTileset, bgTilePickerPosition, Color.white);
					GFX.DrawBox(new Rect((Vector2)bgTilePickerPosition + new Vector2(selectedBgTile.Item1, selectedBgTile.Item2), 1), new Color("#FB32"));
				}
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
					case Mode.Background:
						if (!showBgTilePicker)
						{
							GFX.Draw(bgTileset, new RectInt(new Vector2Int(selectedBgTile.Item1, selectedBgTile.Item2) * tileSize, tileSize), new Rect(layout.newElement + 4, 42), new Color(0, 0.25f));
							GFX.Draw(bgTileset, new RectInt(new Vector2Int(selectedBgTile.Item1, selectedBgTile.Item2) * tileSize, tileSize), new Rect(layout.currentElement, 42));
						}
						break;
				}
			}
		}
	}
}