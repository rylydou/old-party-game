using System.Collections.Generic;
using MGE;

namespace GAME.States
{
	public abstract class GameState
	{
		public Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

		public virtual void Init() { }

		public virtual void Tick() { }
		public virtual void Update() { }
		public virtual void Draw() { }
		public virtual void DrawUI() { }

		public virtual void Exit() { }

		public virtual void PlaySound(string path)
		{
			Sound sound = null;
			if (!sounds.TryGetValue(path, out sound))
			{
				sound = Assets.GetAsset<Sound>(path);
				sounds.Add(path, sound);
			}
			sound?.Play();
		}
	}
}