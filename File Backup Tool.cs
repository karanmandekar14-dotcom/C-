// Program: File Backup Tool
// Description: Creates timestamped backup copies of files with version management

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== File Backup Tool ===");
        Console.WriteLine("1. Backup a single file");
        Console.WriteLine("2. Backup entire directory");
        Console.WriteLine("3. List backups");
        Console.WriteLine("4. Restore a backup");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1": BackupFile(); break;
            case "2": BackupDirectory(); break;
            case "3": ListBackups(); break;
            case "4": RestoreBackup(); break;
            default: Console.WriteLine("Invalid option."); break;
        }
    }

    static void BackupFile()
    {
        Console.Write("Enter file path to backup: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        Console.Write("Enter backup directory (or press Enter for same directory): ");
        string backupDir = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(backupDir))
        {
            backupDir = Path.Combine(Path.GetDirectoryName(filePath), "_backups");
        }

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string ext = Path.GetExtension(filePath);
        string backupPath = Path.Combine(backupDir, $"{fileName}_{timestamp}{ext}");

        try
        {
            if (!Directory.Exists(backupDir))
                Directory.CreateDirectory(backupDir);

            File.Copy(filePath, backupPath, true);
            long originalSize = new FileInfo(filePath).Length;
            long backupSize = new FileInfo(backupPath).Length;

            Console.WriteLine($"\n--- Backup Created ---");
            Console.WriteLine($"  Original: {filePath} ({FormatSize(originalSize)})");
            Console.WriteLine($"  Backup:   {backupPath} ({FormatSize(backupSize)})");
            Console.WriteLine($"  Time:     {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void BackupDirectory()
    {
        Console.Write("Enter directory to backup: ");
        string sourceDir = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(sourceDir) || !Directory.Exists(sourceDir))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string backupName = $"{Path.GetFileName(sourceDir)}_backup_{timestamp}";
        Console.Write("Enter backup parent directory: ");
        string parentDir = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(parentDir))
        {
            Console.WriteLine("Error: Backup directory required.");
            return;
        }

        string backupPath = Path.Combine(parentDir, backupName);

        try
        {
            CopyDirectory(sourceDir, backupPath);
            Console.WriteLine($"\n--- Directory Backup Complete ---");
            Console.WriteLine($"  Source: {sourceDir}");
            Console.WriteLine($"  Backup: {backupPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ListBackups()
    {
        Console.Write("Enter backup directory path: ");
        string backupDir = Console.ReadLine();

        if (!Directory.Exists(backupDir))
        {
            Console.WriteLine("Error: Directory not found.");
            return;
        }

        var backups = Directory.GetFiles(backupDir, "*_*", SearchOption.AllDirectories)
            .OrderByDescending(f => new FileInfo(f).LastWriteTime)
            .ToList();

        if (backups.Count == 0)
        {
            Console.WriteLine("No backups found.");
            return;
        }

        Console.WriteLine($"\n--- {backups.Count} Backup(s) Found ---");
        foreach (string backup in backups)
        {
            var info = new FileInfo(backup);
            Console.WriteLine($"  {info.Name,-40} {FormatSize(info.Length),-10} {info.LastWriteTime:yyyy-MM-dd HH:mm}");
        }
    }

    static void RestoreBackup()
    {
        Console.Write("Enter backup file path: ");
        string backupPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(backupPath) || !File.Exists(backupPath))
        {
            Console.WriteLine("Error: Backup file not found.");
            return;
        }

        Console.Write("Enter restore path: ");
        string restorePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(restorePath))
        {
            Console.WriteLine("Error: Restore path required.");
            return;
        }

        try
        {
            string restoreDir = Path.GetDirectoryName(restorePath);
            if (!string.IsNullOrEmpty(restoreDir) && !Directory.Exists(restoreDir))
                Directory.CreateDirectory(restoreDir);

            File.Copy(backupPath, restorePath, true);
            Console.WriteLine($"\n--- Restored ---");
            Console.WriteLine($"  From: {backupPath}");
            Console.WriteLine($"  To:   {restorePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void CopyDirectory(string source, string dest)
    {
        Directory.CreateDirectory(dest);
        foreach (string file in Directory.GetFiles(source))
        {
            string destFile = Path.Combine(dest, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }
        foreach (string subDir in Directory.GetDirectories(source))
        {
            string destSubDir = Path.Combine(dest, Path.GetFileName(subDir));
            CopyDirectory(subDir, destSubDir);
        }
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
