using System.Reflection;
using MGE.ECS;
using MGE.InputSystem;

namespace MGE.Debug.Menus
{
	public class DMenuInspector : DebugMenu
	{
		public override string name => "Inspector";

		const int itemSize = 24;

		readonly Color disabledColor = Colors.textDark;
		readonly Color enabledColor = Colors.text;

		public Entity entity;

		public DMenuInspector(Entity entity)
		{
			this.entity = entity;
		}

		public override void UpdateBG()
		{
			if (entity is null) Close();

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

				foreach (var field in comp.Key.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
				{
					if (field.GetCustomAttribute<HideInInspectorAttribute>() != null) continue;

					rect = new Rect(0, i * itemSize, size.x, itemSize);

					var value = field.GetValue(comp.Value)?.ToString();

					if (field.FieldType.ToString() == value || string.IsNullOrEmpty(value))
						gui.Text($"| | {field.Name}: " + (string.IsNullOrEmpty(value) ? "§Null§" : "!§Null§"), rect.position + offset, Colors.text);
					else
						gui.Text($"| | {field.Name}: {value}", rect.position + offset, Colors.text);

					i++;
				}
			}
		}

		static string GetStatus(bool enabled, bool visible) =>
			$"{(enabled ? "§Enabled§" : "§Disabled§")} {(visible ? "§Visible§" : "§Invisible§")} ";
	}
}