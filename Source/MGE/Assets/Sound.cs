using Microsoft.Xna.Framework.Audio;

namespace MGE
{
	public class Sound
	{
		public SoundEffect[] sounds;

		public Range volume = new Range(0, 0);
		public Range pitch = new Range(0, 0);
		public Range pan = new Range(0, 0);

		public Sound(params SoundEffect[] sounds)
		{
			this.sounds = sounds;
		}

		public Sound(SoundEffect[] sounds, Range volume, Range pitch, Range pan)
		{
			this.sounds = sounds;
			this.volume = volume;
			this.pitch = pitch;
			this.pan = pan;
		}

		public void Play(Vector2 position, float volume = 1.0f, float pitch = 0.0f)
		{
			sounds.Random().Play(volume + this.volume.random, pitch + this.pitch.random, 0.0f + this.pan.random);
		}
	}
}