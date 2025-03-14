using System.IO;
using System.Reflection;

namespace StdEx.IO
{
    public static class AssemblyExtensions
    {
        public static Stream? GetEmbeddedResource(
            this Assembly assembly, string path, string? rootNamespace = null)
        {
            path = path.TrimStart('/');
            if (string.IsNullOrEmpty(path))
            {
                path = "index.html";
            }

            var resourcePath = rootNamespace != null
                ? $"{rootNamespace}.{path.Replace('/', '.')}"
                : path.Replace('/', '.');

            return assembly.GetManifestResourceStream(resourcePath);
        }
    }
}