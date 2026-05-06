// Program: CSV to JSON Converter
// Description: Converts CSV data to JSON format with type detection

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== CSV to JSON Converter ===");
        Console.WriteLine("1. Convert from file");
        Console.WriteLine("2. Convert from clipboard/text input");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        string csv = "";
        switch (choice)
        {
            case "1":
                Console.Write("Enter CSV file path: ");
                string path = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    Console.WriteLine("Error: File not found.");
                    return;
                }
                csv = File.ReadAllText(path);
                break;
            case "2":
                Console.WriteLine("Enter CSV data (type 'END' on a new line to finish):");
                var lines = new List<string>();
                while (true)
                {
                    string line = Console.ReadLine();
                    if (line == "END") break;
                    lines.Add(line);
                }
                csv = string.Join("\n", lines);
                break;
            default:
                Console.WriteLine("Invalid option.");
                return;
        }

        try
        {
            string json = ConvertCsvToJson(csv);
            Console.WriteLine("\n--- JSON Output ---");
            Console.WriteLine(json);

            Console.Write("\nSave to file? (y/n): ");
            if (Console.ReadLine().Trim().ToLower() == "y")
            {
                Console.Write("Enter output file path: ");
                string outputPath = Console.ReadLine();
                File.WriteAllText(outputPath, json);
                Console.WriteLine($"Saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static string ConvertCsvToJson(string csv)
    {
        var lines = new List<string>(csv.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
        if (lines.Count < 2)
            return "[]";

        string[] headers = ParseCsvLine(lines[0]);
        var records = new List<Dictionary<string, object>>();

        for (int i = 1; i < lines.Count; i++)
        {
            string[] fields = ParseCsvLine(lines[i]);
            var record = new Dictionary<string, object>();

            for (int j = 0; j < headers.Length; j++)
            {
                string header = headers[j].Trim();
                string value = j < fields.Length ? fields[j].Trim() : "";

                record[header] = DetectType(value);
            }
            records.Add(record);
        }

        return BuildJson(records, headers);
    }

    static string[] ParseCsvLine(string line)
    {
        var fields = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        fields.Add(current.ToString());
        return fields.ToArray();
    }

    static object DetectType(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        if (int.TryParse(value, out int intVal))
            return intVal;

        if (double.TryParse(value, out double doubleVal))
            return doubleVal;

        if (bool.TryParse(value, out bool boolVal))
            return boolVal;

        return value;
    }

    static string BuildJson(List<Dictionary<string, object>> records, string[] headers)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[");

        for (int i = 0; i < records.Count; i++)
        {
            sb.AppendLine("  {");
            var record = records[i];
            var keys = new List<string>(record.Keys);

            for (int j = 0; j < keys.Count; j++)
            {
                string key = EscapeJson(keys[j]);
                string val = FormatJsonValue(record[keys[j]]);
                string comma = j < keys.Count - 1 ? "," : "";
                sb.AppendLine($"    \"{key}\": {val}{comma}");
            }

            string recordComma = i < records.Count - 1 ? "," : "";
            sb.AppendLine($"  }}{recordComma}");
        }

        sb.AppendLine("]");
        return sb.ToString();
    }

    static string EscapeJson(string value)
    {
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\t", "\\t");
    }

    static string FormatJsonValue(object value)
    {
        if (value is int || value is double || value is bool)
            return value.ToString().ToLower();

        return $"\"{EscapeJson(value.ToString())}\"";
    }
}
