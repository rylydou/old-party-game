using MGE.ECS;
using MGE.InputSystem;
using MGE.StageSystem;
using MGE.StageSystem.Layers;

namespace MGE.Components
{
	public class CEditor : Component
	{
		public Stage stage = new Stage();

		public int layerIndex = 0;
		public StageLayer layer
		{
			get => stage.layers[layerIndex];
			set => stage.layers[layerIndex] = value;
		}

		bool shift = false;
		bool ctrl = false;
		bool alt = false;

		public override void Init()
		{
			stage.layers.Add(new IntLayer());
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

			if (!shift && !ctrl && !alt && Input.GetButton(Inputs.MouseLeft))
			{
				// Interact
				switch (layer.GetType().ToString())
				{
					case "MGE.StageSystem.Layers.IntLayer":
						break;
				}
			}
		}

		public void Save() { }

		public void Load() { }
	}
}