using System.IO;

namespace StdEx.Net
{
    /// <summary>
    /// Provides utility methods for working with MIME types.
    /// </summary>
    public static class MimeTypeUtils
    {
        /// <summary>
        /// Gets the MIME type for the specified file path based on its extension.
        /// </summary>
        /// <param name="path">The file path whose MIME type to determine.</param>
        /// <returns>
        /// A string containing the MIME type. If the file extension is not recognized,
        /// returns "application/octet-stream".
        /// </returns>
        public static string GetMimeType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".html" => "text/html",
                ".js" => "application/javascript",
                ".css" => "text/css",
                ".json" => "application/json",
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".gif" => "image/gif",
                ".svg" => "image/svg+xml",
                ".ico" => "image/x-icon",
                _ => "application/octet-stream",
            };
        }
    }
}