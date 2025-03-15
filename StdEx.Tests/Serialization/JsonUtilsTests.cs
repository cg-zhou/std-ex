using Newtonsoft.Json;
using Shouldly;
using StdEx.Serialization;
using Shouldly.Configuration;

namespace StdEx.Tests.Serialization;

public class JsonUtilsTests
{
    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    private enum TestEnum
    {
        First,
        Second,
        Third
    }

    private class EnumTestModel
    {
        public TestEnum EnumValue { get; set; }
        public string Text { get; set; } = string.Empty;
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

    [Fact]
    public void SerializeEnum_ShouldUseStringNotNumber()
    {
        // Arrange
        var model = new EnumTestModel { EnumValue = TestEnum.Second, Text = "Test" };

        // Act
        var json = JsonUtils.Serialize(model);

        // Assert
        json.ShouldContain("\"EnumValue\":\"Second\"");
        json.ShouldNotContain("\"EnumValue\":1");
    }

    [Fact]
    public void DeserializeEnum_ShouldAcceptStringRepresentation()
    {
        // Arrange
        string json = "{\"EnumValue\":\"Second\",\"Text\":\"Test\"}";

        // Act
        var result = JsonUtils.Deserialize<EnumTestModel>(json);

        // Assert
        result.ShouldNotBeNull();
        result.EnumValue.ShouldBe(TestEnum.Second);
        result.Text.ShouldBe("Test");
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

    [Fact]
    public void Serialize_WithCamelCase_ShouldConvertPropertyNames()
    {
        // Arrange
        var model = new TestModel { Name = "Test", Age = 25 };

        // Act
        var json = JsonUtils.Serialize(model, true);

        // Assert
        json.ShouldContain("\"name\":\"Test\"", Case.Sensitive);
        json.ShouldContain("\"age\":25", Case.Sensitive);
        json.ShouldNotContain("\"Name\"", Case.Sensitive);
        json.ShouldNotContain("\"Age\"", Case.Sensitive);
    }

    [Fact]
    public void Deserialize_WithCamelCase_ShouldWorkWithCamelCaseJson()
    {
        // Arrange
        string camelCaseJson = "{\"name\":\"Test\",\"age\":25}";

        // Act
        var result = JsonUtils.Deserialize<TestModel>(camelCaseJson, true);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Test");
        result.Age.ShouldBe(25);
    }

    [Fact]
    public void SerializeEnum_WithCamelCase_ShouldUseStringAndCamelCase()
    {
        // Arrange
        var model = new EnumTestModel { EnumValue = TestEnum.Second, Text = "Test" };

        // Act
        var json = JsonUtils.Serialize(model, true);

        // Assert
        json.ShouldContain("\"enumValue\":\"Second\"", Case.Sensitive);
        json.ShouldContain("\"text\":\"Test\"", Case.Sensitive);
        json.ShouldNotContain("\"EnumValue\"", Case.Sensitive);
        json.ShouldNotContain("\"Text\"", Case.Sensitive);
    }
}