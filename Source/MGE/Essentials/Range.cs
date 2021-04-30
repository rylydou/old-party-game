using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public struct Range
	{
		[JsonProperty] public float min;
		[JsonProperty, JsonRequired] public float max;

		public float average { get => min + (max - min) / 2; }
		public float random { get => Random.Float(min, max); }

		public Range(float max)
		{
			this.min = 0.0f;
			this.max = max;
		}

		public Range(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public float Clamp(float value) => Math.Clamp(value, min, max);
	}
}