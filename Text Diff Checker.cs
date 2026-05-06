// Program: Text Diff Checker
// Description: Compares two text inputs or files and highlights differences

using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Text Diff Checker ===");
        Console.WriteLine("1. Compare two text strings");
        Console.WriteLine("2. Compare two text files");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                CompareStrings();
                break;
            case "2":
                CompareFiles();
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }

    static void CompareStrings()
    {
        Console.WriteLine("\n--- Enter first text ---");
        string text1 = Console.ReadLine();
        Console.WriteLine("\n--- Enter second text ---");
        string text2 = Console.ReadLine();

        string[] lines1 = text1.Split('\n');
        string[] lines2 = text2.Split('\n');

        PrintDiff(lines1, lines2);
    }

    static void CompareFiles()
    {
        Console.Write("Enter first file path: ");
        string path1 = Console.ReadLine();
        Console.Write("Enter second file path: ");
        string path2 = Console.ReadLine();

        if (!File.Exists(path1))
        {
            Console.WriteLine($"Error: File not found: {path1}");
            return;
        }
        if (!File.Exists(path2))
        {
            Console.WriteLine($"Error: File not found: {path2}");
            return;
        }

        try
        {
            string[] lines1 = File.ReadAllLines(path1);
            string[] lines2 = File.ReadAllLines(path2);

            PrintDiff(lines1, lines2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void PrintDiff(string[] lines1, string[] lines2)
    {
        int maxLen = Math.Max(lines1.Length, lines2.Length);
        int added = 0, removed = 0, unchanged = 0;

        Console.WriteLine("\n--- Diff Output ---");
        for (int i = 0; i < maxLen; i++)
        {
            bool has1 = i < lines1.Length;
            bool has2 = i < lines2.Length;

            if (has1 && has2 && lines1[i] == lines2[i])
            {
                Console.WriteLine($"  {i + 1,4}: {lines1[i]}");
                unchanged++;
            }
            else if (has1 && !has2)
            {
                Console.WriteLine($"- {i + 1,4}: {lines1[i]}");
                removed++;
            }
            else if (!has1 && has2)
            {
                Console.WriteLine($"+ {i + 1,4}: {lines2[i]}");
                added++;
            }
            else if (lines1[i] != lines2[i])
            {
                Console.WriteLine($"- {i + 1,4}: {lines1[i]}");
                Console.WriteLine($"+ {i + 1,4}: {lines2[i]}");
                removed++;
                added++;
            }
        }

        Console.WriteLine($"\n--- Summary ---");
        Console.WriteLine($"  Unchanged: {unchanged}");
        Console.WriteLine($"  Added:     {added}");
        Console.WriteLine($"  Removed:   {removed}");
        Console.WriteLine($"  Similarity: {maxLen > 0 ? (double)unchanged / maxLen * 100 : 100:F1}%");
    }
}
