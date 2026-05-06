// Program: Disk Usage Analyzer
// Description: Shows file and folder sizes in a directory with visual bar chart

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Disk Usage Analyzer ===");
        Console.Write("Enter directory path: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        Console.Write("Max depth (1 = immediate children only): ");
        if (!int.TryParse(Console.ReadLine(), out int maxDepth) || maxDepth < 1)
        {
            maxDepth = 1;
        }

        Console.WriteLine($"\n--- Analyzing: {dirPath} ---");
        var entries = AnalyzeDirectory(dirPath, maxDepth);

        if (entries.Count == 0)
        {
            Console.WriteLine("Directory is empty.");
            return;
        }

        long totalSize = entries.Sum(e => e.Size);
        long maxSize = entries.Max(e => e.Size);

        Console.WriteLine($"\n  {"Name",-30} {"Size",-12} {"%",-6} Usage");
        Console.WriteLine(new string('-', 70));

        foreach (var entry in entries.OrderByDescending(e => e.Size))
        {
            double percent = (double)entry.Size / totalSize * 100;
            int barLength = maxSize > 0 ? (int)((double)entry.Size / maxSize * 30) : 0;
            string bar = new string('█', Math.Max(1, barLength));
            Console.WriteLine($"  {entry.Name,-30} {FormatSize(entry.Size),-12} {percent,5:F1}% {bar}");
        }

        Console.WriteLine(new string('-', 70));
        Console.WriteLine($"  {"Total",-30} {FormatSize(totalSize),-12}");
        Console.WriteLine($"  {"Items",-30} {entries.Count}");
    }

    static List<DiskEntry> AnalyzeDirectory(string path, int maxDepth)
    {
        var entries = new List<DiskEntry>();

        try
        {
            var dirInfo = new DirectoryInfo(path);

            // Files in current directory
            foreach (var file in dirInfo.GetFiles())
            {
                try
                {
                    entries.Add(new DiskEntry { Name = file.Name, Size = file.Length, Type = "File" });
                }
                catch { }
            }

            // Subdirectories
            if (maxDepth > 1)
            {
                foreach (var dir in dirInfo.GetDirectories())
                {
                    try
                    {
                        long dirSize = GetDirectorySize(dir.FullName, maxDepth - 1);
                        entries.Add(new DiskEntry { Name = dir.Name + "/", Size = dirSize, Type = "Folder" });
                    }
                    catch { }
                }
            }
            else
            {
                foreach (var dir in dirInfo.GetDirectories())
                {
                    try
                    {
                        long dirSize = GetDirectorySize(dir.FullName, 1);
                        entries.Add(new DiskEntry { Name = dir.Name + "/", Size = dirSize, Type = "Folder" });
                    }
                    catch { }
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Warning: Access denied to some files.");
        }

        return entries;
    }

    static long GetDirectorySize(string path, int depth)
    {
        long total = 0;
        try
        {
            var dirInfo = new DirectoryInfo(path);
            foreach (var file in dirInfo.GetFiles())
            {
                try { total += file.Length; } catch { }
            }
            if (depth > 1)
            {
                foreach (var subDir in dirInfo.GetDirectories())
                {
                    try { total += GetDirectorySize(subDir.FullName, depth - 1); } catch { }
                }
            }
        }
        catch { }
        return total;
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

class DiskEntry
{
    public string Name { get; set; }
    public long Size { get; set; }
    public string Type { get; set; }
}
