using MGE.ECS;
using MGE.FileIO;
using MGE.Graphics;
using MGE.InputSystem;
using MGE.StageSystem;
using MGE.StageSystem.Layers;

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

		public override void Init()
		{
			current = this;

			stage.layers.Add(new IntLayer(stage.size));
			stage.layers.Add(new EntityLayer());

			(layer as IntLayer).tiles[0, 0] = 1;
			(layer as IntLayer).tiles[1, 2] = 2;
			(layer as IntLayer).colors.Add(Color.red);
			(layer as IntLayer).colors.Add(Color.green);
		}

		public override void Update()
		{
			shift = Input.GetButton(Inputs.LeftShift);
			ctrl = Input.GetButton(Inputs.LeftControl);
			alt = Input.GetButton(Inputs.LeftAlt);

			if (!shift && ctrl && !alt && Input.GetButtonPress(Inputs.S))
				Save();
			else if (!shift && ctrl && !alt && Input.GetButtonPress(Inputs.L))
				Load();

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
		}

		public override void Draw()
		{
			using (new DrawBatch(transform: null))
			{
				GFX.DrawBox(new Rect(0, 0, Window.windowedSize.x, Window.windowedSize.y), Colors.black);
				GFX.DrawBox(Scale(new Rect(0, 0, stage.size.x, stage.size.y)), Colors.darkGray);

				Config.defualtFont.DrawText($"{stage.size} | {gridMousePos}", new Vector2(0, -(Config.defualtFont.charSize.y + 4) * zoom) + pan, Colors.gray, zoom);

				foreach (var layer in stage.layers)
				{
					layer.Draw(pan, zoom);
				}
			}
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