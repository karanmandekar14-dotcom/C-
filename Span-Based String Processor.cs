// Program: Span-Based String Processor
// Description: High-performance string processing using Span<T> and Memory<T>

using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Span-Based String Processor (C# 7.2+) ===\n");

        string text = "  Hello, World! This is a TEST string with MIXED case.  ";

        Console.WriteLine($"Original: \"{text}\"");
        Console.WriteLine($"Length: {text.Length}\n");

        // Span-based trim (no allocation)
        Span<char> buffer = stackalloc char[text.Length];
        text.AsSpan().Trim().CopyTo(buffer);
        string trimmed = buffer.Slice(0, text.AsSpan().Trim().Length).ToString();
        Console.WriteLine($"Span Trim: \"{trimmed}\"");

        // Span-based reversal
        Span<char> reversed = stackalloc char[trimmed.Length];
        trimmed.AsSpan().CopyTo(reversed);
        reversed.Reverse();
        Console.WriteLine($"Span Reverse: \"{reversed.ToString()}\"");

        // Span-based case conversion
        Span<char> upper = stackalloc char[trimmed.Length];
        trimmed.AsSpan().ToUpperInvariant(upper);
        Console.WriteLine($"Span ToUpper: \"{upper.ToString()}\"");

        // Memory<T> for heap-allocated spans
        Memory<char> memory = new Memory<char>(new char[100]);
        var memorySpan = memory.Span;
        trimmed.AsSpan().CopyTo(memorySpan);
        Console.WriteLine($"Memory: \"{memorySpan.Slice(0, trimmed.Length).ToString()}\"");

        // Span-based search (no allocation)
        ReadOnlySpan<char> span = trimmed.AsSpan();
        int worldIndex = span.IndexOf("World", StringComparison.OrdinalIgnoreCase);
        Console.WriteLine($"\nIndexOf(\"World\"): {worldIndex}");

        // Span-based split without creating array
        Console.WriteLine($"\n--- Span-Based Word Extraction ---");
        ExtractWordsSpan(span);

        // Performance comparison
        Console.WriteLine($"\n--- Performance Comparison ---");
        string largeText = GenerateLargeString(10000);

        var sw = System.Diagnostics.Stopwatch.StartNew();
        int traditionalCount = CountWordsTraditional(largeText);
        sw.Stop();
        Console.WriteLine($"  Traditional: {traditionalCount} words in {sw.ElapsedMilliseconds}ms");

        sw.Restart();
        int spanCount = CountWordsWithSpan(largeText);
        sw.Stop();
        Console.WriteLine($"  Span-based:  {spanCount} words in {sw.ElapsedMilliseconds}ms");
    }

    static void ExtractWordsSpan(ReadOnlySpan<char> text)
    {
        int wordCount = 0;
        while (!text.IsEmpty)
        {
            int spaceIndex = text.IndexOfAny(' ', '\t', '\n');
            if (spaceIndex < 0)
            {
                if (text.Length > 0)
                    Console.WriteLine($"  Word {++wordCount}: {text.ToString()}");
                break;
            }

            if (spaceIndex > 0)
                Console.WriteLine($"  Word {++wordCount}: {text.Slice(0, spaceIndex).ToString()}");

            text = text.Slice(spaceIndex + 1);
            // Skip consecutive spaces
            while (!text.IsEmpty && text[0] == ' ')
                text = text.Slice(1);
        }
        Console.WriteLine($"  Total: {wordCount} words");
    }

    static int CountWordsTraditional(string text)
    {
        string[] words = text.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }

    static int CountWordsWithSpan(string text)
    {
        ReadOnlySpan<char> span = text.AsSpan();
        int count = 0;
        bool inWord = false;

        foreach (char c in span)
        {
            if (char.IsWhiteSpace(c))
            {
                inWord = false;
            }
            else if (!inWord)
            {
                count++;
                inWord = true;
            }
        }
        return count;
    }

    static string GenerateLargeString(int wordCount)
    {
        var sb = new StringBuilder();
        string[] words = { "the", "quick", "brown", "fox", "jumps", "over", "lazy", "dog", "span", "memory" };
        var rng = new Random(42);
        for (int i = 0; i < wordCount; i++)
        {
            if (i > 0) sb.Append(' ');
            sb.Append(words[rng.Next(words.Length)]);
        }
        return sb.ToString();
    }
}
