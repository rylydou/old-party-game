using MGE.ECS;
using MGE.InputSystem;
using MGE.UI;

namespace MGE.Debug.Menus
{
	public class DMenuHierarchy : DebugMenu
	{
		public override string name => "Hierarchy";

		const int itemSize = 24;
		const string disabledChar = "Ç";
		const string enabledChar = "â";
		const string invisChar = "é";
		const string visChar = "ü";

		readonly Vector2 offset = new Vector2(16, 16);
		readonly Color disabledColor = Colors.textDark;
		readonly Color enabledColor = Colors.text;

		int layers;
		int entities;
		int components;

		public override void Init()
		{
			title = "Hierarchy";
		}

		public override void UpdateBG()
		{
			base.UpdateBG();

			gui.Text($"L {layers}, E {entities}, C {components}" + (SceneManager.queuedScene == null ? "" : "| Scene Queued!"), offset, enabledColor);

			layers = 0;
			entities = 0;
			components = 0;

			var i = 1;

			var rect = new Rect();
			var clickRect = new Rect();

			foreach (var layer in SceneManager.activeScene.layers)
			{
				layers++;

				rect = new Rect(0, i * itemSize, size.x, itemSize);
				clickRect = new Rect(rect.position.x, rect.position.y + itemSize / 2, rect.size.x, rect.size.y);

				if (gui.MouseInside(clickRect))
				{
					gui.Image(clickRect, Colors.highlight);

					if (Input.GetButtonPress(Inputs.MouseLeft)) layer.enabled = !layer.enabled;
					else if (Input.GetButtonPress(Inputs.MouseRight)) layer.visible = !layer.visible;
					else if (Input.GetButtonPress(Inputs.MouseMiddle)) SceneManager.activeScene.RemoveLayer(layer);
				}

				gui.Text(GetStatus(layer.enabled, layer.visible) + (layer.isUI ? "UI " : "Layer ") + layer.name, rect.position + offset, layer.enabled ? enabledColor : disabledColor);

				i++;

				foreach (var entity in layer.entities)
				{
					entities++;

					rect = new Rect(0, i * itemSize, size.x, itemSize);
					clickRect = new Rect(rect.position.x, rect.position.y + itemSize / 2, rect.size.x, rect.size.y);

					if (gui.MouseInside(clickRect))
					{
						gui.Image(clickRect, Colors.highlight);

						if (Input.GetButtonPress(Inputs.MouseLeft)) Menuing.OpenMenu(new DMenuInspector(entity), Input.windowMousePosition);
						else if (Input.GetButtonPress(Inputs.MouseRight)) entity.visible = !entity.visible;
						else if (Input.GetButtonPress(Inputs.MouseMiddle)) entity.Destroy();
					}

					gui.Text("| " + (entity.destroyed ? "¡" : "") + GetStatus(entity.enabled, entity.visible) + entity.GetType().Name, rect.position + offset, layer.enabled && entity.enabled ? enabledColor : disabledColor);

					i++;

					foreach (var comp in entity.components)
					{
						components++;

						rect = new Rect(0, i * itemSize, size.x, itemSize);
						clickRect = new Rect(rect.position.x, rect.position.y + itemSize / 2, rect.size.x, rect.size.y);

						if (gui.MouseInside(clickRect))
						{
							gui.Image(clickRect, Colors.highlight);

							if (Input.GetButtonPress(Inputs.MouseLeft)) comp.Value.enabled = !comp.Value.enabled;
							else if (Input.GetButtonPress(Inputs.MouseRight)) comp.Value.visible = !comp.Value.visible;
							else if (Input.GetButtonPress(Inputs.MouseMiddle)) entity.RemoveComponent(comp.Key);
						}

						gui.Text("| | " + GetStatus(comp.Value.enabled, comp.Value.visible) + comp.Key.Name, rect.position + offset, layer.enabled && entity.enabled && comp.Value.enabled ? enabledColor : disabledColor);

						i++;
					}
				}
			}
		}

		static string GetStatus(bool enabled, bool visible) =>
			$"{(enabled ? enabledChar : disabledChar)} {(visible ? visChar : invisChar)} ";
	}
}