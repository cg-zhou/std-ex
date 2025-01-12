using Newtonsoft.Json;

namespace StdEx.Serialization
{
    public static class JsonUtils
    {
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}