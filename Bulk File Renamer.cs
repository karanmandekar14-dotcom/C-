// Program: Bulk File Renamer
// Description: Simulates renaming multiple files with pattern (prefix, suffix, numbering)

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Bulk File Renamer ===");

        Console.Write("Enter number of files: ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Error: Please enter a valid positive number.");
            return;
        }

        var fileNames = new List<string>();
        for (int i = 0; i < count; i++)
        {
            Console.Write($"Enter name for file {i + 1} (with extension): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                fileNames.Add(name.Trim());
            }
        }

        Console.Write("Enter prefix (leave empty for none): ");
        string prefix = Console.ReadLine();

        Console.Write("Enter suffix (leave empty for none): ");
        string suffix = Console.ReadLine();

        Console.Write("Start numbering from (enter 0 to skip numbering): ");
        if (!int.TryParse(Console.ReadLine(), out int startNumber))
        {
            startNumber = 0;
        }

        Console.WriteLine("\n--- Renamed Files ---");
        for (int i = 0; i < fileNames.Count; i++)
        {
            string original = fileNames[i];
            string nameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(original);
            string extension = System.IO.Path.GetExtension(original);

            string newName = prefix;
            if (startNumber > 0)
            {
                newName += $"{nameWithoutExt}_{startNumber + i}{extension}";
            }
            else
            {
                newName += $"{nameWithoutExt}{extension}";
            }
            newName = newName.Insert(newName.Length - extension.Length, suffix);

            Console.WriteLine($"  {original} -> {newName}");
        }
    }
}
