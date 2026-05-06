// Program: String Case Converter
// Description: Converts text between camelCase, PascalCase, snake_case, kebab-case, and CONSTANT_CASE

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== String Case Converter ===");
        Console.Write("Enter text to convert: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Error: Input cannot be empty.");
            return;
        }

        Console.WriteLine("\n--- Conversion Results ---");
        Console.WriteLine($"  Original:       {input}");
        Console.WriteLine($"  camelCase:      {ToCamelCase(input)}");
        Console.WriteLine($"  PascalCase:     {ToPascalCase(input)}");
        Console.WriteLine($"  snake_case:     {ToSnakeCase(input)}");
        Console.WriteLine($"  kebab-case:     {ToKebabCase(input)}");
        Console.WriteLine($"  CONSTANT_CASE:  {ToConstantCase(input)}");
        Console.WriteLine($"  Sentence case:  {ToSentenceCase(input)}");
        Console.WriteLine($"  Title Case:     {ToTitleCase(input)}");
        Console.WriteLine($"  lowercase:      {input.ToLower()}");
        Console.WriteLine($"  UPPERCASE:      {input.ToUpper()}");
        Console.WriteLine($"  iNVERTED cASE:  {InvertCase(input)}");

        Console.WriteLine("\nChoose a format to copy (type name, or 'exit'): ");
        string choice = Console.ReadLine();

        var result = choice.ToLower() switch
        {
            "camelcase" => ToCamelCase(input),
            "pascalcase" => ToPascalCase(input),
            "snake_case" => ToSnakeCase(input),
            "kebab-case" => ToKebabCase(input),
            "constant_case" => ToConstantCase(input),
            "sentence case" => ToSentenceCase(input),
            "title case" => ToTitleCase(input),
            "lowercase" => input.ToLower(),
            "uppercase" => input.ToUpper(),
            _ => null
        };

        if (result != null)
        {
            Console.WriteLine($"\nSelected: {result}");
            Console.WriteLine("(Copied to clipboard mentally — paste manually)");
        }
    }

    static string ToWords(string input)
    {
        // Insert space before uppercase letters
        string spaced = Regex.Replace(input, @"([a-z])([A-Z])", "$1 $2");
        // Replace separators with spaces
        spaced = Regex.Replace(spaced, @"[_\-\.]+", " ");
        // Remove extra spaces
        spaced = Regex.Replace(spaced, @"\s+", " ").Trim();
        return spaced;
    }

    static string ToCamelCase(string input)
    {
        string words = ToWords(input);
        var parts = words.Split(' ');
        var sb = new StringBuilder();
        sb.Append(parts[0].ToLower());
        for (int i = 1; i < parts.Length; i++)
        {
            sb.Append(char.ToUpper(parts[i][0]) + parts[i].Substring(1).ToLower());
        }
        return sb.ToString();
    }

    static string ToPascalCase(string input)
    {
        string camel = ToCamelCase(input);
        return char.ToUpper(camel[0]) + camel.Substring(1);
    }

    static string ToSnakeCase(string input)
    {
        return ToWords(input).Replace(' ', '_').ToLower();
    }

    static string ToKebabCase(string input)
    {
        return ToWords(input).Replace(' ', '-').ToLower();
    }

    static string ToConstantCase(string input)
    {
        return ToWords(input).Replace(' ', '_').ToUpper();
    }

    static string ToSentenceCase(string input)
    {
        string words = ToWords(input).ToLower();
        return char.ToUpper(words[0]) + words.Substring(1);
    }

    static string ToTitleCase(string input)
    {
        string words = ToWords(input).ToLower();
        var parts = words.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].Length > 0)
                parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
        }
        return string.Join(" ", parts);
    }

    static string InvertCase(string input)
    {
        var sb = new StringBuilder();
        foreach (char c in input)
        {
            sb.Append(char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c));
        }
        return sb.ToString();
    }
}
