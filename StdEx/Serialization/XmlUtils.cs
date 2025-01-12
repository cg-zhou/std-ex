using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace StdEx.Serialization
{
    public static class XmlUtils
    {
        // Cache serializers to avoid repeated creation
        private static readonly ConcurrentDictionary<Type, XmlSerializer> _serializers = new ConcurrentDictionary<Type, XmlSerializer>();

        /// <summary>
        /// Serializes an object to XML string
        /// </summary>
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
        /// Deserializes an XML string to object
        /// </summary>
        public static T Deserialize<T>(string xml) where T : class
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            var serializer = GetSerializer<T>();
            using (var stringReader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }

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