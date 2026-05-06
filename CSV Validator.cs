// Program: CSV Validator
// Description: Validates CSV structure including consistent column count and proper formatting

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== CSV Validator ===");
        Console.Write("Enter CSV file path: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                Console.WriteLine("Result: Empty file.");
                return;
            }

            var errors = new List<string>();
            int headerCount = SplitCsvLine(lines[0]).Length;

            // Check header
            Console.WriteLine($"Header columns: {headerCount}");
            Console.WriteLine($"Header: {lines[0]}");
            Console.WriteLine($"\nData rows: {lines.Length - 1}");

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                int fieldCount = SplitCsvLine(lines[i]).Length;
                if (fieldCount != headerCount)
                {
                    errors.Add($"Row {i + 1}: Expected {headerCount} columns, found {fieldCount}");
                }
            }

            // Check for empty fields
            int emptyCount = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = SplitCsvLine(lines[i]);
                foreach (string field in fields)
                {
                    if (string.IsNullOrWhiteSpace(field))
                        emptyCount++;
                }
            }

            Console.WriteLine($"\n--- Validation Result ---");
            if (errors.Count == 0)
            {
                Console.WriteLine("  Status: VALID");
            }
            else
            {
                Console.WriteLine("  Status: INVALID");
                Console.WriteLine($"  Errors: {errors.Count}");
                foreach (string err in errors)
                {
                    Console.WriteLine($"    - {err}");
                }
            }
            Console.WriteLine($"  Empty fields: {emptyCount}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static string[] SplitCsvLine(string line)
    {
        var fields = new List<string>();
        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(current.ToString().Trim());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        fields.Add(current.ToString().Trim());
        return fields.ToArray();
    }
}
