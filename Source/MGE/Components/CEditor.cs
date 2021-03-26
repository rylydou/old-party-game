using MGE.Debug;
using MGE.Debug.Menus;
using MGE.ECS;
using MGE.FileIO;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.StageSystem;
using MGE.StageSystem.Layers;
using MGE.UI;
using MGE.UI.Layouts;

namespace MGE.Components
{
	public class CEditor : Component
	{
		public static CEditor current { get; private set; }

		public EditorState state = EditorState.World;

		public World world = new World();
		public Level level = null;

		public int layerIndex = 0;
		public LevelLayer layer
		{
			get => level.layers[layerIndex];
			set => level.layers[layerIndex] = value;
		}

		Vector2 pan = Vector2.zero;
		Vector2 targetPan = Vector2.zero;
		float zoom = 1.0f;
		float targetZoom = 1.0f;

		bool mouseInBounds = false;
		Vector2Int oldMousePos = Vector2Int.zero;
		Vector2Int gridMousePos = Vector2Int.zero;

		bool shift = false;
		bool ctrl = false;
		bool alt = false;
		Vector2 panMouseStartPos = Vector2.zero;
		Vector2 panStartPos = Vector2.zero;

		bool isolateActiveLayer = false;
		bool enableGrid = false;

		string path = "/Assets/Stages/world.world";

		GUI inspectorGUI;
		GUI hierarchyGUI;
		GUI mainGUI;

		public override void Init()
		{
			current = this;

			world = new World(new Vector2Int(16), new Vector2Int(16), 16);
			world.path = path;
			world.Reload();

			level = world.LevelCreateNew(new Vector2Int(0, 0));
			// level = world.LevelCreateNew(new Vector2Int(1, 1));
			// level = world.LevelCreateNew(new Vector2Int(5, 3));
		}

