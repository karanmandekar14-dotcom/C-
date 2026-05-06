// Program: Markdown Table Generator
// Description: Generates formatted markdown tables from user-provided data

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Markdown Table Generator ===");

        Console.Write("Enter number of columns: ");
        if (!int.TryParse(Console.ReadLine(), out int colCount) || colCount <= 0)
        {
            Console.WriteLine("Error: Invalid column count.");
            return;
        }

        var headers = new List<string>();
        for (int i = 0; i < colCount; i++)
        {
            Console.Write($"Enter header for column {i + 1}: ");
            string header = Console.ReadLine();
            headers.Add(string.IsNullOrWhiteSpace(header) ? $"Column {i + 1}" : header.Trim());
        }

        var rows = new List<List<string>>();
        Console.WriteLine("\nEnter row data (type 'done' on a new line to finish):");

        while (true)
        {
            Console.WriteLine($"\n--- Row {rows.Count + 1} ---");
            Console.Write("Values (comma-separated): ");
            string input = Console.ReadLine();

            if (input.Equals("done", StringComparison.OrdinalIgnoreCase))
                break;

            string[] values = input.Split(',');
            var row = new List<string>();
            for (int i = 0; i < colCount; i++)
            {
                row.Add(i < values.Length ? values[i].Trim() : "");
            }
            rows.Add(row);
        }

        Console.Write("\nAlignment (L=Left, R=Right, C=Center, comma-separated or 'all L'): ");
        string alignInput = Console.ReadLine().Trim().ToUpper();
        var alignments = new List<string>();
        string[] alignParts = alignInput.Split(',');

        if (alignParts.Length == 2 && alignParts[1].Trim().Length == 1)
        {
            char align = alignParts[1].Trim()[0];
            for (int i = 0; i < colCount; i++)
            {
                alignments.Add(GetAlignmentString(align));
            }
        }
        else
        {
            foreach (string part in alignParts)
            {
                char a = part.Trim().Length > 0 ? part.Trim()[0] : 'L';
                alignments.Add(GetAlignmentString(a));
            }
            while (alignments.Count < colCount)
            {
                alignments.Add(":---");
            }
        }

        string table = GenerateMarkdownTable(headers, rows, alignments);
        Console.WriteLine("\n--- Generated Markdown Table ---");
        Console.WriteLine(table);
    }

    static string GetAlignmentString(char align)
    {
        switch (align)
        {
            case 'R': return "---:";
            case 'C': return ":--:";
            default: return ":---";
        }
    }

    static string GenerateMarkdownTable(List<string> headers, List<List<string>> rows, List<string> alignments)
    {
        var lines = new List<string>();

        // Header row
        lines.Add("| " + string.Join(" | ", headers) + " |");

        // Alignment row
        lines.Add("| " + string.Join(" | ", alignments) + " |");

        // Data rows
        foreach (var row in rows)
        {
            lines.Add("| " + string.Join(" | ", row) + " |");
        }

        return string.Join("\n", lines);
    }
}
