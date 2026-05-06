// Program: JSON Formatter
// Description: Pretty-prints minified JSON with proper indentation and line breaks

using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== JSON Formatter ===");
        Console.WriteLine("Enter minified JSON (type 'END' on a new line to finish):");

        var lines = new List<string>();
        while (true)
        {
            string line = Console.ReadLine();
            if (line == "END") break;
            lines.Add(line);
        }

        string json = string.Join(" ", lines).Trim();

        if (string.IsNullOrWhiteSpace(json))
        {
            Console.WriteLine("Error: Empty input.");
            return;
        }

        try
        {
            string formatted = FormatJson(json);
            Console.WriteLine("\n--- Formatted JSON ---");
            Console.WriteLine(formatted);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static string FormatJson(string json)
    {
        var sb = new StringBuilder();
        int indent = 0;
        bool inString = false;
        bool escape = false;

        for (int i = 0; i < json.Length; i++)
        {
            char c = json[i];

            if (escape)
            {
                sb.Append(c);
                escape = false;
                continue;
            }

            if (c == '\\')
            {
                if (inString) escape = true;
                sb.Append(c);
                continue;
            }

            if (c == '"')
            {
                inString = !inString;
                sb.Append(c);
                continue;
            }

            if (inString)
            {
                sb.Append(c);
                continue;
            }

            switch (c)
            {
                case '{':
                case '[':
                    sb.Append(c);
                    sb.AppendLine();
                    indent++;
                    sb.Append(new string(' ', indent * 4));
                    break;
                case '}':
                case ']':
                    sb.AppendLine();
                    indent--;
                    sb.Append(new string(' ', indent * 4));
                    sb.Append(c);
                    break;
                case ',':
                    sb.Append(c);
                    sb.AppendLine();
                    sb.Append(new string(' ', indent * 4));
                    break;
                case ':':
                    sb.Append(": ");
                    break;
                case ' ':
                case '\n':
                case '\r':
                case '\t':
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        return sb.ToString();
    }
}
