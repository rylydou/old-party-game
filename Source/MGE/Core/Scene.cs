using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE
{
	public class Scene
	{
		public string name;

		bool _doneCleaningUp = false;
		public bool doneCleaningUp { get => _doneCleaningUp; set => _doneCleaningUp = value; }
		public Action onDoneCleaningUp = () => { };

		public List<Layer> layers = new List<Layer>();

		public int entityCount { get => layers.Sum((x) => x.entityCount); }
		public int componentCount { get => layers.Sum((x) => x.entityCount); }

		public Scene(List<Layer> layers = null)
		{
			if (layers != null)
			{
				foreach (var layer in layers)
				{
					if (layer.scene != null)
						throw new Exception("Layer aready has an owner!");
					layer.scene = this;
				}

				this.layers = layers;
			}
		}

		public void AddLayer(Layer layer)
		{
			layer.scene = this;
			layers.Add(layer);
		}

		#region Updates
		public virtual void FixedUpdate()
		{
			foreach (var layer in layers)
				layer.FixedUpdate();
		}

		public virtual void Update()
		{
			foreach (var layer in layers)
				layer.Update();
		}

		public virtual void Draw()
		{
			foreach (var layer in layers)
				if (!layer.isUI)
					layer.Draw();
		}

		public virtual void DrawUI()
		{
			foreach (var layer in layers)
				if (layer.isUI)
					layer.DrawUI();
		}

		public void CleanUp()
		{
			using (var timmer = Timmer.Create("Screen Cleanup"))
			{
				OnCleanUp();
				DoneCleaningUp();
			}
		}
		protected virtual void OnCleanUp() { }
		protected virtual void DoneCleaningUp()
		{
			_doneCleaningUp = true;
			onDoneCleaningUp.Invoke();
		}
		#endregion
	}
}