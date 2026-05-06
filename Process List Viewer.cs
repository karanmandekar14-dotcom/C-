// Program: Process List Viewer
// Description: Lists running processes with memory and CPU information

using System;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Process List Viewer ===");
        Console.WriteLine("1. List all processes");
        Console.WriteLine("2. Search for a process");
        Console.WriteLine("3. Show detailed process info");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1": ListAllProcesses(); break;
            case "2": SearchProcess(); break;
            case "3": ShowDetailedInfo(); break;
            default: Console.WriteLine("Invalid option."); break;
        }
    }

    static void ListAllProcesses()
    {
        Process[] processes = Process.GetProcesses();

        Console.WriteLine($"\n--- {processes.Length} Running Processes ---");
        Console.WriteLine($"  {"ID",-8} {"Name",-25} {"Memory (MB)",-12} {"Threads",-8}");
        Console.WriteLine(new string('-', 60));

        var sorted = processes.OrderBy(p => p.ProcessName).ToList();
        foreach (Process p in sorted)
        {
            try
            {
                string name = p.ProcessName;
                int id = p.Id;
                long memory = p.WorkingSet64 / (1024 * 1024);
                int threads = p.Threads.Count;

                Console.WriteLine($"  {id,-8} {name,-25} {memory,-12:N1} {threads,-8}");
            }
            catch { }
        }

        long totalMemory = 0;
        int totalProcesses = 0;
        foreach (Process p in processes)
        {
            try
            {
                totalMemory += p.WorkingSet64;
                totalProcesses++;
            }
            catch { }
        }

        Console.WriteLine(new string('-', 60));
        Console.WriteLine($"  Total memory used: {totalMemory / (1024 * 1024)} MB");
        Console.WriteLine($"  Total processes:   {totalProcesses}");
    }

    static void SearchProcess()
    {
        Console.Write("Enter process name (partial match): ");
        string search = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(search))
            return;

        Process[] processes = Process.GetProcesses();
        var matches = processes.Where(p =>
            p.ProcessName.Contains(search, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine($"No processes matching '{search}'.");
            return;
        }

        Console.WriteLine($"\n--- {matches.Count} Match(es) Found ---");
        foreach (Process p in matches)
        {
            try
            {
                long memory = p.WorkingSet64 / (1024 * 1024);
                Console.WriteLine($"  {p.ProcessName} (ID: {p.Id}) - Memory: {memory:N1} MB");
            }
            catch { }
        }
    }

    static void ShowDetailedInfo()
    {
        Console.Write("Enter process ID or name: ");
        string input = Console.ReadLine();

        Process process = null;
        if (int.TryParse(input, out int pid))
        {
            try { process = Process.GetProcessById(pid); } catch { }
        }
        else
        {
            var processes = Process.GetProcesses();
            process = processes.FirstOrDefault(p =>
                p.ProcessName.Equals(input, StringComparison.OrdinalIgnoreCase));
        }

        if (process == null)
        {
            Console.WriteLine("Process not found.");
            return;
        }

        try
        {
            Console.WriteLine($"\n--- Process Details ---");
            Console.WriteLine($"  Name:           {process.ProcessName}");
            Console.WriteLine($"  ID:             {process.Id}");
            Console.WriteLine($"  Memory (Working Set): {process.WorkingSet64 / (1024 * 1024):N1} MB");
            Console.WriteLine($"  Private Memory: {process.PrivateMemorySize64 / (1024 * 1024):N1} MB");
            Console.WriteLine($"  Virtual Memory: {process.VirtualMemorySize64 / (1024 * 1024):N1} MB");
            Console.WriteLine($"  Threads:        {process.Threads.Count}");
            Console.WriteLine($"  Handles:        {process.HandleCount}");
            Console.WriteLine($"  Main Window:    {(process.MainWindowTitle != "" ? process.MainWindowTitle : "No Window")}");
            Console.WriteLine($"  Started:        {process.StartTime:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  Responding:     {process.Responding}");

            if (process.MainModule != null)
            {
                Console.WriteLine($"  Path:           {process.MainModule.FileName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accessing process info: {ex.Message}");
        }
    }
}
