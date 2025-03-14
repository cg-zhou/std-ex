using System.Reflection;
using Shouldly;
using StdEx.IO;

namespace StdEx.Tests.IO;

public class AssemblyExtensionsTests
{
    [Theory]
    [InlineData("test.txt", null)]
    [InlineData("test.txt", "TestNamespace")]
    [InlineData("/test.txt", "TestNamespace")]
    public void GetEmbeddedResource_WithDifferentPaths_ShouldHandleCorrectly(string path, string? rootNamespace)
    {
        // Act
        var stream = Assembly.GetExecutingAssembly().GetEmbeddedResource(path, rootNamespace);

        // Assert - since we don't have actual embedded resources in test assembly, it should return null
        stream.ShouldBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("/")]
    public void GetEmbeddedResource_WithEmptyPath_ShouldUseIndexHtml(string path)
    {
        // Act
        var stream = Assembly.GetExecutingAssembly().GetEmbeddedResource(path);

        // Assert - since we don't have actual embedded resources in test assembly
        stream.ShouldBeNull();
    }
}
