using System.Collections.Generic;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public class Feilds
	{
		[JsonProperty] public Dictionary<string, string> feilds;
		public Dictionary<string, object> solvedFeilds;

		public string GetString(string key)
		{
			return feilds[key];
		}

		public int GetInt(string key)
		{
			int value;
			if (!solvedFeilds.ContainsKey(key))
			{
				value = int.Parse(GetString(key));
				solvedFeilds.Add(key, value);
			}
			return (int)solvedFeilds[key];
		}

		public float GetFloat(string key)
		{
			float value;
			if (!solvedFeilds.ContainsKey(key))
			{
				value = float.Parse(GetString(key));
				solvedFeilds.Add(key, value);
			}
			return (float)solvedFeilds[key];
		}

		public Vector2 GetVector2(string key)
		{
			Vector2 value;
			if (!solvedFeilds.ContainsKey(key))
			{
				var values = GetString(key).Split(", ");
				value = new Vector2(float.Parse(values[0]), float.Parse(values[1]));
				solvedFeilds.Add(key, value);
			}
			return (Vector2)solvedFeilds[key];
		}

		public Vector2Int GetVector2Int(string key)
		{
			Vector2Int value;
			if (!solvedFeilds.ContainsKey(key))
			{
				var values = GetString(key).Split(", ");
				value = new Vector2Int(int.Parse(values[0]), int.Parse(values[1]));
				solvedFeilds.Add(key, value);
			}
			return (Vector2Int)solvedFeilds[key];
		}

		public Rect GetRect(string key)
		{
			Rect value;
			if (!solvedFeilds.ContainsKey(key))
			{
				var values = GetString(key).Split(", ");
				value = new Rect(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
				solvedFeilds.Add(key, value);
			}
			return (Rect)solvedFeilds[key];
		}

		public RectInt GetRectInt(string key)
		{
			RectInt value;
			if (!solvedFeilds.ContainsKey(key))
			{
				var values = GetString(key).Split(", ");
				value = new Rect(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]));
				solvedFeilds.Add(key, value);
			}
			return (RectInt)solvedFeilds[key];
		}
	}
}