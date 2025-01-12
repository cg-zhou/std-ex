using Newtonsoft.Json;

namespace StdEx.Serialization
{
    public static class JsonUtils
    {
        public static T Deserialize<T>(string json) where T : class
        {
            var result = JsonConvert.DeserializeObject<T>(json);
            if (result == null)
            {
                throw new JsonSerializationException($"Failed to deserialize to type {typeof(T).Name}");
            }
            return result;
        }

        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}