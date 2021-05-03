using MGE.Graphics;
using Microsoft.Xna.Framework.Audio;
using Newtonsoft.Json;

namespace MGE
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Sound
	{
		public string path;
		SoundEffect[] _sounds;
		public SoundEffect[] sounds
		{
			get
			{
				if (_sounds is null)
					_sounds = Assets.GetAssets<SoundEffect>(path);
				return _sounds;
			}
		}

		[JsonProperty] public Range volume = new Range(-0.05f, 0.05f);
		[JsonProperty] public Range pitch = new Range(-0.05f, 0.05f);
		[JsonProperty] public Range pan = new Range(-0.05f, 0.05f);

		public void Play(Vector2 position)
		{
			var pan = (position - Camera.position - (Vector2)Window.sceneSize / 2) / (Vector2)Window.sceneSize;

			sounds.Random().Play(Math.Clamp(volume.random * (1.0f - pan.abs.sqrMagnitude / 1.5f), 0.0f, 1.0f), Math.Clamp(this.pitch.random - pan.y / 4, 0.0f, 1.0f), Math.Clamp(pan.x / 2 + this.pan.random, -1.0f, 1.0f));
		}
	}
}