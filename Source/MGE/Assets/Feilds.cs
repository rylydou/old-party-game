using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public class Params
	{
		[JsonProperty] public Dictionary<string, object> feilds;

		public Params(Dictionary<string, object> feilds)
		{
			this.feilds = feilds;
		}

		public bool GetBool(string key)
		{
			return (bool)feilds[key];
		}

		public int GetInt(string key)
		{
			return System.Convert.ToInt32(feilds[key]);
		}

		public float GetFloat(string key)
		{
			return System.Convert.ToSingle(feilds[key]);
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
			var values = (JArray)feilds[key];
			return new Vector2((float)values[0], (float)values[1]);
		}

		public Vector2Int GetVector2Int(string key)
		{
			var values = (JArray)feilds[key];
			return new Vector2Int((int)values[0], (int)values[1]);
		}

		public Rect GetRect(string key)
		{
			var values = (JArray)feilds[key];
			return new Rect((float)values[0], (float)values[1], (float)values[2], (float)values[3]);
		}

		public RectInt GetRectInt(string key)
		{
			var values = (JArray)feilds[key];
			return new RectInt((int)values[0], (int)values[1], (int)values[2], (int)values[3]);
		}
	}
}