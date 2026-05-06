// Program: File Hash Generator
// Description: Generates MD5, SHA1, and SHA256 hashes for files to verify integrity

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== File Hash Generator ===");
        Console.WriteLine("1. Hash a single file");
        Console.WriteLine("2. Hash all files in a directory");
        Console.WriteLine("3. Verify file integrity (compare hashes)");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1": HashSingleFile(); break;
            case "2": HashDirectory(); break;
            case "3": VerifyFile(); break;
            default: Console.WriteLine("Invalid option."); break;
        }
    }

    static void HashSingleFile()
    {
        Console.Write("Enter file path: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        try
        {
            long fileSize = new FileInfo(filePath).Length;
            Console.WriteLine($"\n--- File: {Path.GetFileName(filePath)} ---");
            Console.WriteLine($"  Size: {FormatSize(fileSize)}");
            Console.WriteLine();
            Console.WriteLine($"  MD5:    {ComputeHash(filePath, "MD5")}");
            Console.WriteLine($"  SHA-1:  {ComputeHash(filePath, "SHA1")}");
            Console.WriteLine($"  SHA-256:{ComputeHash(filePath, "SHA256")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void HashDirectory()
    {
        Console.Write("Enter directory path: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        Console.Write("Hash algorithm (MD5, SHA1, SHA256): ");
        string algo = Console.ReadLine().Trim().ToUpper();

        string[] files = Directory.GetFiles(dirPath, "*.*", SearchOption.TopDirectoryOnly);
        Console.WriteLine($"\n--- Hashes for {files.Length} Files ---");
        Console.WriteLine($"  {"File",-30} {algo,-64}");
        Console.WriteLine(new string('-', 96));

        foreach (string file in files)
        {
            try
            {
                string hash = ComputeHash(file, algo);
                Console.WriteLine($"  {Path.GetFileName(file),-30} {hash}");
            }
            catch
            {
                Console.WriteLine($"  {Path.GetFileName(file),-30} [Error reading file]");
            }
        }
    }

    static void VerifyFile()
    {
        Console.Write("Enter file path: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        Console.Write("Enter expected hash: ");
        string expectedHash = Console.ReadLine().Trim();

        if (string.IsNullOrWhiteSpace(expectedHash))
        {
            Console.WriteLine("Error: Expected hash cannot be empty.");
            return;
        }

        // Detect algorithm by hash length
        string algo = expectedHash.Length switch
        {
            32 => "MD5",
            40 => "SHA1",
            64 => "SHA256",
            _ => "SHA256"
        };

        string actualHash = ComputeHash(filePath, algo);

        Console.WriteLine($"\n--- Verification Result ---");
        Console.WriteLine($"  Algorithm:     {algo}");
        Console.WriteLine($"  Expected:      {expectedHash.ToLower()}");
        Console.WriteLine($"  Actual:        {actualHash}");
        Console.WriteLine($"  Match:         {actualHash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase)}");
        Console.WriteLine($"  File Integrity:{(actualHash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase) ? " VERIFIED ✓" : " FAILED ✗")}");
    }

    static string ComputeHash(string filePath, string algorithm)
    {
        using var stream = File.OpenRead(filePath);
        byte[] hashBytes = algorithm.ToUpper() switch
        {
            "MD5" => MD5.HashData(stream),
            "SHA1" => SHA1.HashData(stream),
            "SHA256" => SHA256.HashData(stream),
            _ => SHA256.HashData(stream)
        };

        var sb = new StringBuilder();
        foreach (byte b in hashBytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    static string FormatSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double size = bytes;
        int order = 0;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }
        return order == 0 ? $"{bytes}B" : $"{size:F1}{sizes[order]}";
    }
}
