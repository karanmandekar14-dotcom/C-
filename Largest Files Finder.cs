// Program: Largest Files Finder
// Description: Finds the top N largest files in a directory tree

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Largest Files Finder ===");
        Console.Write("Enter directory path: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        Console.Write("Number of largest files to find: ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Error: Enter a positive number.");
            return;
        }

        Console.WriteLine("\nScanning... (this may take a moment for large directories)");

        try
        {
            var files = new List<FileInfo>();
            foreach (string path in Directory.EnumerateFiles(dirPath, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    var info = new FileInfo(path);
                    files.Add(info);
                }
                catch { }
            }

            var largest = files.OrderByDescending(f => f.Length).Take(count).ToList();

            Console.WriteLine($"\n--- Top {largest.Count} Largest Files ---");
            Console.WriteLine($"  {"#",-5} {"Size",-12} {"Path"}");
            Console.WriteLine(new string('-', 80));

            for (int i = 0; i < largest.Count; i++)
            {
                var file = largest[i];
                string relativePath = file.FullName.StartsWith(dirPath)
                    ? file.FullName.Substring(dirPath.Length + 1)
                    : file.FullName;

                Console.WriteLine($"  {i + 1,-5} {FormatSize(file.Length),-12} {relativePath}");
            }

            long totalSize = largest.Sum(f => f.Length);
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"  Total size of top {count} files: {FormatSize(totalSize)}");

            // Size breakdown by extension
            Console.WriteLine($"\n--- Size by Extension ---");
            var byExt = largest.GroupBy(f => f.Extension.ToLower())
                               .OrderByDescending(g => g.Sum(f => f.Length));

            foreach (var group in byExt)
            {
                long extSize = group.Sum(f => f.Length);
                int fileCount = group.Count();
                string bar = new string('█', (int)((double)extSize / totalSize * 30));
                Console.WriteLine($"  {group.Key,8} ({fileCount,3} files) {FormatSize(extSize),12} {bar}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static string FormatSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double size = bytes;
        int order = 0;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }
        return order == 0 ? $"{bytes} B" : $"{size:F1} {sizes[order]}";
    }
}
