// Program: Log File Parser
// Description: Parses structured log files and filters by log level (INFO, WARN, ERROR)

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Log File Parser ===");
        Console.Write("Enter the path to a log file: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found or path is empty.");
            return;
        }

        Console.WriteLine("\nFilter by level (INFO, WARN, ERROR, ALL): ");
        string levelFilter = Console.ReadLine().Trim().ToUpper();

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            var parsedEntries = new List<LogEntry>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                LogEntry entry = ParseLogLine(line);
                if (entry != null)
                {
                    parsedEntries.Add(entry);
                }
            }

            var filtered = levelFilter == "ALL"
                ? parsedEntries
                : parsedEntries.FindAll(e => e.Level.Equals(levelFilter));

            Console.WriteLine($"\n--- Found {filtered.Count} matching entries ---");
            foreach (LogEntry entry in filtered)
            {
                Console.WriteLine($"  [{entry.Timestamp}] [{entry.Level}] {entry.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static LogEntry ParseLogLine(string line)
    {
        // Expected format: [2024-01-15 10:30:00] [INFO] Message here
        try
        {
            int firstClose = line.IndexOf(']');
            if (firstClose < 0) return null;

            string timestamp = line.Substring(1, firstClose - 1).Trim();

            int secondOpen = line.IndexOf('[', firstClose);
            int secondClose = line.IndexOf(']', secondOpen);
            if (secondOpen < 0 || secondClose < 0) return null;

            string level = line.Substring(secondOpen + 1, secondClose - secondOpen - 1).Trim();
            string message = line.Substring(secondClose + 1).Trim();

            return new LogEntry { Timestamp = timestamp, Level = level.ToUpper(), Message = message };
        }
        catch
        {
            return null;
        }
    }
}

class LogEntry
{
    public string Timestamp { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
}
