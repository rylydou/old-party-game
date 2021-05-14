using System;
using System.Collections.Generic;

namespace MGE.ECS
{
	[System.Serializable]
	public class Scene
	{
		public string name;

		public bool doneCleaningUp { get; private set; } = false;
		public Action onDoneCleaningUp = () => { };

		public List<Layer> layers = new List<Layer>();

		public bool clearScreen = true;
		public Color screenClearColor = Color.black;

		public Scene() { }

		public Scene(params Layer[] layers)
		{
			if (layers != null)
			{
				foreach (var layer in layers)
				{
					if (layer.scene != null)
						throw new Exception("Layer aready has an owner!");
					layer.scene = this;
					this.layers.Add(layer);
				}
			}

			Init();
		}

		public void AddLayer(Layer layer)
		{
			layer.scene = this;
			layers.Add(layer);
		}

		public void RemoveLayer(Layer layer)
		{
			layer.scene = null;
			layers.Remove(layer);
		}

		public Layer GetLayer(string name)
		{
			return layers.Find(layer => layer.name == name);
		}

		#region Updates
		internal virtual void Init()
		{
			foreach (var layer in layers)
			{
				if (!layer.enabled) continue;

				layer.Init();
			}
		}

		public virtual void Tick()
		{
			foreach (var layer in layers)
			{
				if (!layer.enabled) continue;

				layer.Tick();
			}
		}

		public virtual void Update()
		{
			foreach (var layer in layers)
			{
				if (!layer.enabled) continue;

				layer.Update();
			}
		}

		public virtual void Draw()
		{
			if (clearScreen)
				Engine.game.GraphicsDevice.Clear(screenClearColor);

			foreach (var layer in layers)
			{
				if (!layer.visible) continue;
				if (layer.isUI) continue;

				layer.Draw();
			}
		}

		public virtual void DrawUI()
		{
			foreach (var layer in layers)
			{
				if (!layer.enabled) continue;
				if (!layer.isUI) continue;

				layer.DrawUI();
			}
		}

		public void CleanUp()
		{
			using (var timmer = Timmer.Start("Screen Cleanup"))
			{
				OnCleanUp();
				DoneCleaningUp();
			}
		}

		protected virtual void OnCleanUp()
		{
			foreach (var layer in layers)
			{
				layer.DestroyAllEntites();
			}
		}

		protected virtual void DoneCleaningUp()
		{
			doneCleaningUp = true;
			onDoneCleaningUp.Invoke();
		}
		#endregion
	}
}