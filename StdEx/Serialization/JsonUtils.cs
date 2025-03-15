using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;

namespace StdEx.Serialization
{
    /// <summary>
    /// Provides utility methods for JSON serialization and deserialization.
    /// </summary>
    public static class JsonUtils
    {
        /// <summary>
        /// Default serializer settings with string enum converter.
        /// </summary>
        private static readonly JsonSerializerSettings _defaultSettings = new JsonSerializerSettings
        {
            Converters = new JsonConverter[] { new StringEnumConverter() },
        };

        /// <summary>
        /// Camel case serializer settings with string enum converter.
        /// </summary>
        private static readonly JsonSerializerSettings _camelCaseSettings = new JsonSerializerSettings
        {
            Converters = new JsonConverter[] { new StringEnumConverter() },
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// Deserializes a JSON string into an object of specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <param name="useCamelCase">When true, uses camel case property naming strategy.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="Exception">Thrown when deserialization fails or results in a null value.</exception>
        public static T Deserialize<T>(string json, bool useCamelCase = false) where T : class
        {
            var settings = useCamelCase ? _camelCaseSettings : _defaultSettings;
            var result = JsonConvert.DeserializeObject<T>(json, settings);
            if (result == null)
            {
                throw new Exception($"Failed to deserialize to type {typeof(T).Name}");
            }
            return result;
        }

        /// <summary>
        /// Serializes an object to a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="useCamelCase">When true, uses camel case property naming strategy.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string Serialize<T>(T obj, bool useCamelCase = false)
        {
            var settings = useCamelCase ? _camelCaseSettings : _defaultSettings;
            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}