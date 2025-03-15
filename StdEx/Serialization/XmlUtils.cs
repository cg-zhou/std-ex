using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace StdEx.Serialization
{
    /// <summary>
    /// Provides utility methods for XML serialization and deserialization.
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// Cache for XML serializers to improve performance by avoiding repeated serializer creation.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, XmlSerializer> _serializers = new ConcurrentDictionary<Type, XmlSerializer>();

        /// <summary>
        /// Serializes an object to XML string.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>An XML string representation of the object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the object to serialize is null.</exception>
        public static string Serialize<T>(T obj) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var serializer = GetSerializer<T>();
            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8
            });

            serializer.Serialize(xmlWriter, obj);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Deserializes an XML string into an object of specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="xml">The XML string to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the XML string is null or empty.</exception>
        public static T Deserialize<T>(string xml) where T : class
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            var serializer = GetSerializer<T>();
            using (var stringReader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(stringReader)!;
            }
        }

        /// <summary>
        /// Gets or creates an XmlSerializer for the specified type from the cache.
        /// </summary>
        /// <typeparam name="T">The type for which to get a serializer.</typeparam>
        /// <returns>An XmlSerializer for the specified type.</returns>
        private static XmlSerializer GetSerializer<T>()
        {
            var type = typeof(T);
            return _serializers.GetOrAdd(type, t =>
            {
                return new XmlSerializer(t);
            });
        }
    }
}