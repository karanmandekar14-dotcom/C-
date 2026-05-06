// Program: Async Log File Aggregator
// Description: Reads and aggregates multiple log files asynchronously with parallel processing

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("=== Async Log File Aggregator ===\n");

        Console.Write("Enter directory containing log files: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        Console.Write("Log file pattern (e.g., *.log): ");
        string pattern = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(pattern))
            pattern = "*.log";

        Console.Write("Minimum log level to include (DEBUG, INFO, WARN, ERROR): ");
        string minLevel = Console.ReadLine().Trim().ToUpper() ?? "DEBUG";

        string[] logFiles = Directory.GetFiles(dirPath, pattern, SearchOption.AllDirectories);

        if (logFiles.Length == 0)
        {
            Console.WriteLine("No log files found.");
            return;
        }

        Console.WriteLine($"\nFound {logFiles.Length} log file(s). Processing...\n");

        var sw = System.Diagnostics.Stopwatch.StartNew();

        // Process all files in parallel
        var tasks = logFiles.Select(file => ProcessLogFileAsync(file, minLevel)).ToArray();
        var results = await Task.WhenAll(tasks);

        sw.Stop();

        // Aggregate results
        var allEntries = results.SelectMany(r => r).OrderBy(e => e.Timestamp).ToList();
        int totalEntries = allEntries.Count;

        Console.WriteLine($"--- Aggregation Results ---");
        Console.WriteLine($"  Files processed: {logFiles.Length}");
        Console.WriteLine($"  Total entries (≥ {minLevel}): {totalEntries}");
        Console.WriteLine($"  Processing time: {sw.ElapsedMilliseconds}ms");

        // Summary by level
        Console.WriteLine($"\n--- Entries by Level ---");
        var byLevel = allEntries.GroupBy(e => e.Level).OrderByDescending(g => g.Count());
        foreach (var group in byLevel)
        {
            string bar = new string('█', group.Count());
            Console.WriteLine($"  {group.Key,5}: {group.Count(),5} {bar}");
        }

        // Summary by source file
        Console.WriteLine($"\n--- Entries by Source File ---");
        var byFile = allEntries.GroupBy(e => e.SourceFile).OrderByDescending(g => g.Count());
        foreach (var group in byFile)
        {
            Console.WriteLine($"  {Path.GetFileName(group.Key),-30}: {group.Count(),5} entries");
        }

        // Recent errors
        var errors = allEntries.Where(e => e.Level == "ERROR").Take(10).ToList();
        if (errors.Count > 0)
        {
            Console.WriteLine($"\n--- Recent Errors ---");
            foreach (var entry in errors)
            {
                Console.WriteLine($"  [{entry.Timestamp:HH:mm:ss}] [{entry.SourceFile}] {entry.Message}");
            }
        }

        // Time range
        if (allEntries.Count > 0)
        {
            Console.WriteLine($"\n--- Time Range ---");
            Console.WriteLine($"  First: {allEntries.First().Timestamp:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  Last:  {allEntries.Last().Timestamp:yyyy-MM-dd HH:mm:ss}");
        }
    }

    static async Task<List<LogEntry>> ProcessLogFileAsync(string filePath, string minLevel)
    {
        var entries = new List<LogEntry>();
        var levels = GetLevelThreshold(minLevel);

        try
        {
            // Async file read
            string content = await File.ReadAllTextAsync(filePath);
            string[] lines = content.Split('\n');

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                // Parse: [2024-01-15 10:30:00] [LEVEL] Message
                var match = Regex.Match(line, @"^\[([^\]]+)\]\s*\[(\w+)\]\s*(.*)");
                if (match.Success)
                {
                    string timestampStr = match.Groups[1].Value;
                    string level = match.Groups[2].Value.ToUpper();
                    string message = match.Groups[3].Value;

                    if (DateTime.TryParse(timestampStr, out DateTime timestamp)
                        && levels.Contains(level))
                    {
                        entries.Add(new LogEntry
                        {
                            Timestamp = timestamp,
                            Level = level,
                            Message = message,
                            SourceFile = filePath
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Warning: Error reading {Path.GetFileName(filePath)}: {ex.Message}");
        }

        return entries;
    }

    static HashSet<string> GetLevelThreshold(string minLevel)
    {
        var allLevels = new[] { "DEBUG", "INFO", "WARN", "WARNING", "ERROR", "FATAL", "CRITICAL" };
        int thresholdIndex = Array.FindIndex(allLevels, l => l.StartsWith(minLevel) || l == minLevel);
        if (thresholdIndex < 0) thresholdIndex = 0;
        return new HashSet<string>(allLevels.Skip(thresholdIndex));
    }
}

class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
    public string SourceFile { get; set; }
}
