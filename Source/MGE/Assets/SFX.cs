using MGE.FileIO;
using MGE.Graphics;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public class SFX : Asset
	{
		public override string extension => ".sfx";

		Sound[] _sounds;
		public Sound[] sounds
		{
			get
			{
				if (_sounds is null)
					_sounds = Assets.GetAssets<Sound>(localPath.Replace(extension, string.Empty));
				return _sounds;
			}
		}

		[JsonProperty] public Range volume = new Range(-0.05f, 0.05f);
		[JsonProperty] public Range pitch = new Range(-0.05f, 0.05f);
		[JsonProperty] public Range pan = new Range(-0.05f, 0.05f);

		public override void Load(string fullPath, string localPath = null)
		{
			base.Load(fullPath, localPath);

			var s = IO.LoadJson<SFX>(fullPath);

			volume = s.volume;
			pitch = s.pitch;
			pan = s.pan;
		}

		public void Play()
		{
			sounds.Random()?.Play(Math.Clamp01(volume.random), Math.Clamp11(pitch.random), Math.Clamp11(pan.random));
		}

		public void Play(Vector2 position)
		{
			var pan = (position - Camera.position - (Vector2)Window.sceneSize / 2) / (Vector2)Window.sceneSize;

			sounds.Random()?.Play(Math.Clamp01(volume.random * (1.0f - pan.abs.sqrMagnitude / 1.5f)), Math.Clamp11(this.pitch.random - pan.y / 4), Math.Clamp01(pan.x / 2 - 0.5f + this.pan.random));
		}
	}
}