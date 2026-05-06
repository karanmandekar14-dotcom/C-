// Program: INI File Reader/Writer
// Description: Reads, parses, and writes INI-style configuration files

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== INI File Manager ===");
        Console.WriteLine("1. Read INI file");
        Console.WriteLine("2. Create/Edit INI value");
        Console.WriteLine("3. Create new INI file");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ReadIniFile();
                break;
            case "2":
                EditIniValue();
                break;
            case "3":
                CreateIniFile();
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }

    static void ReadIniFile()
    {
        Console.Write("Enter INI file path: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        try
        {
            var config = ParseIniFile(filePath);
            Console.WriteLine($"\n--- Contents of {Path.GetFileName(filePath)} ---");

            foreach (var section in config)
            {
                Console.WriteLine($"\n[{section.Key}]");
                foreach (var kvp in section.Value)
                {
                    Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void EditIniValue()
    {
        Console.Write("Enter INI file path: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        Console.Write("Enter section name: ");
        string section = Console.ReadLine();
        Console.Write("Enter key name: ");
        string key = Console.ReadLine();
        Console.Write("Enter new value: ");
        string value = Console.ReadLine();

        try
        {
            var config = ParseIniFile(filePath);

            if (!config.ContainsKey(section))
            {
                config[section] = new Dictionary<string, string>();
            }
            config[section][key] = value;

            WriteIniFile(filePath, config);
            Console.WriteLine($"Updated [{section}] {key} = {value}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void CreateIniFile()
    {
        Console.Write("Enter file path to create (e.g., config.ini): ");
        string filePath = Console.ReadLine();

        var config = new Dictionary<string, Dictionary<string, string>>();

        while (true)
        {
            Console.Write("Enter section name (or 'done' to finish): ");
            string section = Console.ReadLine();
            if (section.Equals("done", StringComparison.OrdinalIgnoreCase))
                break;

            config[section] = new Dictionary<string, string>();

            while (true)
            {
                Console.Write($"  Enter key (or 'done' to finish section): ");
                string key = Console.ReadLine();
                if (key.Equals("done", StringComparison.OrdinalIgnoreCase))
                    break;

                Console.Write($"  Enter value for '{key}': ");
                string value = Console.ReadLine();
                config[section][key] = value;
            }
        }

        WriteIniFile(filePath, config);
        Console.WriteLine($"INI file created: {filePath}");
    }

    static Dictionary<string, Dictionary<string, string>> ParseIniFile(string filePath)
    {
        var config = new Dictionary<string, Dictionary<string, string>>();
        string currentSection = "General";
        config[currentSection] = new Dictionary<string, string>();

        foreach (string line in File.ReadLines(filePath))
        {
            string trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";") || trimmed.StartsWith("#"))
                continue;

            if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
            {
                currentSection = trimmed.Substring(1, trimmed.Length - 2);
                if (!config.ContainsKey(currentSection))
                {
                    config[currentSection] = new Dictionary<string, string>();
                }
                continue;
            }

            int eqIndex = trimmed.IndexOf('=');
            if (eqIndex > 0)
            {
                string key = trimmed.Substring(0, eqIndex).Trim();
                string value = trimmed.Substring(eqIndex + 1).Trim();
                config[currentSection][key] = value;
            }
        }

        return config;
    }

    static void WriteIniFile(string filePath, Dictionary<string, Dictionary<string, string>> config)
    {
        var lines = new List<string>();
        foreach (var section in config)
        {
            lines.Add($"[{section.Key}]");
            foreach (var kvp in section.Value)
            {
                lines.Add($"{kvp.Key} = {kvp.Value}");
            }
            lines.Add("");
        }
        File.WriteAllLines(filePath, lines);
    }
}
