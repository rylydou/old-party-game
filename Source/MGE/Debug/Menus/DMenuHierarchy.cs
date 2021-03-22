using MGE.ECS;
using MGE.InputSystem;
using MGE.UI;

namespace MGE.Debug.Menus
{
	public class DMenuHierarchy : DebugMenu
	{
		public override string name => "Hierarchy";

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

			foreach (var layer in SceneManager.activeScene.layers)
			{
				layers++;

				rect = new Rect(0, i * allSize, size.x, allSize);

				switch (gui.Button(GetStatus(layer.enabled, layer.visible) + (layer.isUI ? "UI " : "Layer ") + layer.name, rect))
				{
					case PointerInteraction.LClick:
						layer.enabled = !layer.enabled;
						break;
					case PointerInteraction.RClick:
						layer.visible = !layer.visible;
						break;
					case PointerInteraction.MClick:
						SceneManager.activeScene.RemoveLayer(layer);
						break;
				}

				i++;

				foreach (var entity in layer.entities)
				{
					entities++;

					rect = new Rect(0, i * allSize, size.x, allSize);

					switch (gui.Button("| " + (entity.destroyed ? "§Error§" : "") + GetStatus(entity.enabled, entity.visible) + entity.GetType().Name, rect))
					{
						case PointerInteraction.LClick:
							Menuing.OpenMenu(new DMenuInspector(entity), Input.windowMousePosition);
							break;
						case PointerInteraction.RClick:
							entity.visible = !entity.visible;
							break;
						case PointerInteraction.MClick:
							entity.Destroy();
							break;
					}

					i++;

					foreach (var comp in entity.components)
					{
						components++;

						rect = new Rect(0, i * allSize, size.x, allSize);

						switch (gui.Button("| | " + GetStatus(comp.Value.enabled, comp.Value.visible) + comp.Key.Name, rect))
						{
							case PointerInteraction.LClick:
								comp.Value.enabled = !comp.Value.enabled;
								break;
							case PointerInteraction.RClick:
								comp.Value.visible = !comp.Value.visible;
								break;
							case PointerInteraction.MClick:
								entity.RemoveComponent(comp.Key);
								break;
						}

						i++;
					}
				}
			}
		}

		static string GetStatus(bool enabled, bool visible) =>
			$"{(enabled ? "§Enabled§" : "§Disabled§")} {(visible ? "§Visible§" : "§Invisible§")} ";
	}
}