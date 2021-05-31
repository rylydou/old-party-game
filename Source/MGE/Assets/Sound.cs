using Microsoft.Xna.Framework.Audio;

namespace MGE
{
	public class Sound : Asset
	{
		public override string extension => ".wav";

		public SoundEffect sound;

		public Sound() { }

		public override void Load(string fullPath, string localPath = null)
		{
			base.Load(fullPath, localPath);

			sound = SoundEffect.FromFile(fullPath);
		}

		public void Play(float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f)
		{
			sound.Play(Math.Clamp01(volume), Math.Clamp11(pitch), Math.Clamp11(pan));
		}
	}
}