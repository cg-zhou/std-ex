using System;
using Xunit;
using System.Xml.Serialization;
using StdEx.Serialization;

namespace StdEx.Tests.Serialization
{
    public class XmlUtilsTests
    {
        [Fact]
        public void SerializeAndDeserialize_ShouldWork()
        {
            // Arrange
            var testObject = new TestClass
            {
                Id = 1,
                Name = "测试名称",
                CreatedDate = new DateTime(2024, 3, 20)
            };

            // Act
            string xml = XmlUtils.Serialize(testObject);
            var deserializedObject = XmlUtils.Deserialize<TestClass>(xml);

            // Assert
            Assert.NotNull(deserializedObject);
            Assert.Equal(testObject.Id, deserializedObject.Id);
            Assert.Equal(testObject.Name, deserializedObject.Name);
            Assert.Equal(testObject.CreatedDate, deserializedObject.CreatedDate);
        }

        [Fact]
        public void Serialize_NullObject_ShouldThrowException()
        {
            TestClass? testObject = null;
            Assert.Throws<ArgumentNullException>(() => XmlUtils.Serialize(testObject));
        }

        [Fact]
        public void Deserialize_EmptyString_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => XmlUtils.Deserialize<TestClass>(""));
        }
    }

    public class TestClass
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
} 