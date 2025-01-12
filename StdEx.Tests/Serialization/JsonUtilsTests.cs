using Newtonsoft.Json;
using Shouldly;
using StdEx.Serialization;

namespace StdEx.Tests.Serialization
{
    public class JsonUtilsTests
    {
        private class TestModel
        {
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
        }

        [Fact]
        public void SerializeAndDeserialize_ShouldWork()
        {
            // Arrange
            var model = new TestModel { Name = "Test", Age = 25 };

            // Act
            var json = JsonUtils.Serialize(model);
            var result = JsonUtils.Deserialize<TestModel>(json);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe(model.Name);
            result.Age.ShouldBe(model.Age);
        }

        [Theory]
        [InlineData("{", "Unexpected end when reading JSON")] // Unclosed object
        [InlineData("[1, 2,]", "Cannot deserialize the current JSON array")] // Invalid array format
        public void Deserialize_WithInvalidJson_ShouldThrow_JsonSerializationException(string invalidJson, string expectedErrorMessage)
        {
            // Act & Assert
            var exception = Should.Throw<JsonSerializationException>(() => 
                JsonUtils.Deserialize<TestModel>(invalidJson));

            exception.Message.ShouldContain(expectedErrorMessage);
        }

        [Theory]
        [InlineData("invalid", "Unexpected character")] // Not a JSON string
        [InlineData("{\"name\": }", "Unexpected character encountered while parsing value")] // Invalid property value
        public void Deserialize_WithInvalidJson_ShouldThrowJsonReaderException(string invalidJson, string expectedErrorMessage)
        {
            // Act & Assert
            var exception = Should.Throw<JsonReaderException>(() =>
                JsonUtils.Deserialize<TestModel>(invalidJson));

            exception.Message.ShouldContain(expectedErrorMessage);
        }
    }
}