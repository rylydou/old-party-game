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

		public Stage stage = new Stage();

		public int layerIndex = 0;
		public StageLayer layer
		{
			get => stage.layers[layerIndex];
			set => stage.layers[layerIndex] = value;
		}

		Vector2 pan = Vector2.zero;
		float zoom = 1.0f;
		Vector2 targetPan = Vector2.zero;
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

		GUI inspectorGUI;
		GUI layersGUI;

		public override void Init()
		{
			current = this;
		}

		public override void Update()
		{
			layersGUI = new GUI(new Rect(0, 0, 64 * 4, Window.windowedSize.y), true);
			inspectorGUI = new GUI(new Rect(Window.windowedSize.x - 64 * 5, 0, 64 * 5, Window.windowedSize.y), true);

			layersGUI.Image(new Rect(Vector2.zero, layersGUI.rect.size), Colors.transBG);

			inspectorGUI.Image(new Rect(Vector2.zero, inspectorGUI.rect.size), Colors.transBG);

			shift = Input.GetButton(Inputs.LeftShift) || Input.GetButton(Inputs.RightShift);
			ctrl = Input.GetButton(Inputs.LeftControl) || Input.GetButton(Inputs.RightControl);
			alt = Input.GetButton(Inputs.LeftAlt) || Input.GetButton(Inputs.RightAlt);

			// > Copy Active Layer
			if (shift && !ctrl && alt && Input.GetButtonPress(Inputs.Up))
			{
				stage.layers.Insert(layerIndex, layer);
			}
			else if (shift && !ctrl && alt && Input.GetButtonPress(Inputs.Down))
			{
				stage.layers.Insert(layerIndex + 1, layer);
				layerIndex++;
			}
			// > Move Layer
			else if (!shift && !ctrl && alt && Input.GetButtonPress(Inputs.Up))
			{
				if (layerIndex > 0)
				{
					stage.layers.Move(layerIndex, layerIndex - 1);
					layerIndex--;
				}
			}
			else if (!shift && !ctrl && alt && Input.GetButtonPress(Inputs.Down))
			{
				if (layerIndex < stage.layers.Count - 1)
				{
					stage.layers.Move(layerIndex, layerIndex + 1);
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
				if (layerIndex < stage.layers.Count - 1)
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

				targetZoom = Math.Clamp(targetZoom - zoomDelta, 1.0f / stage.tileSize, stage.tileSize / 2);

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

			gridMousePos = localMousePos / (targetZoom * stage.tileSize);
			mouseInBounds =
				mousePos.x > layersGUI.rect.width && mousePos.x < Window.windowedSize.x - inspectorGUI.rect.width &&
				mousePos.y > 0 && mousePos.y < Window.windowedSize.y &&
				gridMousePos.x >= 0 && gridMousePos.x < stage.size.x &&
				gridMousePos.y >= 0 && gridMousePos.y < stage.size.y &&
				localMousePos.x > 0 &&
				localMousePos.y > 0;

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

				foreach (var layer in stage.layers)
				{
					var rect = new Rect(layout.newElement, new Vector2(layersGUI.rect.width, layout.currentSize));

					if (index == layerIndex)
						layersGUI.Image(rect, Colors.highlight);

					switch (layersGUI.Button(layer.name, rect, layer.isVisible ? Colors.text : Colors.textDark))
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
					stage.layers.RemoveAt(layerToRemove);
					if (layerIndex >= layerToRemove)
						layerIndex--;
				}

				if (layersGUI.ButtonClicked("Add New Layer...", new Rect(layout.newElement, new Vector2(layersGUI.rect.width, layout.currentSize))))
				{
					Menuing.OpenMenu(new DMenuNewLayer(), Input.windowMousePosition);
				}
			}

			layer.Editor_Update(ref inspectorGUI);

			oldMousePos = gridMousePos;
		}

		public override void Draw()
		{
			using (new DrawBatch(transform: null))
			{
				GFX.DrawBox(new Rect(0, 0, Window.windowedSize.x, Window.windowedSize.y), Colors.black);
				GFX.DrawBox(Scale(new Rect(0, 0, stage.size.x, stage.size.y)), Colors.darkGray);
				GFX.DrawRect(Scale(new Rect(0, 0, stage.size.x, stage.size.y)), mouseInBounds ? Colors.gray : Colors.lightGray, Math.Clamp(stage.tileSize / 2 * zoom, 1, float.PositiveInfinity));

				Config.font.DrawText(stage.size.ToString(), new Vector2(0, -(Config.font.charSize.y + stage.tileSize) * zoom) + pan, Colors.gray, zoom);

				if (isolateActiveLayer)
				{
					layer.Editor_Draw(pan, zoom);
				}
				else
				{
					foreach (var layer in stage.layers)
					{
						if (layer.isVisible)
							layer.Editor_Draw(pan, zoom);
					}
				}

				if (enableGrid)
				{
					var color = Colors.accent.ChangeAlpha(0.15f);

					for (int y = 1; y < stage.size.y; y++)
						GFX.DrawLine(Scale(new Vector2(0, y)), Scale(new Vector2(stage.size.x, y)), color, 1f);

					for (int x = 1; x < stage.size.x; x++)
						GFX.DrawLine(Scale(new Vector2(x, 0)), Scale(new Vector2(x, stage.size.y)), color, 1f);

					if (mouseInBounds)
					{
						color = Colors.accent.ChangeAlpha(1f / 3f);

						var size = 2f;

						GFX.DrawLine(Scale(new Vector2(gridMousePos.x + 0.5f, 0)) + size / 2, Scale(new Vector2(gridMousePos.x + 0.5f, stage.size.y)) + size / 2, color, size);
						GFX.DrawLine(Scale(new Vector2(0, gridMousePos.y + 0.5f)) - size / 2, Scale(new Vector2(stage.size.x, gridMousePos.y + 0.5f)) - size / 2, color, size);
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

			layersGUI.Draw();

			if (inspectorGUI.elements.Count < 2)
			{
				inspectorGUI.Text("(No properties to edit)", new Rect(0, 0, inspectorGUI.rect.width, inspectorGUI.rect.height), Colors.textDark, 1, TextAlignment.Center);
				inspectorGUI.Text("Today is a sad day indeed :(", new Rect(0, 24, inspectorGUI.rect.width, inspectorGUI.rect.height), Colors.text.ChangeAlpha(0.15f), 0.75f, TextAlignment.Center);
			}

			inspectorGUI.Draw();
		}

		public static Vector2 Scale(Vector2 vector) => current.pan + vector * current.zoom * current.stage.tileSize;
		public static Rect Scale(Rect rect) => new Rect(Scale(rect.position), rect.size * current.zoom * current.stage.tileSize);

		public void Save()
		{
			Logger.Log("Saving...");
			IO.Save(App.exePath + "/Assets/Stages/test.stage", stage);
			Logger.Log("Saved!");
		}

		public void Load()
		{
			Logger.Log("Loading...");
			// Logger.Log(IO.Load<string>(App.exePath + "/Assets/Stages/test.stage"));
			stage = IO.Load<Stage>(App.exePath + "/Assets/Stages/test.stage");
			// IO.SaveJson(App.exePath + "/Assets/Stages/test.stage", IO.Load<TestStruct>(App.exePath + "/Assets/Stages/test.stage"));
			Logger.Log("Loaded!");
		}
	}
}