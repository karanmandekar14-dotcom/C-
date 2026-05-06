// Program: CSV Column Extractor
// Description: Extracts specific columns from CSV data by index or header name

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== CSV Column Extractor ===");
        Console.Write("Enter the path to a CSV file: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found or path is empty.");
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                Console.WriteLine("Error: CSV file is empty.");
                return;
            }

            string[] headers = lines[0].Split(',');
            Console.WriteLine("\nAvailable columns:");
            for (int i = 0; i < headers.Length; i++)
            {
                Console.WriteLine($"  [{i + 1}] {headers[i].Trim()}");
            }

            Console.Write("\nEnter column numbers to extract (comma-separated, e.g. 1,3): ");
            string colInput = Console.ReadLine();
            string[] colParts = colInput.Split(',');
            var indices = new List<int>();

            foreach (string part in colParts)
            {
                if (int.TryParse(part.Trim(), out int col) && col >= 1 && col <= headers.Length)
                {
                    indices.Add(col - 1);
                }
            }

            if (indices.Count == 0)
            {
                Console.WriteLine("Error: No valid column indices provided.");
                return;
            }

            Console.WriteLine("\n--- Extracted Data ---");
            foreach (string line in lines)
            {
                string[] fields = line.Split(',');
                var values = new List<string>();
                foreach (int idx in indices)
                {
                    values.Add(idx < fields.Length ? fields[idx].Trim() : "");
                }
                Console.WriteLine(string.Join(" | ", values));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
