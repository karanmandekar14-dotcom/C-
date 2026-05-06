// Program: Directory Tree Printer
// Description: Prints directory structure as a visual tree

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Directory Tree Printer ===");
        Console.Write("Enter directory path: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        Console.Write("Max depth (0 = unlimited): ");
        if (!int.TryParse(Console.ReadLine(), out int maxDepth))
        {
            maxDepth = 0;
        }

        Console.Write("Include files? (y/n): ");
        bool includeFiles = Console.ReadLine().Trim().ToLower() == "y";

        Console.Write("Save to file? (y/n): ");
        bool saveToFile = Console.ReadLine().Trim().ToLower() == "y";

        string dirName = Path.GetFileName(dirPath.TrimEnd(Path.DirectorySeparatorChar));
        var output = new List<string>();

        Console.WriteLine($"\n--- Directory Tree: {dirName} ---");
        Console.WriteLine();

        string tree = BuildTree(dirPath, "", true, maxDepth, 0, includeFiles);
        Console.WriteLine(tree);

        if (saveToFile)
        {
            string outputPath = Path.Combine(dirPath, "directory_tree.txt");
            File.WriteAllText(outputPath, tree);
            Console.WriteLine($"\nSaved to: {outputPath}");
        }
    }

    static string BuildTree(string path, string indent, bool isLast, int maxDepth, int currentDepth, bool includeFiles)
    {
        var sb = new System.Text.StringBuilder();

        if (currentDepth == 0)
        {
            sb.AppendLine(Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar)) + "/");
        }

        if (maxDepth > 0 && currentDepth >= maxDepth)
            return sb.ToString();

        try
        {
            string[] dirs = Directory.GetDirectories(path);
            string[] files = includeFiles ? Directory.GetFiles(path) : Array.Empty<string>();

            int totalItems = dirs.Length + files.Length;
            int currentIndex = 0;

            // Directories first
            foreach (string dir in dirs)
            {
                currentIndex++;
                bool lastItem = currentIndex >= totalItems;
                string prefix = indent + (lastItem ? "└── " : "├── ");
                string dirName = Path.GetFileName(dir);
                sb.AppendLine(prefix + dirName + "/");

                string newIndent = indent + (lastItem ? "    " : "│   ");
                sb.Append(BuildTree(dir, newIndent, lastItem, maxDepth, currentDepth + 1, includeFiles));
            }

            // Then files
            foreach (string file in files)
            {
                currentIndex++;
                bool lastItem = currentIndex >= totalItems;
                string prefix = indent + (lastItem ? "└── " : "├── ");
                string fileName = Path.GetFileName(file);
                long size = new FileInfo(file).Length;
                sb.AppendLine(prefix + $"{fileName} ({FormatSize(size)})");
            }
        }
        catch (UnauthorizedAccessException)
        {
            sb.AppendLine(indent + "  [Access Denied]");
        }
        catch (Exception ex)
        {
            sb.AppendLine(indent + $"  [Error: {ex.Message}]");
        }

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
        return order == 0 ? $"{bytes}B" : $"{size:F0}{sizes[order]}";
    }
}
