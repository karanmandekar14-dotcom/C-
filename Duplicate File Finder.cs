// Program: Duplicate File Finder
// Description: Finds duplicate files in a directory by comparing content hashes

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Duplicate File Finder ===");
        Console.Write("Enter directory path to scan: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found or path is empty.");
            return;
        }

        Console.WriteLine("Scanning for duplicate files...");

        try
        {
            var fileHashes = new Dictionary<string, List<string>>();
            string[] files = Directory.GetFiles(dirPath, "*.*", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                try
                {
                    string hash = ComputeFileHash(file);
                    if (!fileHashes.ContainsKey(hash))
                    {
                        fileHashes[hash] = new List<string>();
                    }
                    fileHashes[hash].Add(file);
                }
                catch
                {
                    // Skip files we can't read
                }
            }

            var duplicates = fileHashes.Where(kv => kv.Value.Count > 1).ToList();

            if (duplicates.Count == 0)
            {
                Console.WriteLine("No duplicate files found.");
            }
            else
            {
                Console.WriteLine($"\nFound {duplicates.Count} group(s) of duplicates:\n");
                int group = 1;
                foreach (var dupGroup in duplicates)
                {
                    Console.WriteLine($"  Group {group++}:");
                    foreach (string file in dupGroup.Value)
                    {
                        long size = new FileInfo(file).Length;
                        Console.WriteLine($"    - {Path.GetFileName(file)} ({size:N0} bytes)");
                    }
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static string ComputeFileHash(string filePath)
    {
        using (var sha256 = SHA256.Create())
        using (var stream = File.OpenRead(filePath))
        {
            byte[] hashBytes = sha256.ComputeHash(stream);
            var sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
