namespace StdEx.TmdbNfoLoader;

internal static class ConsoleHelper
{
    private static void WriteWithTimestamp(string message, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"[{timestamp}] ");
        
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        
        Console.ForegroundColor = originalColor;
    }

    public static void WriteSuccess(string message)
    {
        WriteWithTimestamp(message, ConsoleColor.Green);
    }

    public static void WriteError(string message, Exception? ex = null)
    {
        WriteWithTimestamp(message, ConsoleColor.Red);
        
        if (ex != null)
        {
            var indent = "    ";
            var current = ex;
            while (current != null)
            {
                WriteWithTimestamp($"{indent}-> {current.GetType().Name}: {current.Message}", ConsoleColor.Red);
                if (current is HttpRequestException httpEx && httpEx.InnerException != null)
                {
                    WriteWithTimestamp($"{indent}   Status: {httpEx.StatusCode}", ConsoleColor.Red);
                }
                current = current.InnerException;
                indent += "    ";
            }
        }
    }

    public static void WriteWarning(string message)
    {
        WriteWithTimestamp(message, ConsoleColor.Yellow);
    }

    public static void WriteInfo(string message)
    {
        WriteWithTimestamp(message, ConsoleColor.Cyan);
    }
}
