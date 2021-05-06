using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public class Feilds
	{
		[JsonProperty] public Dictionary<string, object> feilds;

		public Feilds(Dictionary<string, object> feilds)
		{
			this.feilds = feilds;
		}

		public bool GetBool(string key)
		{
			return (bool)feilds[key];
		}

		public int GetInt(string key)
		{
			return (int)feilds[key];
		}

		public float GetFloat(string key)
		{
			return (float)feilds[key];
		}

		public string GetString(string key)
		{
			return (string)feilds[key];
		}

		public Color GetColor(string key)
		{
			return Color.FromHex(GetString(key));
		}

		public Vector2 GetVector2(string key)
		{
			var values = (float[])feilds[key];
			return new Vector2(values[0], values[1]);
		}

		public Vector2Int GetVector2Int(string key)
		{
			var values = (int[])feilds[key];
			return new Vector2Int(values[0], values[1]);
		}

		public Rect GetRect(string key)
		{
			var values = (float[])feilds[key];
			return new Rect(values[0], values[1], values[2], values[3]);
		}

		public RectInt GetRectInt(string key)
		{
			var values = (int[])feilds[key];
			return new RectInt(values[0], values[1], values[2], values[3]);
		}
	}
}