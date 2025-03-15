using Shouldly;
using StdEx.Net;

namespace StdEx.Tests.Net;

public class MimeTypeUtilsTests
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
    public void GetMimeType_WithDifferentExtensions_ShouldReturnCorrectMimeType(string path, string expectedMimeType)
    {
        // Act
        var mimeType = MimeTypeUtils.GetMimeType(path);

        // Assert
        mimeType.ShouldBe(expectedMimeType);
    }

    [Theory]
    [InlineData("test")]
    [InlineData(".")]
    [InlineData("")]
    public void GetMimeType_WithInvalidPath_ShouldReturnOctetStream(string path)
    {
        // Act
        var mimeType = MimeTypeUtils.GetMimeType(path);

        // Assert
        mimeType.ShouldBe("application/octet-stream");
    }
}
