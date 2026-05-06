// Program: HTML Tag Stripper
// Description: Removes HTML tags from text while preserving content and formatting

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== HTML Tag Stripper ===");
        Console.WriteLine("1. Strip HTML from text");
        Console.WriteLine("2. Extract links from HTML");
        Console.WriteLine("3. Extract images from HTML");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        Console.WriteLine("\nEnter HTML text (type 'END' on a new line to finish):");
        var lines = new List<string>();
        while (true)
        {
            string line = Console.ReadLine();
            if (line == "END") break;
            lines.Add(line);
        }

        string html = string.Join("\n", lines);

        switch (choice)
        {
            case "1":
                StripAndDisplay(html);
                break;
            case "2":
                ExtractLinks(html);
                break;
            case "3":
                ExtractImages(html);
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }

    static void StripAndDisplay(string html)
    {
        string text = StripHtmlTags(html);
        Console.WriteLine("\n--- Plain Text ---");
        Console.WriteLine(text);
        Console.WriteLine($"\nOriginal length: {html.Length}");
        Console.WriteLine($"Stripped length: {text.Length}");
    }

    static void ExtractLinks(string html)
    {
        var links = new List<(string text, string url)>();
        var regex = new Regex(@"<a\s+(?:[^>]*?\s+)?href=""([^""]*)""[^>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        foreach (Match match in regex.Matches(html))
        {
            string url = match.Groups[1].Value;
            string text = StripHtmlTags(match.Groups[2].Value).Trim();
            links.Add((text, url));
        }

        Console.WriteLine($"\n--- Found {links.Count} Links ---");
        for (int i = 0; i < links.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {links[i].text}");
            Console.WriteLine($"     URL: {links[i].url}");
        }
    }

    static void ExtractImages(string html)
    {
        var images = new List<(string src, string alt)>();
        var regex = new Regex(@"<img\s+(?:[^>]*?\s+)?src=""([^""]*)""[^>]*(?:alt=""([^""]*)"")?", RegexOptions.IgnoreCase);

        foreach (Match match in regex.Matches(html))
        {
            images.Add((match.Groups[1].Value, match.Groups[2].Value));
        }

        Console.WriteLine($"\n--- Found {images.Count} Images ---");
        for (int i = 0; i < images.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {images[i].src}");
            if (!string.IsNullOrEmpty(images[i].alt))
                Console.WriteLine($"     Alt: {images[i].alt}");
        }
    }

    static string StripHtmlTags(string html)
    {
        // Replace common HTML entities
        html = html.Replace("&nbsp;", " ")
                   .Replace("&amp;", "&")
                   .Replace("&lt;", "<")
                   .Replace("&gt;", ">")
                   .Replace("&quot;", "\"")
                   .Replace("&#39;", "'")
                   .Replace("&copy;", "(c)")
                   .Replace("&reg;", "(R)")
                   .Replace("&trade;", "(TM)");

        // Remove script and style content
        html = Regex.Replace(html, @"<script[^>]*>.*?</script>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        html = Regex.Replace(html, @"<style[^>]*>.*?</style>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Replace block elements with newlines
        html = Regex.Replace(html, @"</(?:p|div|br|tr|li|h[1-6])?>", "\n", RegexOptions.IgnoreCase);

        // Remove remaining tags
        html = Regex.Replace(html, @"<[^>]+>", "");

        // Clean up whitespace
        html = Regex.Replace(html, @"[ \t]+", " ");
        html = Regex.Replace(html, @"\n{3,}", "\n\n");

        return html.Trim();
    }
}
