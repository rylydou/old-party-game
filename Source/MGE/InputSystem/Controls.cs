namespace MGE.InputSystem
{
	public abstract class Controls
	{
		public bool isEnabled = true;
		public bool isConnected = false;

		public string name;

		public SpriteSheet inputsSheet;

		protected Controls()
		{
			inputsSheet = Assets.GetAsset<SpriteSheet>($"UI/Controls/{name}");
		}

		public abstract void Update();

		public virtual void DrawInput(string input, Rect rect, Color? color)
		{
			if (!color.HasValue)
				color = Color.white;

			inputsSheet?.Draw(input, rect, color.Value);
		}
	}
}