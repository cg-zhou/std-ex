using Shouldly;
using StdEx.IO;

namespace StdEx.Tests.IO;

public class ContentTypeUtilsTests
{
    [Theory]
    [InlineData("test.html", "text/html")]
    [InlineData("test.js", "application/javascript")]
    [InlineData("test.css", "text/css")]
    [InlineData("test.json", "application/json")]
    [InlineData("test.png", "image/png")]
    [InlineData("test.jpg", "image/jpeg")]
    [InlineData("test.gif", "image/gif")]
    [InlineData("test.svg", "image/svg+xml")]
    [InlineData("test.ico", "image/x-icon")]
    [InlineData("test.unknown", "application/octet-stream")]
    public void GetContentType_WithDifferentExtensions_ShouldReturnCorrectMimeType(string path, string expectedContentType)
    {
        // Act
        var contentType = ContentTypeUtils.GetContentType(path);

        // Assert
        contentType.ShouldBe(expectedContentType);
    }

    [Theory]
    [InlineData("test")]
    [InlineData(".")]
    [InlineData("")]
    public void GetContentType_WithInvalidPath_ShouldReturnOctetStream(string path)
    {
        // Act
        var contentType = ContentTypeUtils.GetContentType(path);

        // Assert
        contentType.ShouldBe("application/octet-stream");
    }
}
