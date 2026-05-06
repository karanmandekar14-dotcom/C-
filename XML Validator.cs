// Program: XML Validator
// Description: Basic XML well-formedness check — validates tag matching and nesting

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== XML Validator ===");
        Console.WriteLine("1. Validate from file");
        Console.WriteLine("2. Validate from text input");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        string xml = "";
        switch (choice)
        {
            case "1":
                Console.Write("Enter XML file path: ");
                string path = Console.ReadLine();
                if (!File.Exists(path))
                {
                    Console.WriteLine("Error: File not found.");
                    return;
                }
                xml = File.ReadAllText(path);
                break;
            case "2":
                Console.WriteLine("Enter XML text (type 'END' on a new line to finish):");
                var lines = new List<string>();
                while (true)
                {
                    string line = Console.ReadLine();
                    if (line == "END") break;
                    lines.Add(line);
                }
                xml = string.Join("\n", lines);
                break;
            default:
                Console.WriteLine("Invalid option.");
                return;
        }

        (bool isValid, string error) = ValidateXml(xml);
        Console.WriteLine($"\n--- Validation Result ---");
        Console.WriteLine($"  Status: {(isValid ? "VALID" : "INVALID")}");
        if (!isValid)
        {
            Console.WriteLine($"  Error: {error}");
        }
    }

    static (bool isValid, string error) ValidateXml(string xml)
    {
        var stack = new Stack<string>();
        var tagBuilder = new StringBuilder();
        bool inTag = false;
        bool isClosing = false;
        bool isSelfClosing = false;
        bool inComment = false;

        for (int i = 0; i < xml.Length; i++)
        {
            char c = xml[i];

            // Handle comments
            if (i + 3 < xml.Length && xml.Substring(i, 4) == "<!--")
            {
                inComment = true;
                continue;
            }
            if (inComment)
            {
                if (i + 2 < xml.Length && xml.Substring(i, 3) == "-->")
                {
                    inComment = false;
                    i += 2;
                }
                continue;
            }

            if (c == '<')
            {
                inTag = true;
                isClosing = false;
                isSelfClosing = false;
                tagBuilder.Clear();
                continue;
            }

            if (c == '>' && inTag)
            {
                inTag = false;
                string tag = tagBuilder.ToString().Trim();

                if (tag.StartsWith("!") || tag.StartsWith("?"))
                {
                    // DOCTYPE, XML declaration, etc.
                    continue;
                }

                if (tag.EndsWith("/"))
                {
                    isSelfClosing = true;
                    tag = tag.Substring(0, tag.Length - 1).Trim();
                }

                if (tag.StartsWith("/"))
                {
                    string tagName = tag.Substring(1).Trim();
                    if (stack.Count == 0)
                        return (false, $"Unexpected closing tag '</{tagName}>' at position {i}");

                    string expected = stack.Pop();
                    if (!expected.Equals(tagName, StringComparison.OrdinalIgnoreCase))
                        return (false, $"Mismatched tag: expected '</{expected}>' but found '</{tagName}>' at position {i}");
                }
                else if (!isSelfClosing)
                {
                    // Extract tag name (before any attributes)
                    int spaceIdx = tag.IndexOf(' ');
                    string tagName = spaceIdx > 0 ? tag.Substring(0, spaceIdx) : tag;
                    stack.Push(tagName);
                }
                continue;
            }

            if (inTag)
            {
                tagBuilder.Append(c);
            }
        }

        if (stack.Count > 0)
        {
            return (false, $"Unclosed tag: '<{stack.Pop()}>'");
        }

        return (true, null);
    }
}
