using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MGE.FileIO
{
	public static class Serializer
	{
		public static string SerializeJson(object obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.Indented);
		}

		public static T DeserializeJson<T>(string data)
		{
			// JsonConvert.DefaultSettings += () =>
			// {
			// 	return new JsonSerializerSettings()
			// 	{
			// 		TypeNameHandling = TypeNameHandling.All,
			// 	};
			// };
			return JsonConvert.DeserializeObject<T>(data);
		}
	}
}