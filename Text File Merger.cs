// Program: Text File Merger
// Description: Merges multiple text files into one with optional headers and separators

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Text File Merger ===");
        Console.WriteLine("Enter file paths to merge (one per line, type 'done' to finish):");

        var filePaths = new List<string>();
        while (true)
        {
            Console.Write($"  File {filePaths.Count + 1}: ");
            string path = Console.ReadLine();

            if (path.Equals("done", StringComparison.OrdinalIgnoreCase))
                break;

            if (string.IsNullOrWhiteSpace(path))
                continue;

            if (!File.Exists(path))
            {
                Console.WriteLine($"    Warning: File not found, skipping.");
                continue;
            }

            filePaths.Add(path);
        }

        if (filePaths.Count == 0)
        {
            Console.WriteLine("Error: No files to merge.");
            return;
        }

        Console.Write("Add file headers? (y/n): ");
        bool addHeaders = Console.ReadLine().Trim().ToLower() == "y";

        Console.Write("Add separator lines? (y/n): ");
        bool addSeparators = Console.ReadLine().Trim().ToLower() == "y";

        Console.Write("Remove duplicate lines? (y/n): ");
        bool removeDuplicates = Console.ReadLine().Trim().ToLower() == "y";

        Console.Write("Enter output file path (or press Enter for console output): ");
        string outputPath = Console.ReadLine();

        var allLines = new List<string>();
        int totalLinesAdded = 0;

        foreach (string filePath in filePaths)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                string fileName = Path.GetFileName(filePath);

                if (addHeaders)
                {
                    allLines.Add("");
                    allLines.Add(new string('=', 60));
                    allLines.Add($"  File: {fileName}");
                    allLines.Add($"  Lines: {lines.Length}");
                    allLines.Add(new string('=', 60));
                    allLines.Add("");
                }

                if (addSeparators && allLines.Count > 0 && allLines[allLines.Count - 1] != "")
                    allLines.Add("");

                foreach (string line in lines)
                {
                    if (removeDuplicates && allLines.Contains(line))
                        continue;
                    allLines.Add(line);
                    totalLinesAdded++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error reading {Path.GetFileName(filePath)}: {ex.Message}");
            }
        }

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            Console.WriteLine($"\n--- Merged Output ({totalLinesAdded} lines) ---");
            foreach (string line in allLines)
                Console.WriteLine(line);
        }
        else
        {
            File.WriteAllLines(outputPath, allLines);
            Console.WriteLine($"\n--- Merge Complete ---");
            Console.WriteLine($"  Files merged: {filePaths.Count}");
            Console.WriteLine($"  Total lines: {totalLinesAdded}");
            Console.WriteLine($"  Output: {outputPath}");
        }
    }
}
