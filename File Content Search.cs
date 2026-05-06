// Program: File Content Search
// Description: Searches for text patterns across all files in a directory

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== File Content Search ===");
        Console.Write("Enter directory path: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        Console.Write("Enter search text: ");
        string searchText = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            Console.WriteLine("Error: Search text cannot be empty.");
            return;
        }

        Console.Write("Case sensitive? (y/n): ");
        bool caseSensitive = Console.ReadLine().Trim().ToLower() == "y";

        Console.Write("File pattern (e.g., *.txt, *.cs, *.*): ");
        string pattern = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(pattern))
            pattern = "*.*";

        Console.Write("Search subdirectories? (y/n): ");
        bool recursive = Console.ReadLine().Trim().ToLower() == "y";

        Console.WriteLine($"\n--- Searching for '{searchText}' in {pattern} ---");

        var results = new List<SearchResult>();
        var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

        try
        {
            string[] files = Directory.GetFiles(dirPath, pattern, searchOption);
            Console.WriteLine($"Scanning {files.Length} files...");

            foreach (string file in files)
            {
                try
                {
                    string content = File.ReadAllText(file);
                    string[] lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.None);

                    var matchingLines = new List<MatchedLine>();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        bool matchFound = caseSensitive
                            ? lines[i].Contains(searchText)
                            : lines[i].Contains(searchText, StringComparison.OrdinalIgnoreCase);

                        if (matchFound)
                        {
                            matchingLines.Add(new MatchedLine
                            {
                                LineNumber = i + 1,
                                Content = lines[i].Trim()
                            });
                        }
                    }

                    if (matchingLines.Count > 0)
                    {
                        results.Add(new SearchResult
                        {
                            FilePath = file,
                            Matches = matchingLines
                        });
                    }
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return;
        }

        // Display results
        Console.WriteLine($"\n--- Found {results.Count} matching file(s) ---");

        if (results.Count == 0)
        {
            Console.WriteLine("  No matches found.");
            return;
        }

        int totalMatches = results.Sum(r => r.Matches.Count);
        Console.WriteLine($"  Total matches: {totalMatches}\n");

        foreach (var result in results)
        {
            string relativePath = result.FilePath.StartsWith(dirPath)
                ? result.FilePath.Substring(dirPath.Length + 1)
                : result.FilePath;

            Console.WriteLine($"  📄 {relativePath} ({result.Matches.Count} matches)");
            foreach (var match in result.Matches)
            {
                string preview = match.Content.Length > 80
                    ? match.Content.Substring(0, 80) + "..."
                    : match.Content;
                string highlighted = HighlightText(preview, searchText, caseSensitive);
                Console.WriteLine($"     Line {match.LineNumber,4}: {highlighted}");
            }
            Console.WriteLine();
        }
    }

    static string HighlightText(string text, string searchText, bool caseSensitive)
    {
        int index = caseSensitive
            ? text.IndexOf(searchText)
            : text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);

        if (index < 0) return text;

        string before = text.Substring(0, index);
        string match = text.Substring(index, searchText.Length);
        string after = text.Substring(index + searchText.Length);

        return $"{before}>>[{match}]<<{after}";
    }
}

class SearchResult
{
    public string FilePath { get; set; }
    public List<MatchedLine> Matches { get; set; }
}

class MatchedLine
{
    public int LineNumber { get; set; }
    public string Content { get; set; }
}
