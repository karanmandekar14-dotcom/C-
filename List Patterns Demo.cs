// Program: List Patterns Demo
// Description: Demonstrates C# 11 list pattern matching for arrays and collections

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== List Pattern Matching (C# 11) ===\n");

        // Basic list patterns
        int[][] arrays =
        {
            Array.Empty<int>(),
            new[] { 1 },
            new[] { 1, 2 },
            new[] { 1, 2, 3 },
            new[] { 1, 2, 3, 4, 5 },
            new[] { 10, 20, 30, 40, 50 },
            new[] { -1, 0, 1 }
        };

        Console.WriteLine("--- Array Pattern Matching ---");
        foreach (var arr in arrays)
        {
            string description = arr switch
            {
                [] => "Empty array",
                [var single] => $"Single element: {single}",
                [var first, var second] => $"Two elements: {first}, {second}",
                [1, 2, 3] => "Exact match: [1, 2, 3]",
                [var f, .., var l] => $"Starts with {f}, ends with {l} (length: {arr.Length})",
                _ => "Unknown pattern"
            };
            Console.WriteLine($"  [{string.Join(", ", arr)}] -> {description}");
        }

        // Slice patterns
        Console.WriteLine($"\n--- Slice Patterns ---");
        int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        Console.WriteLine($"  Full:    [{string.Join(", ", numbers)}]");
        Console.WriteLine($"  [..3]:   First 3: [{string.Join(", ", numbers[..3])}]");
        Console.WriteLine($"  [^3..]:  Last 3: [{string.Join(", ", numbers[^3..])}]");
        Console.WriteLine($"  [2..5]:  Index 2-4: [{string.Join(", ", numbers[2..5])}]");
        Console.WriteLine($"  [..^2]:  All but last 2: [{string.Join(", ", numbers[..^2])}]");
        Console.WriteLine($"  [1..^1]: All but first and last: [{string.Join(", ", numbers[1..^1])}]");

        // Pattern matching with conditions
        Console.WriteLine($"\n--- Patterns with Guards ---");
        int[][] datasets =
        {
            [1, 2, 3],
            [1, 2, 3, 4, 5],
            [0, 0, 0],
            [-5, 0, 5],
            [100, 200]
        };

        foreach (var data in datasets)
        {
            string classification = data switch
            {
                [var x, var y, var z] when x == 0 && y == 0 && z == 0 => "Zero vector",
                [var x, var y, var z] when x + z == 2 * y => "Arithmetic sequence",
                [var f, .., var l] when f < l => $"Increasing trend ({f} → {l})",
                [var f, .., var l] when f > l => $"Decreasing trend ({f} → {l})",
                [var f, .., var l] => $"Flat trend ({f} → {l})",
                _ => "Unclassified"
            };
            Console.WriteLine($"  [{string.Join(", ", data)}] -> {classification}");
        }

        // Practical example: Command parser
        Console.WriteLine($"\n--- Command Parser ---");
        string[][] commands =
        {
            ["open", "file.txt"],
            ["save"],
            ["delete", "file.txt", "--force"],
            ["help"],
            ["move", "a.txt", "b.txt", "--overwrite"]
        };

        foreach (var cmd in commands)
        {
            string action = cmd switch
            {
                ["open", var file] => $"Open file: {file}",
                ["save"] => "Save current file",
                ["delete", var file, "--force"] => $"Force delete: {file}",
                ["delete", var file] => $"Delete (prompt): {file}",
                ["help"] => "Show help",
                ["move", var from, var to] => $"Move {from} → {to}",
                ["move", var from, var to, .. var options] => $"Move {from} → {to} with options: {string.Join(", ", options)}",
                [var command, ..] => $"Unknown command: {command}",
                [] => "Empty command"
            };
            Console.WriteLine($"  [{string.Join(", ", cmd.Select(c => $"\"{c}\""))}] -> {action}");
        }

        // String as character array pattern
        Console.WriteLine($"\n--- String Pattern Matching ---");
        string[] words = { "hi", "hello", "hey", "goodbye", "bye" };
        foreach (string word in words)
        {
            char[] chars = word.ToCharArray();
            string pattern = chars switch
            {
                ['h', 'i'] => "Short greeting",
                ['h', .., 'o'] => "Greeting ending with 'o'",
                ['g', .., 'e'] => "Word starting with 'g', ending with 'e'",
                [_, 'y'] => "Two-letter word ending with 'y'",
                _ => "Other"
            };
            Console.WriteLine($"  \"{word}\" -> {pattern}");
        }
    }
}
