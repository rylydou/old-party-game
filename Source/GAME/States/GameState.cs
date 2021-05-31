using System.Collections.Generic;
using MGE;

namespace GAME.States
{
	public abstract class GameState
	{
		public Dictionary<string, SFX> sounds = new Dictionary<string, SFX>();

		public virtual void Init() { }

		public virtual void Tick() { }
		public virtual void Update() { }
		public virtual void Draw() { }
		public virtual void DrawUI() { }

		public virtual void Exit() { }

		public virtual void PlaySound(string path)
		{
			SFX sound = null;
			if (!sounds.TryGetValue(path, out sound))
			{
				sound = Assets.GetAsset<SFX>(path);
				sounds.Add(path, sound);
			}
			sound?.Play();
		}
	}
}