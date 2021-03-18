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
		public const float tileSize = 16;

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

		bool mouseInBounds = false;
		Vector2Int gridMousePos = Vector2Int.zero;

		bool shift = false;
		bool ctrl = false;
		bool alt = false;
		Vector2 panMouseStartPos = Vector2.zero;
		Vector2 panStartPos = Vector2.zero;

		bool isolateActiveLayer = false;

		GUI inspectorGUI;
		GUI mainGUI;

		public override void Init()
		{
			current = this;
		}

		public override void Update()
		{
			mainGUI = new GUI(new Rect(0, 0, 64 * 4, Window.windowedSize.y), true);
			inspectorGUI = new GUI(new Rect(Window.windowedSize.x - 64 * 5, 0, 64 * 5, Window.windowedSize.y), true);

			mainGUI.Image(new Rect(Vector2.zero, mainGUI.rect.size), Colors.transBG);

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

			if (Input.GetButtonPress(Inputs.MouseMiddle))
			{
				panStartPos = pan;
				panMouseStartPos = Input.windowMousePosition;
			}
			else if (Input.GetButton(Inputs.MouseMiddle))
			{
				pan = panStartPos + Input.windowMousePosition - panMouseStartPos;
			}
			else
			{
				var zoomDelta = 0.0f;

				if (Input.scroll > 0)
					zoomDelta = zoom / 2 * (int)Input.scroll;
				else
					zoomDelta = zoom * (int)Input.scroll;

				var oldZoom = zoom;

				zoom = Math.Clamp(zoom - zoomDelta, 1.0f / tileSize, tileSize * 2);

				var zoomChange = oldZoom - zoom;

				pan = pan + (Input.windowMousePosition - pan) * (1.0f / oldZoom * zoomChange);
			}

			var mousePos = Input.windowMousePosition;

			gridMousePos = (Input.windowMousePosition - pan) / (zoom * tileSize);
			mouseInBounds =
				mousePos.x > mainGUI.rect.width && mousePos.x < Window.windowedSize.x - inspectorGUI.rect.width &&
				mousePos.y > 0 && mousePos.y < Window.windowedSize.y &&
				gridMousePos.x >= 0 && gridMousePos.x < stage.size.x &&
				gridMousePos.y >= 0 && gridMousePos.y < stage.size.y;

			if (mouseInBounds)
			{

				if (layer is IntLayer intLayer)
				{
					if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
					{
						intLayer.tiles[gridMousePos] = intLayer.colorIndex;
					}
					else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
					{
						intLayer.tiles[gridMousePos] = 0;
					}
				}
			}

			using (var layout = new StackLayout(new Vector2Int(0, 16), 32, false))
			{
				var index = 0;
				var layerToRemove = -1;

				foreach (var layer in stage.layers)
				{
					var rect = new Rect(layout.newElement, new Vector2(mainGUI.rect.width, layout.currentSize));

					if (index == layerIndex)
						mainGUI.Image(rect, Colors.highlight);

					switch (mainGUI.Button(layer.name, rect, TextAlignment.Center, layer.isVisible ? Colors.text : Colors.textDark))
					{
						case PointerInteraction.LClick:
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
					stage.layers.RemoveAt(layerToRemove);

				if (mainGUI.ButtonClicked("Add Lew Layer...", new Rect(layout.newElement, new Vector2(mainGUI.rect.width, layout.currentSize))))
				{
					Menuing.OpenMenu(new DMenuNewLayer());
				}
			}

			layer.Update(ref inspectorGUI);
		}

		public override void Draw()
		{
			using (new DrawBatch(transform: null))
			{
				GFX.DrawBox(new Rect(0, 0, Window.windowedSize.x, Window.windowedSize.y), Colors.black);
				GFX.DrawBox(Scale(new Rect(0, 0, stage.size.x, stage.size.y)), Colors.darkGray);

				Config.defualtFont.DrawText($"{stage.size} | {gridMousePos}", new Vector2(0, -(Config.defualtFont.charSize.y + 4) * zoom) + pan, Colors.gray, zoom);

				if (isolateActiveLayer)
				{
					layer.Draw(pan, zoom);
				}
				else
				{
					foreach (var layer in stage.layers)
					{
						if (layer.isVisible)
							layer.Draw(pan, zoom);
					}
				}
			}

			mainGUI.Draw();
			if (inspectorGUI.elements.Count < 2)
				inspectorGUI.Text("(No properties to edit)", new Rect(0, 8, inspectorGUI.rect.width, inspectorGUI.rect.height), Colors.textDark, 1, TextAlignment.Center);
			inspectorGUI.Draw();
		}

		public static Vector2 Scale(Vector2 vector) => current.pan + vector * current.zoom * tileSize;
		public static Rect Scale(Rect rect) => new Rect(Scale(rect.position), rect.size * current.zoom * tileSize);

		public void Save()
		{
			IO.Save(App.exePath + "/Assets/Stages/test.stage", stage);
			Logger.Log("Saved!");
		}

		public void Load()
		{
			stage = IO.Load<Stage>(App.exePath + "/Assets/Stages/test.stage");
			Logger.Log("Loaded!");
		}
	}
}