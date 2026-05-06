// Program: Temporary File Cleaner
// Description: Finds and cleans temporary, cache, and log files from directories

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Temporary File Cleaner ===");
        Console.Write("Enter directory to clean: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        Console.Write("Search subdirectories? (y/n): ");
        bool recursive = Console.ReadLine().Trim().ToLower() == "y";

        Console.Write("Minimum file age in days (delete files older than this): ");
        if (!int.TryParse(Console.ReadLine(), out int minAgeDays) || minAgeDays < 0)
        {
            minAgeDays = 7;
        }

        Console.Write("Perform actual deletion? (y = delete, n = simulate): ");
        bool actuallyDelete = Console.ReadLine().Trim().ToLower() == "y";

        var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var tempExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".tmp", ".temp", ".bak", ".backup", ".old", ".log", ".cache",
            ".thumb", ".thumbs", ".ds_store", ".~", ".swp", ".swo",
            ".pch", ".idb", ".ilk", ".pdb", ".obj", ".lib",
            ".pyc", ".pyo", "__pycache__", ".class",
            ".orig", ".rej", ".merge",
            ".zip", ".rar" // Optional: archives
        };

        var tempFiles = new List<CleanupCandidate>();
        DateTime cutoff = DateTime.Now.AddDays(-minAgeDays);

        try
        {
            string[] files = Directory.GetFiles(dirPath, "*.*", searchOption);

            foreach (string file in files)
            {
                try
                {
                    var info = new FileInfo(file);
                    string ext = info.Extension;
                    string name = info.Name.ToLower();

                    bool isTemp = tempExtensions.Contains(ext)
                        || name.StartsWith("~")
                        || name.StartsWith("._")
                        || name.EndsWith(".tmp")
                        || name.Contains(".cache")
                        || ext == "" && name.StartsWith("Thumbs");

                    if (isTemp && info.LastWriteTime < cutoff)
                    {
                        tempFiles.Add(new CleanupCandidate
                        {
                            Path = file,
                            Size = info.Length,
                            Extension = ext,
                            LastModified = info.LastWriteTime,
                            Reason = GetReason(ext, name)
                        });
                    }
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error scanning: {ex.Message}");
            return;
        }

        if (tempFiles.Count == 0)
        {
            Console.WriteLine("\nNo temporary files found matching criteria.");
            return;
        }

        long totalSize = tempFiles.Sum(f => f.Size);

        Console.WriteLine($"\n--- Found {tempFiles.Count} Temporary Files ---");
        Console.WriteLine($"  Total size: {FormatSize(totalSize)}");
        Console.WriteLine($"  Minimum age: {minAgeDays} days");
        Console.WriteLine($"  Mode: {(actuallyDelete ? "DELETE" : "SIMULATE")}\n");

        Console.WriteLine($"  {"Size",-10} {"Last Modified",-20} {"Reason",-20} {"Path"}");
        Console.WriteLine(new string('-', 100));

        foreach (var file in tempFiles.OrderByDescending(f => f.Size))
        {
            Console.WriteLine($"  {FormatSize(file.Size),-10} {file.LastModified:yyyy-MM-dd HH:mm,-20} {file.Reason,-20} {file.Path}");
        }

        if (actuallyDelete)
        {
            Console.Write($"\nDelete {tempFiles.Count} files ({FormatSize(totalSize)})? (y/n): ");
            if (Console.ReadLine().Trim().ToLower() == "y")
            {
                int deleted = 0;
                long freedSpace = 0;
                foreach (var file in tempFiles)
                {
                    try
                    {
                        File.Delete(file.Path);
                        freedSpace += file.Size;
                        deleted++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  Failed: {file.Path} ({ex.Message})");
                    }
                }
                Console.WriteLine($"\n--- Cleanup Complete ---");
                Console.WriteLine($"  Deleted: {deleted} files");
                Console.WriteLine($"  Freed:   {FormatSize(freedSpace)}");
            }
        }
        else
        {
            Console.WriteLine($"\n--- Simulation Complete ---");
            Console.WriteLine($"  Would delete: {tempFiles.Count} files");
            Console.WriteLine($"  Would free:   {FormatSize(totalSize)}");
            Console.WriteLine($"  Run again with 'y' to actually delete.");
        }
    }

    static string GetReason(string ext, string name)
    {
        if (ext == ".tmp" || ext == ".temp") return "Temporary file";
        if (ext == ".bak" || ext == ".backup") return "Backup file";
        if (ext == ".old") return "Old version";
        if (ext == ".log") return "Log file";
        if (ext == ".cache") return "Cache file";
        if (ext == ".pyc" || ext == ".pyo") return "Python cache";
        if (ext == ".class") return "Java class";
        if (name.StartsWith("~")) return "Temp/draft file";
        return "Temporary type";
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
        return order == 0 ? $"{bytes}B" : $"{size:F1}{sizes[order]}";
    }
}

class CleanupCandidate
{
    public string Path { get; set; }
    public long Size { get; set; }
    public string Extension { get; set; }
    public DateTime LastModified { get; set; }
    public string Reason { get; set; }
}
