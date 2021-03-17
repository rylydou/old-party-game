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

		bool hideOthers = false;

		GUI inspectorGUI;
		GUI mainGUI;

		public override void Init()
		{
			current = this;
		}

		public override void Update()
		{
			mainGUI = new GUI(new Rect(0, 0, 64 * 6, Window.windowedSize.y), true);
			inspectorGUI = new GUI(new Rect(Window.windowedSize.x - 64 * 6, 0, 64 * 6, Window.windowedSize.y), true);

			mainGUI.Image(mainGUI.rect, Colors.transBG);

			inspectorGUI.Image(inspectorGUI.rect, Colors.transBG);

			shift = Input.GetButton(Inputs.LeftShift) || Input.GetButton(Inputs.RightShift);
			ctrl = Input.GetButton(Inputs.LeftControl) || Input.GetButton(Inputs.RightControl);
			alt = Input.GetButton(Inputs.LeftAlt) || Input.GetButton(Inputs.RightAlt);

			if (!shift && ctrl && !alt && Input.GetButtonPress(Inputs.S))
				Save();
			else if (!shift && ctrl && !alt && Input.GetButtonPress(Inputs.L))
				Load();
			else if (!shift && ctrl && !alt && Input.GetButtonPress(Inputs.I))
			{
				hideOthers = !hideOthers;
				Logger.Log($"Hide Others: {hideOthers}");
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

			gridMousePos = (Input.windowMousePosition - pan) / (zoom * tileSize);
			mouseInBounds = gridMousePos.x >= 0 && gridMousePos.x < stage.size.x && gridMousePos.y >= 0 && gridMousePos.y < stage.size.y;

			if (mouseInBounds)
			{
				if (layer is IntLayer intLayer)
				{
					if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
					{
						intLayer.tiles[gridMousePos] = 1;
					}
					else if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseRight))
					{
						intLayer.tiles[gridMousePos] = 0;
					}
				}
			}

			using (var layout = new StackLayout(new Vector2Int(0, 16), 32, false))
			{
				int index = 0;
				foreach (var layer in stage.layers)
				{
					var rect = new Rect(layout.newElement, new Vector2(mainGUI.rect.width, layout.currentSize));

					switch (mainGUI.Button(layer.name, rect))
					{
						case PointerInteraction.LClick:
							layerIndex = index;
							break;
					}

					if (index == layerIndex)
						mainGUI.Image(rect, Colors.highlight);

					index++;
				}

				if (mainGUI.ButtonClicked("Add Lew Layer...", new Rect(layout.newElement, new Vector2(mainGUI.rect.width, layout.currentSize))))
				{
					stage.layers.Add(new IntLayer(stage.size));
				}
			}
		}

		public override void Draw()
		{
			using (new DrawBatch(transform: null))
			{
				GFX.DrawBox(new Rect(0, 0, Window.windowedSize.x, Window.windowedSize.y), Colors.black);
				GFX.DrawBox(Scale(new Rect(0, 0, stage.size.x, stage.size.y)), Colors.darkGray);

				Config.defualtFont.DrawText($"{stage.size} | {gridMousePos}", new Vector2(0, -(Config.defualtFont.charSize.y + 4) * zoom) + pan, Colors.gray, zoom);

				if (hideOthers)
				{
					layer.Draw(pan, zoom);
				}
				else
				{
					foreach (var layer in stage.layers)
					{
						layer.Draw(pan, zoom);
					}
				}
			}

			mainGUI.Draw();
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