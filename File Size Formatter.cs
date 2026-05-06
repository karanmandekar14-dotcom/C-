// Program: File Size Formatter
// Description: Converts file sizes in bytes to human-readable format (KB, MB, GB, TB)

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== File Size Formatter ===");
        Console.WriteLine("Enter file size in bytes (or type 'exit' to quit):");

        while (true)
        {
            Console.Write("\nBytes: ");
            string input = Console.ReadLine();

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            if (!long.TryParse(input, out long bytes) || bytes < 0)
            {
                Console.WriteLine("Error: Please enter a valid non-negative number.");
                continue;
            }

            string result = FormatFileSize(bytes);
            Console.WriteLine($"Formatted size: {result}");
        }
    }

    static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB" };
        double size = bytes;
        int order = 0;

        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }

        return order == 0
            ? $"{bytes} {sizes[order]}"
            : $"{size:0.##} {sizes[order]}";
    }
}
