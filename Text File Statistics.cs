// Program: Text File Statistics
// Description: Counts lines, words, characters, and paragraphs in a text file

using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Text File Statistics ===");
        Console.Write("Enter the path to a text file: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("Error: File path cannot be empty.");
            return;
        }

        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File not found: {filePath}");
                return;
            }

            string content = File.ReadAllText(filePath);
            string[] lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] allWords = content.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            int lineCount = content.Split(new[] { '\r', '\n' }, StringSplitOptions.None).Length;
            int wordCount = allWords.Length;
            int charCount = content.Length;
            int nonEmptyLines = lines.Length;
            int sentenceCount = CountSentences(content);
            int paragraphCount = CountParagraphs(content);

            Console.WriteLine("\n--- File Statistics ---");
            Console.WriteLine($"  File: {Path.GetFileName(filePath)}");
            Console.WriteLine($"  Characters: {charCount:N0}");
            Console.WriteLine($"  Words: {wordCount:N0}");
            Console.WriteLine($"  Lines (total): {lineCount:N0}");
            Console.WriteLine($"  Lines (non-empty): {nonEmptyLines:N0}");
            Console.WriteLine($"  Sentences: {sentenceCount:N0}");
            Console.WriteLine($"  Paragraphs: {paragraphCount:N0}");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Error: Access denied. Check file permissions.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static int CountSentences(string text)
    {
        int count = 0;
        foreach (char c in text)
        {
            if (c == '.' || c == '!' || c == '?')
            {
                count++;
            }
        }
        return count;
    }

    static int CountParagraphs(string text)
    {
        string[] parts = text.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length;
    }
}