		public override void Update()
		{
			hierarchyGUI = new GUI(new Rect(0, 0, 64 * 4, Window.windowedSize.y), true);
			inspectorGUI = new GUI(new Rect(Window.windowedSize.x - 64 * 5, 0, 64 * 5, Window.windowedSize.y), true);
			mainGUI = new GUI(new Rect(0, 0, Window.windowedSize), true);

			hierarchyGUI.Image(new Rect(Vector2.zero, hierarchyGUI.rect.size), Colors.transBG);

			inspectorGUI.Image(new Rect(Vector2.zero, inspectorGUI.rect.size), Colors.transBG);

			shift = Input.GetButton(Inputs.LeftShift) || Input.GetButton(Inputs.RightShift);
			ctrl = Input.GetButton(Inputs.LeftControl) || Input.GetButton(Inputs.RightControl);
			alt = Input.GetButton(Inputs.LeftAlt) || Input.GetButton(Inputs.RightAlt);

			// > Copy Active Layer
			if (shift && !ctrl && alt && Input.GetButtonPress(Inputs.Up))
			{
				level.layers.Insert(layerIndex, layer);
			}
			else if (shift && !ctrl && alt && Input.GetButtonPress(Inputs.Down))
			{
				level.layers.Insert(layerIndex + 1, layer);
				layerIndex++;
			}
			// > Move Layer
			else if (!shift && !ctrl && alt && Input.GetButtonPress(Inputs.Up))
			{
				if (layerIndex > 0)
				{
					level.layers.Move(layerIndex, layerIndex - 1);
					layerIndex--;
				}
			}
			else if (!shift && !ctrl && alt && Input.GetButtonPress(Inputs.Down))
			{
				if (layerIndex < level.layers.Count - 1)
				{
					level.layers.Move(layerIndex, layerIndex + 1);
					layerIndex++;
				}
			}
			// > Saving & Loading
			if (!shift && ctrl && !alt && Input.GetButtonPress(Inputs.S))
				Save();
			else if (!shift && ctrl && !alt && Input.GetButtonPress(Inputs.L))
				Load();
			// > Change Active Layer
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Up))
			{
				if (layerIndex > 0)
					layerIndex--;
			}
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Down))
			{
				if (layerIndex < level.layers.Count - 1)
					layerIndex++;
			}
			// > Toggle isolateActiveLayer
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.Tab))
			{
				isolateActiveLayer = !isolateActiveLayer;
			}
			// > Toggle enableGrid
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.G))
			{
				enableGrid = !enableGrid;
			}
			// > Help
			else if (!shift && !ctrl && !alt && Input.GetButtonPress(Inputs.F1))
			{
				Menuing.OpenMenu(new DMenuEditorCheatsheet());
			}

			if (Input.GetButtonPress(Inputs.MouseMiddle))
			{
				panStartPos = targetPan;
				panMouseStartPos = Input.windowMousePosition;
			}
			else if (Input.GetButton(Inputs.MouseMiddle))
			{
				targetPan = panStartPos + Input.windowMousePosition - panMouseStartPos;
			}
			else
			{
				var zoomDelta = 0.0f;

				if (Input.scroll > 0)
					zoomDelta = targetZoom / 2 * (int)Input.scroll;
				else
					zoomDelta = targetZoom * (int)Input.scroll;

				var oldZoom = targetZoom;

				targetZoom = Math.Clamp(targetZoom - zoomDelta, 1.0f / world.tileSize, world.tileSize / 2);

				var zoomChange = oldZoom - targetZoom;

				targetPan = targetPan + (Input.windowMousePosition - targetPan) * (1.0f / oldZoom * zoomChange);
			}

			pan = Vector2.Lerp(pan, targetPan, 25 * Time.unscaledDeltaTime);
			zoom = Math.Lerp(zoom, targetZoom, 25 * Time.unscaledDeltaTime);

			if ((pan - targetPan).sqrMagnitude < 0.0125f * 0.0125f)
				pan = targetPan;

			if (Math.Abs(zoom - targetZoom) < 0.0125f * 0.0125f)
				zoom = targetZoom;

			var mousePos = Input.windowMousePosition;

			var localMousePos = (Input.windowMousePosition - targetPan);

			gridMousePos = localMousePos / (targetZoom * world.tileSize);
			mouseInBounds =
				mousePos.x > hierarchyGUI.rect.width && mousePos.x < Window.windowedSize.x - inspectorGUI.rect.width &&
				mousePos.y > 0 && mousePos.y < Window.windowedSize.y &&
				gridMousePos.x >= 0 && gridMousePos.x < world.levelSize.x &&
				gridMousePos.y >= 0 && gridMousePos.y < world.levelSize.y &&
				localMousePos.x > 0 &&
				localMousePos.y > 0;

			switch (state)
			{
				case EditorState.World:
					World_Update();
					break;
				case EditorState.Level:
					Level_Update();
					break;
			}

			oldMousePos = gridMousePos;
		}

		public override void Draw()
		{
			switch (state)
			{
				case EditorState.World:
					World_Draw();
					break;
				case EditorState.Level:
					Level_Draw();
					break;
			}

			hierarchyGUI.Draw();

			if (inspectorGUI.elements.Count < 2)
			{
				inspectorGUI.Text("(No properties to edit)", new Rect(0, 0, inspectorGUI.rect.width, inspectorGUI.rect.height), Colors.textDark, 1, TextAlignment.Center);
				inspectorGUI.Text("Today is a sad day indeed :(", new Rect(0, 24, inspectorGUI.rect.width, inspectorGUI.rect.height), Colors.text.ChangeAlpha(0.15f), 0.75f, TextAlignment.Center);
			}

			inspectorGUI.Draw();

			mainGUI.Draw();
		}

		public void World_Update()
		{

		}

		public void World_Draw()
		{
			using (new DrawBatch(transform: null))
			{
				var levelSize = (Vector2)world.levelSize * world.tileSize * zoom;

				world.availableLevels.For((x, y, state) =>
				{
					GFX.DrawBox(new Rect(pan + levelSize * new Vector2(x, y), levelSize), state ? Colors.lightGray : Colors.gray);
				});

				GFX.DrawRect(new Rect(pan, levelSize * (Vector2)world.size), Colors.accent, 8);

				var color = Colors.accent.ChangeAlpha(0.15f);

				float lineSize = Math.Clamp(8 * zoom, 1, 8);

				for (int y = 1; y < world.size.y; y++)
					GFX.DrawBox(new Rect(pan.x, pan.y + y * world.levelSize.y * world.tileSize * zoom - lineSize / 2, world.size.x * world.levelSize.x * world.tileSize * zoom, lineSize), color);

				for (int x = 1; x < world.size.x; x++)
					GFX.DrawBox(new Rect(pan.x + x * world.levelSize.x * world.tileSize * zoom - lineSize / 2, pan.y, lineSize, world.size.y * world.levelSize.y * world.tileSize * zoom), color);
			}
		}

		public void Level_Update()
		{
			if (mouseInBounds)
			{
				if (layer is IntLayer intLayer)
				{
					if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
					{
						foreach (var point in Util.LineToPointsInGrid(oldMousePos, gridMousePos))
							intLayer.tiles.Set(point, intLayer.selectedColor);
						intLayer.lastChanged = Time.unscaledTime;
					}
					else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
					{
						foreach (var point in Util.LineToPointsInGrid(oldMousePos, gridMousePos))
							intLayer.tiles.Set(point, 0);
						intLayer.lastChanged = Time.unscaledTime;
					}
				}
			}

			using (var layout = new StackLayout(new Vector2Int(0, 8), 32, false))
			{
				var index = 0;
				var layerToRemove = -1;

				foreach (var layer in level.layers)
				{
					var rect = new Rect(layout.newElement, new Vector2(hierarchyGUI.rect.width, layout.currentSize));

					if (index == layerIndex)
						hierarchyGUI.Image(rect, Colors.highlight);

					switch (hierarchyGUI.Button(layer.name, rect, layer.isVisible ? Colors.text : Colors.textDark))
					{
						case PointerInteraction.LClick:
							if (index == layerIndex)
								Menuing.OpenMenu(new DMenuTextInput("Enter Layer Name...", layer.name, (x) => layer.name = x));
							else
								layerIndex = index;
							break;
						case PointerInteraction.RClick:
							layer.isVisible = !layer.isVisible;
							break;
						case PointerInteraction.MClick:
							layerToRemove = index;
							break;
					}

					index++;
				}

				if (layerToRemove > 0)
				{
					level.layers.RemoveAt(layerToRemove);
					if (layerIndex >= layerToRemove)
						layerIndex--;
				}

				if (hierarchyGUI.ButtonClicked("Add New Layer...", new Rect(layout.newElement, new Vector2(hierarchyGUI.rect.width, layout.currentSize))))
				{
					Menuing.OpenMenu(new DMenuNewLayer(), Input.windowMousePosition);
				}
			}

			layer.Editor_Update(ref inspectorGUI);
		}

		public void Level_Draw()
		{
			using (new DrawBatch(transform: null))
			{
				GFX.DrawBox(new Rect(0, 0, Window.windowedSize.x, Window.windowedSize.y), Colors.black);
				GFX.DrawBox(Scale(new Rect(0, 0, world.levelSize.x, world.levelSize.y)), Colors.darkGray);
				GFX.DrawRect(Scale(new Rect(0, 0, world.levelSize.x, world.levelSize.y)), mouseInBounds ? Colors.gray : Colors.lightGray, Math.Clamp(world.tileSize / 2 * zoom, 1, float.PositiveInfinity));

				Config.font.DrawText(world.levelSize.ToString(), new Vector2(0, -(Config.font.charSize.y + world.tileSize) * zoom) + pan, Colors.gray, zoom);

				if (isolateActiveLayer)
				{
					layer.Editor_Draw(pan, zoom);
				}
				else
				{
					foreach (var layer in level.layers)
					{
						if (layer.isVisible)
							layer.Editor_Draw(pan, zoom);
					}
				}

				if (enableGrid)
				{
					var color = Colors.accent.ChangeAlpha(0.15f);

					const float lineSize = 1;

					for (int y = 1; y < world.levelSize.y; y++)
						GFX.DrawBox(new Rect(pan.x, pan.y + y * world.tileSize * zoom - lineSize / 2, world.levelSize.x * world.tileSize * zoom, lineSize), color);

					for (int x = 1; x < world.levelSize.x; x++)
						GFX.DrawBox(new Rect(pan.x + x * world.tileSize * zoom - lineSize / 2, pan.y, lineSize, world.levelSize.y * world.tileSize * zoom), color);

					if (mouseInBounds)
					{
						color = Colors.accent.ChangeAlpha(1f / 3f);

						var size = 2f;

						GFX.DrawLine(Scale(new Vector2(gridMousePos.x + 0.5f, 0)) + size / 2, Scale(new Vector2(gridMousePos.x + 0.5f, world.levelSize.y)) + size / 2, color, size);
						GFX.DrawLine(Scale(new Vector2(0, gridMousePos.y + 0.5f)) - size / 2, Scale(new Vector2(world.levelSize.x, gridMousePos.y + 0.5f)) - size / 2, color, size);
					}
				}

				if (mouseInBounds)
				{
					var rect = Scale(new Rect(gridMousePos, Vector2Int.one));
					GFX.DrawBox(rect, Colors.accent.ChangeAlpha(Math.Abs(Math.Sin(Time.time * 1.5f)) * 0.15f));
					GFX.DrawRect(rect, Colors.accent.ChangeAlpha(0.5f), Math.Clamp(zoom * 0.5f, 1, float.PositiveInfinity));

					if (enableGrid)
						Config.font.DrawText(gridMousePos.ToString(), Input.windowMousePosition + new Vector2(26, -32), Colors.accent);
				}
			}
		}

		public static Vector2 Scale(Vector2 vector) => current.pan + vector * current.zoom * current.world.tileSize;
		public static Rect Scale(Rect rect) => new Rect(Scale(rect.position), rect.size * current.zoom * current.world.tileSize);

		public void Save()
		{
			try
			{
				using (Timmer.Start($"Saving World to {path}"))
				{
					world.SaveAllLevels();
					IO.Save(path, world);
				}
			}
			catch (System.Exception e)
			{
				Logger.MSGBox("Error When Saving!", e.Message, System.Windows.Forms.MessageBoxIcon.Error);
			}
		}

		public void Load()
		{
			try
			{
				using (Timmer.Start($"Loading World from {path}"))
				{
					world = IO.Load<World>(path);
				}
			}
			catch (System.Exception e)
			{
				Logger.MSGBox("Error When Loading!", e.Message, System.Windows.Forms.MessageBoxIcon.Error);
			}
		}

		public void Log(string message)
		{
			Logger.Log(message);
		}

		public void LogWarning(string message)
		{
			Logger.LogWarning(message);
		}

		public void LogError(string message)
		{
			Logger.Log(message);
		}
	}
}