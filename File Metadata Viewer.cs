// Program: File Metadata Viewer
// Description: Shows detailed file metadata including dates, size, attributes, and hash

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== File Metadata Viewer ===");
        Console.Write("Enter file path: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        try
        {
            var info = new FileInfo(filePath);

            Console.WriteLine($"\n--- File Metadata ---");
            Console.WriteLine($"  Name:              {info.Name}");
            Console.WriteLine($"  Full Path:         {info.FullName}");
            Console.WriteLine($"  Extension:         {info.Extension}");
            Console.WriteLine($"  Size:              {FormatSize(info.Length)} ({info.Length:N0} bytes)");
            Console.WriteLine();
            Console.WriteLine($"  Created:           {info.CreationTime:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  Modified:          {info.LastWriteTime:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  Accessed:          {info.LastAccessTime:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
            Console.WriteLine($"  Is Read Only:      {info.IsReadOnly}");
            Console.WriteLine($"  Is Hidden:         {info.Attributes.HasFlag(FileAttributes.Hidden)}");
            Console.WriteLine($"  Is System:         {info.Attributes.HasFlag(FileAttributes.System)}");
            Console.WriteLine($"  Is Archive:        {info.Attributes.HasFlag(FileAttributes.Archive)}");
            Console.WriteLine($"  Is Compressed:     {info.Attributes.HasFlag(FileAttributes.Compressed)}");
            Console.WriteLine($"  Attributes:        {info.Attributes}");
            Console.WriteLine();

            // File content type hint
            string ext = info.Extension.ToLower();
            string typeHint = GetFileTypeHint(ext);
            Console.WriteLine($"  Type Hint:         {typeHint}");

            // Hash
            Console.Write("  Calculate hash? (y/n): ");
            if (Console.ReadLine().Trim().ToLower() == "y")
            {
                Console.WriteLine("\n  Calculating hashes...");
                string md5 = ComputeHash(filePath, "MD5");
                string sha1 = ComputeHash(filePath, "SHA1");
                string sha256 = ComputeHash(filePath, "SHA256");

                Console.WriteLine($"  MD5:               {md5}");
                Console.WriteLine($"  SHA-1:             {sha1}");
                Console.WriteLine($"  SHA-256:           {sha256}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static string FormatSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double size = bytes;
        int order = 0;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }
        return order == 0 ? $"{bytes} B" : $"{size:F2} {sizes[order]}";
    }

    static string GetFileTypeHint(string ext)
    {
        return ext switch
        {
            ".txt" or ".doc" or ".docx" => "Text Document",
            ".pdf" => "PDF Document",
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".svg" => "Image File",
            ".mp3" or ".wav" or ".flac" => "Audio File",
            ".mp4" or ".avi" or ".mkv" => "Video File",
            ".zip" or ".rar" or ".7z" => "Archive",
            ".exe" => "Executable",
            ".dll" => "Dynamic Link Library",
            ".cs" or ".py" or ".js" or ".java" => "Source Code",
            ".html" or ".htm" => "HTML Document",
            ".css" => "Stylesheet",
            ".json" or ".xml" => "Data File",
            ".csv" => "Spreadsheet Data",
            _ => $"Unknown ({ext})"
        };
    }

    static string ComputeHash(string filePath, string algorithm)
    {
        using var stream = File.OpenRead(filePath);
        byte[] hashBytes = algorithm.ToUpper() switch
        {
            "MD5" => MD5.HashData(stream),
            "SHA1" => SHA1.HashData(stream),
            "SHA256" => SHA256.HashData(stream),
            _ => Array.Empty<byte>()
        };

        var sb = new StringBuilder();
        foreach (byte b in hashBytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}
