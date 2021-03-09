using System.Reflection;
using MGE.ECS;
using MGE.InputSystem;

namespace MGE.Debug.Menus
{
	public class DMenuInspector : DebugMenu
	{
		public override string name => "Inspector";

		const int itemSize = 24;
		const string disabledChar = "Ç";
		const string enabledChar = "â";
		const string invisChar = "é";
		const string visChar = "ü";

		readonly Vector2 offset = new Vector2(16, 16);
		readonly Color disabledColor = Colors.textDark;
		readonly Color enabledColor = Colors.text;

		public Entity entity;

		public DMenuInspector(Entity entity)
		{
			this.entity = entity;
		}

		public override void UpdateBG()
		{
			base.UpdateBG();

			if (entity == null)
			{
				gui.Text("No Entity Selected!", offset, Colors.text);

				return;
			}

			title = $"Inspecting {entity.GetType().Name}";

			gui.Text(entity.GetType().Name, offset, Colors.text);

			var i = 1;

			foreach (var comp in entity.components)
			{
				var rect = new Rect(0, i * itemSize, size.x, itemSize);
				var clickRect = new Rect(rect.position.x, rect.position.y + itemSize / 2, rect.size.x, rect.size.y);

				if (gui.MouseInside(clickRect))
				{
					gui.Image(clickRect, Colors.highlight);

					if (Input.GetButtonPress(Inputs.MouseLeft)) comp.Value.enabled = !comp.Value.enabled;
					else if (Input.GetButtonPress(Inputs.MouseRight)) comp.Value.visible = !comp.Value.visible;
					else if (Input.GetButtonPress(Inputs.MouseMiddle)) entity.RemoveComponent(comp.Key);
				}

				gui.Text("| " + GetStatus(comp.Value.enabled, comp.Value.visible) + comp.Key.Name, rect.position + offset, entity.layer.enabled && entity.enabled && comp.Value.enabled ? enabledColor : disabledColor);

				i++;

				foreach (var field in comp.Key.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
				{
					if (field.GetCustomAttribute<HideInInspectorAttribute>() != null) continue;

					rect = new Rect(0, i * itemSize, size.x, itemSize);

					var value = field.GetValue(comp.Value)?.ToString();

					if (field.FieldType.ToString() == value || string.IsNullOrEmpty(value))
						gui.Text($"| | {field.Name}: " + (string.IsNullOrEmpty(value) ? "Ø" : "!Ø"), rect.position + offset, Colors.text);
					else
						gui.Text($"| | {field.Name}: {value}", rect.position + offset, Colors.text);

					i++;
				}
			}
		}

		static string GetStatus(bool enabled, bool visible) =>
			$"{(enabled ? enabledChar : disabledChar)} {(visible ? visChar : invisChar)} ";
	}
}