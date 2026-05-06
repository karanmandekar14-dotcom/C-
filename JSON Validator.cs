// Program: JSON Validator
// Description: Validates JSON syntax by manually parsing brackets, quotes, and structure

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== JSON Validator ===");
        Console.WriteLine("Enter JSON text (type 'END' on a new line to finish):");

        var lines = new List<string>();
        while (true)
        {
            string line = Console.ReadLine();
            if (line == "END") break;
            lines.Add(line);
        }

        string json = string.Join("\n", lines);

        if (string.IsNullOrWhiteSpace(json))
        {
            Console.WriteLine("Error: Empty input.");
            return;
        }

        (bool isValid, string error) = ValidateJson(json);
        if (isValid)
        {
            Console.WriteLine("\nResult: Valid JSON!");
        }
        else
        {
            Console.WriteLine($"\nResult: Invalid JSON");
            Console.WriteLine($"Error: {error}");
        }
    }

    static (bool isValid, string error) ValidateJson(string json)
    {
        string trimmed = json.Trim();
        if (trimmed.Length == 0)
            return (false, "Empty input");

        var stack = new Stack<char>();
        bool inString = false;
        bool escape = false;

        for (int i = 0; i < trimmed.Length; i++)
        {
            char c = trimmed[i];

            if (escape)
            {
                escape = false;
                continue;
            }

            if (c == '\\')
            {
                if (inString) escape = true;
                continue;
            }

            if (c == '"')
            {
                inString = !inString;
                continue;
            }

            if (inString)
                continue;

            switch (c)
            {
                case '{':
                case '[':
                    stack.Push(c);
                    break;
                case '}':
                    if (stack.Count == 0 || stack.Pop() != '{')
                        return (false, $"Unexpected '}}' at position {i}");
                    break;
                case ']':
                    if (stack.Count == 0 || stack.Pop() != '[')
                        return (false, $"Unexpected ']' at position {i}");
                    break;
            }
        }

        if (inString)
            return (false, "Unterminated string");

        if (stack.Count > 0)
            return (false, $"Unclosed bracket: '{stack.Pop()}'");

        return (true, null);
    }
}
