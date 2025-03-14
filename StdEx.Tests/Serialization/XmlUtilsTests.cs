using Shouldly;
using StdEx.Serialization;

namespace StdEx.Tests.Serialization;

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
        deserializedObject.ShouldNotBeNull();
        deserializedObject.Id.ShouldBe(testObject.Id);
        deserializedObject.Name.ShouldBe(testObject.Name);
        deserializedObject.CreatedDate.ShouldBe(testObject.CreatedDate);
    }

    [Fact]
    public void Serialize_NullObject_ShouldThrow()
    {
        // Arrange
        TestClass testObject = null!;

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => 
            XmlUtils.Serialize(testObject));
    }

    [Fact]
    public void Deserialize_EmptyString_ShouldThrow()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => 
            XmlUtils.Deserialize<TestClass>(""));
    }
}

public class TestClass
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedDate { get; set; }
}
