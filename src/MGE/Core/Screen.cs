using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE
{
	public abstract class Screen : EssentialVars
	{
		public string name;

		bool _doneCleaningUp = false;
		public bool doneCleaningUp { get => _doneCleaningUp; set => _doneCleaningUp = value; }
		public Action onDoneCleaningUp = () => { };

		public void Init() => OnInit();
		protected virtual void OnInit() { }

		public void Update() => OnUpdate();
		protected virtual void OnUpdate() { }

		public void Draw() => OnDraw();
		protected virtual void OnDraw() { }

		public void DrawUI() => OnDrawUI();
		protected virtual void OnDrawUI() { }

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
	}
}