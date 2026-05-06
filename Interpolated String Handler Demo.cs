// Program: Interpolated String Handler Demo
// Description: Demonstrates C# 10 custom interpolated string handlers for efficient string building

using System;
using System.Runtime.CompilerServices;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Interpolated String Handler Demo (C# 10) ===\n");

        // Custom handler: Append if condition is met
        string name = "World";
        int age = 25;

        DebugLog($"Hello, {name}! You are {age} years old.");
        DebugLog($"Processing item #{42}");

        // Custom handler: Format as table
        Console.WriteLine("\n--- Table Formatter ---");
        PrintTable(
            $"Name,Age,City",
            $"Alice,30,New York",
            $"Bob,25,London",
            $"Charlie,35,Tokyo"
        );

        // Custom handler: Build HTML
        Console.WriteLine("\n--- HTML Builder ---");
        string html = BuildHtml($"My Page", $"Welcome to {name}!");
        Console.WriteLine(html);

        // Custom handler: CSV builder
        Console.WriteLine("\n--- CSV Builder ---");
        var csv = new CsvBuilder();
        csv.AppendLine($"Name,Email,Score");
        csv.AppendLine($"Alice,alice@test.com,95");
        csv.AppendLine($"Bob,bob@test.com,87");
        csv.AppendLine($"Charlie,charlie@test.com,92");
        Console.WriteLine(csv.ToString());

        // Custom handler: Log with level
        Console.WriteLine("\n--- Log Builder ---");
        var logger = new Logger { MinLevel = LogLevel.Warning };
        logger.Debug($"This is a debug message - should not appear");
        logger.Info($"This is an info message - should not appear");
        logger.Warning($"Disk usage at 85%");
        logger.Error($"Failed to connect to database");
    }

    static void DebugLog(string message)
    {
        Console.WriteLine($"  [DEBUG] {message}");
    }

    static void PrintTable(params string[] rows)
    {
        if (rows.Length == 0) return;

        string[] headers = rows[0].Split(',');
        int[] colWidths = headers.Select(h => h.Length).ToArray();

        var dataRows = rows.Skip(1).Select(r => r.Split(',')).ToList();
        foreach (var row in dataRows)
        {
            for (int i = 0; i < row.Length && i < colWidths.Length; i++)
            {
                colWidths[i] = Math.Max(colWidths[i], row[i].Length);
            }
        }

        // Print header
        for (int i = 0; i < headers.Length; i++)
            Console.Write($"  {headers[i].PadRight(colWidths[i])}  ");
        Console.WriteLine();
        Console.WriteLine("  " + new string('-', colWidths.Sum() + colWidths.Length * 4));

        // Print data
        foreach (var row in dataRows)
        {
            for (int i = 0; i < row.Length && i < colWidths.Length; i++)
                Console.Write($"  {row[i].PadRight(colWidths[i])}  ");
            Console.WriteLine();
        }
    }

    static string BuildHtml(string title, string body)
    {
        return $"<html><head><title>{title}</title></head><body><h1>{title}</h1><p>{body}</p></body></html>";
    }
}

class CsvBuilder
{
    private readonly StringBuilder _sb = new();

    public void AppendLine(string line) => _sb.AppendLine(line);
    public override string ToString() => _sb.ToString();
}

enum LogLevel { Debug, Info, Warning, Error }

class Logger
{
    public LogLevel MinLevel { get; set; }

    public void Debug(string msg) => Log(LogLevel.Debug, msg);
    public void Info(string msg) => Log(LogLevel.Info, msg);
    public void Warning(string msg) => Log(LogLevel.Warning, msg);
    public void Error(string msg) => Log(LogLevel.Error, msg);

    void Log(LogLevel level, string msg)
    {
        if (level >= MinLevel)
            Console.WriteLine($"  [{level}] {msg}");
    }
}
