// Program: Async File Processor
// Description: Demonstrates async/await file operations with progress reporting

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("=== Async File Processor ===\n");

        Console.WriteLine("1. Write large file asynchronously");
        Console.WriteLine("2. Read and analyze file asynchronously");
        Console.WriteLine("3. Copy file with progress");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        string tempDir = Path.Combine(Path.GetTempPath(), "AsyncFileDemo");
        Directory.CreateDirectory(tempDir);

        switch (choice)
        {
            case "1": await WriteLargeFileAsync(tempDir); break;
            case "2": await ReadAndAnalyzeAsync(tempDir); break;
            case "3": await CopyWithProgressAsync(tempDir); break;
            default: Console.WriteLine("Invalid option."); break;
        }
    }

    static async Task WriteLargeFileAsync(string dir)
    {
        Console.Write("Enter file size in MB (1-100): ");
        if (!int.TryParse(Console.ReadLine(), out int sizeMB) || sizeMB < 1 || sizeMB > 100)
        {
            Console.WriteLine("Invalid size.");
            return;
        }

        string filePath = Path.Combine(dir, "large_file.txt");
        var sw = System.Diagnostics.Stopwatch.StartNew();

        Console.WriteLine($"\nWriting {sizeMB} MB file asynchronously...");

        using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None,
            bufferSize: 4096, useAsync: true);

        byte[] buffer = new byte[1024 * 1024]; // 1 MB buffer
        new Random(42).NextBytes(buffer);

        for (int i = 0; i < sizeMB; i++)
        {
            await stream.WriteAsync(buffer, 0, buffer.Length);
            double progress = (double)(i + 1) / sizeMB * 100;
            Console.Write($"\r  Progress: {progress:F0}%");
        }

        sw.Stop();
        Console.WriteLine($"\n  Written: {filePath}");
        Console.WriteLine($"  Size: {sizeMB} MB");
        Console.WriteLine($"  Time: {sw.ElapsedMilliseconds}ms");
    }

    static async Task ReadAndAnalyzeAsync(string dir)
    {
        // Find the large file or create a text sample
        string filePath = Path.Combine(dir, "large_file.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Large file not found. Creating sample text file...");
            filePath = Path.Combine(dir, "sample.txt");

            using var writer = new StreamWriter(filePath);
            var rng = new Random(42);
            string[] words = { "the", "quick", "brown", "fox", "jumps", "over", "lazy", "dog", "hello", "world" };

            for (int i = 0; i < 100000; i++)
            {
                int wordCount = rng.Next(5, 15);
                var line = new StringBuilder();
                for (int j = 0; j < wordCount; j++)
                {
                    if (j > 0) line.Append(' ');
                    line.Append(words[rng.Next(words.Length)]);
                }
                await writer.WriteLineAsync(line.ToString());
            }
        }

        var sw = System.Diagnostics.Stopwatch.StartNew();
        Console.WriteLine($"\nReading and analyzing: {Path.GetFileName(filePath)}...");

        string content = await File.ReadAllTextAsync(filePath);

        // Analysis
        string[] lines = content.Split('\n');
        string[] allWords = content.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        var wordFreq = allWords
            .GroupBy(w => w.ToLower())
            .OrderByDescending(g => g.Count())
            .Take(10)
            .ToList();

        sw.Stop();

        Console.WriteLine($"\n--- Analysis Results ---");
        Console.WriteLine($"  File size: {new FileInfo(filePath).Length:N0} bytes");
        Console.WriteLine($"  Lines: {lines.Length:N0}");
        Console.WriteLine($"  Words: {allWords.Length:N0}");
        Console.WriteLine($"  Characters: {content.Length:N0}");
        Console.WriteLine($"  Read time: {sw.ElapsedMilliseconds}ms");

        Console.WriteLine($"\n--- Top 10 Words ---");
        foreach (var (word, count) in wordFreq.Select(g => (g.Key, g.Count())))
        {
            string bar = new string('█', count / 100);
            Console.WriteLine($"  {word,-10}: {count,6:N0} {bar}");
        }
    }

    static async Task CopyWithProgressAsync(string dir)
    {
        Console.Write("Enter source file path: ");
        string source = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(source) || !File.Exists(source))
        {
            Console.WriteLine("Source file not found.");
            return;
        }

        string dest = Path.Combine(dir, Path.GetFileName(source));
        var sw = System.Diagnostics.Stopwatch.StartNew();

        Console.WriteLine($"\nCopying with progress...");

        using var sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
        using var destStream = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);

        byte[] buffer = new byte[81920]; // 80 KB
        long totalRead = 0;
        int bytesRead;

        while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await destStream.WriteAsync(buffer, 0, bytesRead);
            totalRead += bytesRead;
            double progress = (double)totalRead / sourceStream.Length * 100;
            Console.Write($"\r  Progress: {progress:F1}% ({totalRead / 1024:N0} KB / {sourceStream.Length / 1024:N0} KB)");
        }

        sw.Stop();
        Console.WriteLine($"\n  Copied to: {dest}");
        Console.WriteLine($"  Time: {sw.ElapsedMilliseconds}ms");
    }
}
