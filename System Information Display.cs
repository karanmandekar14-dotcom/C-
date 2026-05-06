// Program: System Information Display
// Description: Displays OS, CPU, memory, disk, and network information

using System;
using System.IO;
using System.Linq;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== System Information Display ===");

        // OS Information
        Console.WriteLine($"\n--- Operating System ---");
        Console.WriteLine($"  OS:               {Environment.OSVersion}");
        Console.WriteLine($"  OS Version:       {Environment.OSVersion.VersionString}");
        Console.WriteLine($"  Platform:         {Environment.OSVersion.Platform}");
        Console.WriteLine($"  Machine Name:     {Environment.MachineName}");
        Console.WriteLine($"  User Name:        {Environment.UserName}");
        Console.WriteLine($"  Domain:           {Environment.UserDomainName}");
        Console.WriteLine($"  CLR Version:      {Environment.Version}");
        Console.WriteLine($"  Architecture:     {Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit"}");
        Console.WriteLine($"  Processors:       {Environment.ProcessorCount}");

        // Memory Information
        Console.WriteLine($"\n--- Memory ---");
        long workingSet = Process.GetCurrentProcess().WorkingSet64;
        Console.WriteLine($"  Current Process:  {workingSet / (1024 * 1024):N1} MB");
        Console.WriteLine($"  Managed Memory:   {GC.GetGCMemoryInfo().HeapSizeBytes / (1024 * 1024):N1} MB");

        // Disk Information
        Console.WriteLine($"\n--- Drives ---");
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (!drive.IsReady) continue;

            double totalGB = drive.TotalSize / (1024.0 * 1024 * 1024);
            double freeGB = drive.TotalFreeSpace / (1024.0 * 1024 * 1024);
            double usedGB = totalGB - freeGB;
            double usedPercent = totalGB > 0 ? usedGB / totalGB * 100 : 0;
            int barLength = (int)(usedPercent / 100 * 30);
            string bar = new string('█', barLength) + new string('░', 30 - barLength);

            Console.WriteLine($"  {drive.Name,-5} {drive.DriveType}");
            Console.WriteLine($"    Label:          {drive.VolumeLabel}");
            Console.WriteLine($"    File System:    {drive.DriveFormat}");
            Console.WriteLine($"    Total:          {totalGB:F1} GB");
            Console.WriteLine($"    Used:           {usedGB:F1} GB ({usedPercent:F1}%)");
            Console.WriteLine($"    Free:           {freeGB:F1} GB");
            Console.WriteLine($"    {bar}");
            Console.WriteLine();
        }

        // Environment Variables
        Console.WriteLine($"--- Environment ---");
        Console.WriteLine($"  Current Directory: {Environment.CurrentDirectory}");
        Console.WriteLine($"  System Directory:  {Environment.SystemDirectory}");
        Console.WriteLine($"  Temp Directory:    {Environment.GetEnvironmentVariable("TEMP")}");
        Console.WriteLine($"  User Profile:      {Environment.GetEnvironmentVariable("USERPROFILE")}");

        // Running Processes Summary
        Console.WriteLine($"\n--- Processes ---");
        Process[] processes = Process.GetProcesses();
        Console.WriteLine($"  Running:          {processes.Length}");

        long totalProcessMemory = 0;
        foreach (Process p in processes)
        {
            try { totalProcessMemory += p.WorkingSet64; } catch { }
        }
        Console.WriteLine($"  Total Memory:     {totalProcessMemory / (1024 * 1024):N1} MB");

        // Top 5 memory consumers
        Console.WriteLine($"\n  Top 5 by Memory:");
        var topMemory = processes
            .Select(p => (Name: p.ProcessName, Memory: p.WorkingSet64))
            .OrderByDescending(x => x.Memory)
            .Take(5)
            .ToList();

        foreach (var (name, memory) in topMemory)
        {
            Console.WriteLine($"    {name,-25} {memory / (1024 * 1024):N1} MB");
        }

        // System Uptime
        Console.WriteLine($"\n--- Uptime ---");
        using (var uptime = new PerformanceCounter("System", "System Up Time"))
        {
            uptime.NextValue(); // First call returns 0
            float seconds = uptime.NextValue();
            TimeSpan uptimeSpan = TimeSpan.FromSeconds(seconds);
            Console.WriteLine($"  System Uptime:    {uptimeSpan.Days}d {uptimeSpan.Hours}h {uptimeSpan.Minutes}m");
        }
    }
}
