// Program: Empty Folder Finder
// Description: Finds and optionally deletes empty folders in a directory tree

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Empty Folder Finder ===");
        Console.Write("Enter directory path: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        Console.Write("Include folders with only subfolders but no files? (y/n): ");
        bool includeNoFileFolders = Console.ReadLine().Trim().ToLower() == "y";

        Console.WriteLine("\nScanning...");

        var emptyFolders = FindEmptyFolders(dirPath, includeNoFileFolders);

        if (emptyFolders.Count == 0)
        {
            Console.WriteLine("No empty folders found.");
            return;
        }

        Console.WriteLine($"\n--- Found {emptyFolders.Count} Empty Folder(s) ---");
        foreach (string folder in emptyFolders)
        {
            Console.WriteLine($"  📁 {folder}");
        }

        Console.Write($"\nDelete all empty folders? (y/n): ");
        if (Console.ReadLine().Trim().ToLower() == "y")
        {
            int deleted = 0, failed = 0;
            foreach (string folder in emptyFolders)
            {
                try
                {
                    Directory.Delete(folder);
                    Console.WriteLine($"  Deleted: {folder}");
                    deleted++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Failed: {folder} ({ex.Message})");
                    failed++;
                }
            }
            Console.WriteLine($"\n--- Delete Summary ---");
            Console.WriteLine($"  Deleted: {deleted}");
            Console.WriteLine($"  Failed:  {failed}");
        }
    }

    static List<string> FindEmptyFolders(string path, bool includeNoFileFolders)
    {
        var emptyFolders = new List<string>();
        FindEmptyRecursive(path, emptyFolders, includeNoFileFolders);
        return emptyFolders;
    }

    static bool FindEmptyRecursive(string path, List<string> emptyFolders, bool includeNoFileFolders)
    {
        bool isEmpty = true;

        try
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            // Check if directory has any files
            if (files.Length > 0)
                isEmpty = false;

            // Process subdirectories
            foreach (string dir in dirs)
            {
                bool subDirEmpty = FindEmptyRecursive(dir, emptyFolders, includeNoFileFolders);
                if (subDirEmpty)
                {
                    isEmpty = false; // Parent is not empty because it has this subfolder
                }
            }

            // Determine if this folder should be considered empty
            bool hasOnlyEmptySubfolders = dirs.Length > 0 && dirs.All(d =>
                Directory.GetFiles(d).Length == 0 &&
                Directory.GetDirectories(d).Length == 0);

            bool isTrulyEmpty = files.Length == 0 && dirs.Length == 0;
            bool hasOnlyEmptyContent = includeNoFileFolders && hasOnlyEmptySubfolders;

            if (isTrulyEmpty || hasOnlyEmptyContent)
            {
                emptyFolders.Add(path);
                return true;
            }
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }

        return isEmpty;
    }
}
