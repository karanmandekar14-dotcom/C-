// Program: Config File Parser
// Description: Simple key-value configuration file parser with section support

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Config File Parser ===");
        Console.WriteLine("1. Parse and display config");
        Console.WriteLine("2. Search for a key");
        Console.WriteLine("3. Compare two config files");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ParseAndDisplay();
                break;
            case "2":
                SearchForKey();
                break;
            case "3":
                CompareConfigs();
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }

    static void ParseAndDisplay()
    {
        Console.Write("Enter config file path: ");
        string path = Console.ReadLine();

        if (!File.Exists(path))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        try
        {
            var config = ParseConfig(path);
            Console.WriteLine($"\n--- Config: {Path.GetFileName(path)} ---");

            foreach (var section in config)
            {
                Console.WriteLine($"\n[{section.Key}]");
                foreach (var kvp in section.Value)
                {
                    Console.WriteLine($"  {kvp.Key,-25} = {kvp.Value}");
                }
            }

            Console.WriteLine($"\nTotal sections: {config.Count}");
            Console.WriteLine($"Total keys: {config.Values.Sum(s => s.Count)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void SearchForKey()
    {
        Console.Write("Enter config file path: ");
        string path = Console.ReadLine();

        if (!File.Exists(path))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        Console.Write("Enter key to search: ");
        string searchKey = Console.ReadLine().Trim();

        try
        {
            var config = ParseConfig(path);
            bool found = false;

            foreach (var section in config)
            {
                if (section.Value.ContainsKey(searchKey))
                {
                    Console.WriteLine($"  [{section.Key}] {searchKey} = {section.Value[searchKey]}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine($"Key '{searchKey}' not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void CompareConfigs()
    {
        Console.Write("Enter first config file path: ");
        string path1 = Console.ReadLine();
        Console.Write("Enter second config file path: ");
        string path2 = Console.ReadLine();

        if (!File.Exists(path1) || !File.Exists(path2))
        {
            Console.WriteLine("Error: One or both files not found.");
            return;
        }

        try
        {
            var config1 = ParseConfig(path1);
            var config2 = ParseConfig(path2);

            var allKeys1 = new HashSet<string>();
            var allKeys2 = new HashSet<string>();

            foreach (var s in config1.Values)
                foreach (var k in s.Keys)
                    allKeys1.Add(k);

            foreach (var s in config2.Values)
                foreach (var k in s.Keys)
                    allKeys2.Add(k);

            var onlyInFirst = allKeys1.Except(allKeys2);
            var onlyInSecond = allKeys2.Except(allKeys1);
            var inBoth = allKeys1.Intersect(allKeys2);

            Console.WriteLine($"\n--- Config Comparison ---");
            Console.WriteLine($"  Keys only in {Path.GetFileName(path1)}: {string.Join(", ", onlyInFirst)}");
            Console.WriteLine($"  Keys only in {Path.GetFileName(path2)}: {string.Join(", ", onlyInSecond)}");
            Console.WriteLine($"  Keys in both: {string.Join(", ", inBoth)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static Dictionary<string, Dictionary<string, string>> ParseConfig(string path)
    {
        var config = new Dictionary<string, Dictionary<string, string>>();
        string currentSection = "default";
        config[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (string rawLine in File.ReadLines(path))
        {
            string line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#") || line.StartsWith("//"))
                continue;

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                currentSection = line.Substring(1, line.Length - 2).Trim();
                if (!config.ContainsKey(currentSection))
                {
                    config[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                continue;
            }

            int eqIdx = line.IndexOf('=');
            if (eqIdx > 0)
            {
                string key = line.Substring(0, eqIdx).Trim();
                string value = line.Substring(eqIdx + 1).Trim();
                config[currentSection][key] = value;
            }
        }

        return config;
    }
}
