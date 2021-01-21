using System.Text.Json;
using System.Text.Json.Serialization;

namespace MGE.FileIO
{
	public class Serializer
	{
		public static string Serialize(object obj)
		{
			Logger.Log(obj.GetType());

			return JsonSerializer.Serialize(obj, obj.GetType());
		}

		public static T Deserialize<T>(string data)
		{
			return JsonSerializer.Deserialize<T>(data);
		}
	}
}