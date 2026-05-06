// Program: ASCII Art Generator
// Description: Converts text input into ASCII art banners using block letters

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ASCII Art Generator ===");
        Console.Write("Enter text to convert: ");
        string text = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Error: Text cannot be empty.");
            return;
        }

        string art = GenerateAsciiArt(text.ToUpper());
        Console.WriteLine("\n--- ASCII Art ---");
        Console.WriteLine(art);
    }

    static string GenerateAsciiArt(string text)
    {
        var letters = new Dictionary<char, string[]>
        {
            { 'A', new[] { "  █  ", " █ █ ", "█████", "█   █", "█   █" } },
            { 'B', new[] { "████ ", "█   █", "████ ", "█   █", "████ " } },
            { 'C', new[] { " ████", "█    ", "█    ", "█    ", " ████" } },
            { 'D', new[] { "████ ", "█   █", "█   █", "█   █", "████ " } },
            { 'E', new[] { "█████", "█    ", "████ ", "█    ", "█████" } },
            { 'F', new[] { "█████", "█    ", "████ ", "█    ", "█    " } },
            { 'G', new[] { " ████", "█    ", "█  ██", "█   █", " ████" } },
            { 'H', new[] { "█   █", "█   █", "█████", "█   █", "█   █" } },
            { 'I', new[] { "█████", "  █  ", "  █  ", "  █  ", "█████" } },
            { 'J', new[] { "█████", "   █ ", "   █ ", "█  █ ", " ██  " } },
            { 'K', new[] { "█   █", "█  █ ", "███  ", "█  █ ", "█   █" } },
            { 'L', new[] { "█    ", "█    ", "█    ", "█    ", "█████" } },
            { 'M', new[] { "█   █", "██ ██", "█ █ █", "█   █", "█   █" } },
            { 'N', new[] { "█   █", "██  █", "█ █ █", "█  ██", "█   █" } },
            { 'O', new[] { " ███ ", "█   █", "█   █", "█   █", " ███ " } },
            { 'P', new[] { "████ ", "█   █", "████ ", "█    ", "█    " } },
            { 'Q', new[] { " ███ ", "█   █", "█ █ █", "█  █ ", " ██ █" } },
            { 'R', new[] { "████ ", "█   █", "████ ", "█  █ ", "█   █" } },
            { 'S', new[] { " ████", "█    ", " ███ ", "    █", "████ " } },
            { 'T', new[] { "█████", "  █  ", "  █  ", "  █  ", "  █  " } },
            { 'U', new[] { "█   █", "█   █", "█   █", "█   █", " ███ " } },
            { 'V', new[] { "█   █", "█   █", "█   █", " █ █ ", "  █  " } },
            { 'W', new[] { "█   █", "█   █", "█ █ █", "██ ██", "█   █" } },
            { 'X', new[] { "█   █", " █ █ ", "  █  ", " █ █ ", "█   █" } },
            { 'Y', new[] { "█   █", " █ █ ", "  █  ", "  █  ", "  █  " } },
            { 'Z', new[] { "█████", "   █ ", "  █  ", " █   ", "█████" } },
            { ' ', new[] { "     ", "     ", "     ", "     ", "     " } },
        };

        int height = 5;
        var result = new List<string>();

        for (int row = 0; row < height; row++)
        {
            string line = "";
            foreach (char c in text)
            {
                if (letters.ContainsKey(c))
                {
                    line += letters[c][row] + "  ";
                }
                else
                {
                    line += "     " + "  ";
                }
            }
            result.Add(line.TrimEnd());
        }

        return string.Join("\n", result);
    }
}
