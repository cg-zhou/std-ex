using StdEx.Media.Tmdb;
using StdEx.Media.Tmdb.Models;
using StdEx.Serialization;
using StdEx.TmdbNfoLoader;
using System.Net.Http.Headers;

public partial class Program
{
    // Supported video file extensions
    private static readonly string[] VideoExtensions =
    [
        ".mp4", ".mkv", ".avi", ".mov",
        ".wmv", ".flv", ".m4v", ".rmvb"
    ];

    private const int retryCount = 3;
    private const int delaySeconds = 6000;

    static async Task Main(string[] args)
    {
        string path;
        var config = new TmdbConfig();

        if (args.Length >= 2)
        {
            path = args[0];
            config.BearerToken = args[1];
        }
        else if (args.Length == 1)
        {
            path = args[0];
            Console.WriteLine("Please input your TMDB bearer token:");
            var token = Console.ReadLine() ?? string.Empty;
            config.BearerToken = token;
        }
        else
        {
            Console.WriteLine("Please input a directory path:");
            path = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Please input your TMDB bearer token:");
            var token = Console.ReadLine() ?? string.Empty;
            config.BearerToken = token;
        }

        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(config.BearerToken))
        {
            Console.WriteLine("Usage: StdEx.TmdbNfoLoader.exe <directory_path> [bearer_token]");
            Console.WriteLine("Or run without arguments to input values interactively.");
            return;
        }

        if (!Directory.Exists(path))
        {
            Console.WriteLine($"Directory not found: {path}");
            return;
        }

        try
        {
            var tmdb = new TmdbUtils(config);

            var videoFiles = FindVideoFiles(path);
            await ProcessVideoFiles(videoFiles, tmdb, config);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }

        if (args.Length == 0)
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    private static async Task DelayAsync()
    {
        ConsoleHelper.WriteInfo($"Delay by throttler...");
        await Task.Delay(delaySeconds);
    }

    private static IEnumerable<FileInfo> FindVideoFiles(string path)
    {
        var directory = new DirectoryInfo(path);

        foreach (var file in directory.EnumerateFiles("*.*", SearchOption.AllDirectories)
            .Where(f => VideoExtensions.Contains(f.Extension.ToLower())))
        {
            yield return file;
        }
    }

    private static async Task ProcessVideoFiles(IEnumerable<FileInfo> files, TmdbUtils tmdb, TmdbConfig config)
    {
        var fileList = files.ToList();
        if (!fileList.Any())
        {
            ConsoleHelper.WriteWarning("No video files found.");
            return;
        }

        ConsoleHelper.WriteInfo("\nProcessing video files:");
        ConsoleHelper.WriteInfo("======================");

        int processed = 0;
        int skipped = 0;

        foreach (var file in fileList)
        {
            try
            {
                var directory = file.Directory!;
                var baseName = Path.GetFileNameWithoutExtension(file.Name);
                var nfoPath = Path.ChangeExtension(file.FullName, ".nfo");

                if (File.Exists(nfoPath))
                {
                    ConsoleHelper.WriteWarning($"Skipped {file.Name} (NFO already exists)");
                    skipped++;
                    continue;
                }

                var movieName = GetMovieName(file.Name);
                ConsoleHelper.WriteInfo($"\nProcessing movie: {movieName}");
                ConsoleHelper.WriteInfo("Searching TMDB database...");

                var movieNfo = await tmdb.GetMovieNfo(movieName);
                ConsoleHelper.WriteSuccess($"Found movie: {movieNfo.Title} ({movieNfo.Year})");

                // Set local image paths
                var posterPath = Path.Combine(directory.FullName, $"{baseName}-poster.jpg");
                var fanartPath = Path.Combine(directory.FullName, $"{baseName}-fanart.jpg");

                // Use absolute paths
                movieNfo.Art.LocalPoster = posterPath;
                movieNfo.Art.LocalFanart = fanartPath;

                ConsoleHelper.WriteInfo("Downloading poster image...");
                await DownloadImageAsync(movieNfo.Art.Poster, posterPath);

                ConsoleHelper.WriteInfo("Downloading fanart image...");
                await DownloadImageAsync(movieNfo.Art.Fanart, fanartPath);

                ConsoleHelper.WriteInfo("Generating NFO file...");
                var nfoContent = XmlUtils.Serialize(movieNfo);
                await File.WriteAllTextAsync(nfoPath, nfoContent);

                ConsoleHelper.WriteSuccess($"Successfully processed: {movieName}");
                await DelayAsync();

                processed++;
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError($"Failed to process: {file.Name}", ex);
                await DelayAsync();
            }
        }

        ConsoleHelper.WriteInfo("\nSummary:");
        ConsoleHelper.WriteInfo("======================");
        ConsoleHelper.WriteSuccess($"Processed: {processed} files");
        ConsoleHelper.WriteWarning($"Skipped: {skipped} files");
        ConsoleHelper.WriteInfo($"Total: {fileList.Count} files");
    }

    private static string GetMovieName(string fileName)
    {
        // Remove extension first
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

        // Split by dot and take first part
        var parts = nameWithoutExtension.Split('.');
        return parts[0].Trim();
    }

    private static async Task DownloadImageAsync(string url, string localPath)
    {
        if (string.IsNullOrEmpty(url))
        {
            ConsoleHelper.WriteWarning("Skipped: Image URL is empty");
            return;
        }

        if (File.Exists(localPath))
        {
            ConsoleHelper.WriteWarning($"Skipped: Image already exists at {localPath}");
            return;
        }

        for (int i = 0; i < retryCount; i++)
        {
            try
            {
                await DelayAsync();

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                ConsoleHelper.WriteInfo($"Downloading from: {url}");
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(localPath, imageBytes);
                    ConsoleHelper.WriteSuccess($"Image saved to: {localPath}");
                    return;
                }

                ConsoleHelper.WriteWarning($"Download failed (Attempt {i + 1}/{retryCount}), Status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError($"Download failed (Attempt {i + 1}/{retryCount})", ex);
            }
        }

        ConsoleHelper.WriteError($"Failed to download image after {retryCount} attempts: {url}");
    }
}
