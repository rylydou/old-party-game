using System;
using MGE.InputSystem;
using MGE.UI;

namespace MGE.Debug.Menus
{
	public class DMenuTextInput : DebugMenu
	{
		public override string name => "Text Input";

		public TextFeildData data;
		public Action<string> onSubmit;
		public Action<string> onTyped;
		private TextFeildRule rule;

		public DMenuTextInput(string title, string text, Action<string> onSubmit, Action<string> onTyped = null, TextFeildRule? rule = null)
		{
			this.title = title;
			this.data = new TextFeildData(text) { isActive = true };
			this.onSubmit = onSubmit;
			this.onTyped = onTyped;
			this.rule = rule.HasValue ? rule.Value : TextFeildRule.all;

			this.size.y = barSize + offset.y * 2;
		}

		public override void UpdateBG()
		{
			base.UpdateBG();

			if (Input.GetButtonPress(Inputs.Escape))
				Close();

			if (Input.GetButtonPress(Inputs.Enter))
			{
				if (!rule.IsValid(data.text)) return;

				onSubmit?.Invoke(data.text);
				Close();
			}

			gui.TextFeild(ref data, new Rect(offset, gui.rect.width - offset.x * 2, allSize));

			if (data.lastTimeTyped == Time.unscaledTime)
			{
				onTyped?.Invoke(data.text);
			}

			data.isActive = true;
		}
	}
}