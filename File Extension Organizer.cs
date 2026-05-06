// Program: File Extension Organizer
// Description: Organizes files by extension into categorized folders

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== File Extension Organizer ===");
        Console.Write("Enter directory to organize: ");
        string dirPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        Console.WriteLine("\nThis will organize files into folders by type:");
        Console.WriteLine("  Documents: .doc, .docx, .pdf, .txt, .xls, .xlsx, .ppt, .pptx");
        Console.WriteLine("  Images:    .jpg, .jpeg, .png, .gif, .bmp, .svg, .ico, .webp");
        Console.WriteLine("  Videos:    .mp4, .avi, .mkv, .mov, .wmv, .flv");
        Console.WriteLine("  Audio:     .mp3, .wav, .flac, .aac, .ogg, .wma");
        Console.WriteLine("  Archives:  .zip, .rar, .7z, .tar, .gz");
        Console.WriteLine("  Code:      .cs, .py, .js, .html, .css, .java, .cpp, .c");
        Console.WriteLine("  Other:     everything else\n");

        Console.Write("Proceed? (y/n): ");
        if (Console.ReadLine().Trim().ToLower() != "y")
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        var categories = new Dictionary<string, string[]>
        {
            { "Documents", new[] { ".doc", ".docx", ".pdf", ".txt", ".xls", ".xlsx", ".ppt", ".pptx", ".rtf", ".odt" } },
            { "Images", new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".ico", ".webp", ".tiff" } },
            { "Videos", new[] { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm" } },
            { "Audio", new[] { ".mp3", ".wav", ".flac", ".aac", ".ogg", ".wma", ".m4a" } },
            { "Archives", new[] { ".zip", ".rar", ".7z", ".tar", ".gz", ".bz2" } },
            { "Code", new[] { ".cs", ".py", ".js", ".html", ".css", ".java", ".cpp", ".c", ".h", ".rb", ".go", ".rs" } }
        };

        string[] allFiles = Directory.GetFiles(dirPath);
        int moved = 0, skipped = 0;

        foreach (string file in allFiles)
        {
            string ext = Path.GetExtension(file).ToLower();
            if (string.IsNullOrEmpty(ext)) continue;

            string category = "Other";
            foreach (var cat in categories)
            {
                if (Array.IndexOf(cat.Value, ext) >= 0)
                {
                    category = cat.Key;
                    break;
                }
            }

            string destDir = Path.Combine(dirPath, category);
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            string destFile = Path.Combine(destDir, Path.GetFileName(file));

            try
            {
                if (File.Exists(destFile))
                {
                    destFile = Path.Combine(destDir, Path.GetFileNameWithoutExtension(file) + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + ext);
                }
                File.Move(file, destFile);
                Console.WriteLine($"  Moved: {Path.GetFileName(file)} -> {category}/");
                moved++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Skipped: {Path.GetFileName(file)} ({ex.Message})");
                skipped++;
            }
        }

        Console.WriteLine($"\n--- Summary ---");
        Console.WriteLine($"  Moved: {moved}");
        Console.WriteLine($"  Skipped: {skipped}");
    }
}
