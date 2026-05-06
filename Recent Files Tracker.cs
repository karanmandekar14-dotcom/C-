// Program: Recent Files Tracker
// Description: Shows recently modified files in a directory with sorting and filtering

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Recent Files Tracker ===");
        Console.Write("Enter directory path: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        Console.Write("Search subdirectories? (y/n): ");
        bool recursive = Console.ReadLine().Trim().ToLower() == "y";

        Console.Write("Show files from last N days (0 = all): ");
        if (!int.TryParse(Console.ReadLine(), out int daysFilter) || daysFilter < 0)
        {
            daysFilter = 0;
        }

        Console.Write("File pattern (e.g., *.txt, *.cs, *.*): ");
        string pattern = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(pattern))
            pattern = "*.*";

        Console.Write("Number of results to show (0 = all): ");
        if (!int.TryParse(Console.ReadLine(), out int limit))
        {
            limit = 20;
        }

        var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var fileInfos = new List<FileInfo>();

        try
        {
            string[] files = Directory.GetFiles(dirPath, pattern, searchOption);
            DateTime cutoff = daysFilter > 0 ? DateTime.Now.AddDays(-daysFilter) : DateTime.MinValue;

            foreach (string file in files)
            {
                try
                {
                    var info = new FileInfo(file);
                    if (info.LastWriteTime >= cutoff)
                    {
                        fileInfos.Add(info);
                    }
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return;
        }

        if (fileInfos.Count == 0)
        {
            Console.WriteLine("No recent files found.");
            return;
        }

        // Sort by most recently modified
        var recentFiles = fileInfos.OrderByDescending(f => f.LastWriteTime);
        if (limit > 0)
            recentFiles = recentFiles.Take(limit);

        Console.WriteLine($"\n--- Recent Files ({fileInfos.Count} total) ---");
        Console.WriteLine($"  {"Modified",-20} {"Size",-10} {"Extension",-10} {"Path"}");
        Console.WriteLine(new string('-', 85));

        foreach (var file in recentFiles)
        {
            string relativePath = file.FullName.StartsWith(dirPath)
                ? file.FullName.Substring(dirPath.Length + 1)
                : file.FullName;

            string timeStr = GetRelativeTimeString(file.LastWriteTime);
            string ext = file.Extension.ToLower();

            Console.WriteLine($"  {timeStr,-20} {FormatSize(file.Length),-10} {ext,-10} {relativePath}");
        }

        // Summary by extension
        Console.WriteLine($"\n--- Files by Type ---");
        var byExt = fileInfos.GroupBy(f => f.Extension.ToLower())
            .OrderByDescending(g => g.Count())
            .Take(10);

        foreach (var group in byExt)
        {
            string bar = new string('█', group.Count());
            Console.WriteLine($"  {group.Key,8} ({group.Count(),3}) {bar}");
        }
    }

    static string GetRelativeTimeString(DateTime date)
    {
        TimeSpan diff = DateTime.Now - date;
        if (diff.TotalSeconds < 60)
            return $"{(int)diff.TotalSeconds}s ago";
        if (diff.TotalMinutes < 60)
            return $"{(int)diff.TotalMinutes}m ago";
        if (diff.TotalHours < 24)
            return $"{(int)diff.TotalHours}h ago";
        if (diff.TotalDays < 7)
            return $"{(int)diff.TotalDays}d ago";
        return date.ToString("yyyy-MM-dd");
    }

    static string FormatSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double size = bytes;
        int order = 0;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }
        return order == 0 ? $"{bytes}B" : $"{size:F0}{sizes[order]}";
    }
}
