using StdEx.Media.Tmdb;

public class Program
{
    // 支持的视频文件扩展名
    private static readonly string[] VideoExtensions =
    [
        ".mp4", ".mkv", ".avi", ".mov",
        ".wmv", ".flv", ".m4v", ".rmvb"
    ];

    static async Task Main(string[] args)
    {
        string path;
        string token;

        if (args.Length >= 2)
        {
            path = args[0];
            token = args[1];
        }
        else if (args.Length == 1)
        {
            path = args[0];
            Console.WriteLine("Please input your TMDB bearer token:");
            token = Console.ReadLine() ?? string.Empty;
        }
        else
        {
            Console.WriteLine("Please input a directory path:");
            path = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Please input your TMDB bearer token:");
            token = Console.ReadLine() ?? string.Empty;
        }

        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(token))
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
            var tmdb = new TmdbUtils(token);
            var videoFiles = FindVideoFiles(path);
            await ProcessVideoFiles(videoFiles, tmdb);
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

    private static IEnumerable<FileInfo> FindVideoFiles(string path)
    {
        var directory = new DirectoryInfo(path);

        foreach (var file in directory.EnumerateFiles("*.*", SearchOption.AllDirectories)
            .Where(f => VideoExtensions.Contains(f.Extension.ToLower())))
        {
            yield return file;
        }
    }

    private static async Task ProcessVideoFiles(IEnumerable<FileInfo> files, TmdbUtils tmdb)
    {
        var fileList = files.ToList();
        if (!fileList.Any())
        {
            Console.WriteLine("No video files found.");
            return;
        }

        Console.WriteLine("\nProcessing video files:");
        Console.WriteLine("======================");

        foreach (var file in fileList)
        {
            try
            {
                Console.Write($"Processing {file.Name}... ");

                // 使用文件名（不含扩展名）作为电影名称
                var movieName = Path.GetFileNameWithoutExtension(file.Name);
                var nfoContent = await tmdb.GenerateMovieNfo(movieName);

                // 创建同名的 .nfo 文件
                var nfoPath = Path.ChangeExtension(file.FullName, ".nfo");
                await File.WriteAllTextAsync(nfoPath, nfoContent);

                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");
            }
        }

        Console.WriteLine($"\nProcessed: {fileList.Count} files");
    }
}
