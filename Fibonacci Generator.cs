// Program: Fibonacci Generator
// Description: Generates Fibonacci sequence with multiple modes and mathematical properties

using System;
using System.Collections.Generic;
using System.Numerics;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Fibonacci Generator ===");
        Console.WriteLine("1. Generate first N terms");
        Console.WriteLine("2. Generate up to value N");
        Console.WriteLine("3. Find Nth Fibonacci number");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1": GenerateTerms(); break;
            case "2": GenerateUpToValue(); break;
            case "3": FindNth(); break;
            default: Console.WriteLine("Invalid option."); break;
        }
    }

    static void GenerateTerms()
    {
        Console.Write("Enter number of terms: ");
        if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
        {
            Console.WriteLine("Error: Enter a positive number.");
            return;
        }

        var fibs = GenerateFibonacci(n);
        Console.WriteLine($"\nFirst {n} Fibonacci terms:");
        Console.WriteLine(string.Join(", ", fibs));

        // Analysis
        if (fibs.Count >= 2)
        {
            Console.WriteLine($"\n--- Golden Ratio Convergence ---");
            for (int i = 1; i < Math.Min(fibs.Count, 15); i++)
            {
                double ratio = (double)fibs[i].ToDecimal() / Math.Max((double)fibs[i - 1].ToDecimal(), 1);
                Console.WriteLine($"  F({i + 1})/F({i}) = {ratio:F10}");
            }
            Console.WriteLine($"  Golden ratio (phi) = 1.6180339887...");
        }
    }

    static void GenerateUpToValue()
    {
        Console.Write("Generate Fibonacci numbers up to: ");
        if (!long.TryParse(Console.ReadLine(), out long max) || max <= 0)
        {
            Console.WriteLine("Error: Enter a positive number.");
            return;
        }

        var fibs = new List<long> { 0, 1 };
        while (true)
        {
            long next = fibs[fibs.Count - 1] + fibs[fibs.Count - 2];
            if (next > max) break;
            fibs.Add(next);
        }

        Console.WriteLine($"\nFibonacci numbers up to {max}:");
        Console.WriteLine(string.Join(", ", fibs));
        Console.WriteLine($"Count: {fibs.Count}");
    }

    static void FindNth()
    {
        Console.Write("Enter N (1-based index): ");
        if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
        {
            Console.WriteLine("Error: Enter a positive number.");
            return;
        }

        if (n <= 92)
        {
            long result = GetNthFibonacci(n);
            Console.WriteLine($"\n  F({n}) = {result}");
        }
        else
        {
            // Use BigInteger for large numbers
            BigInteger result = GetNthFibonacciBig(n);
            Console.WriteLine($"\n  F({n}) = {result}");
            Console.WriteLine($"  (This number has {result.ToString().Length} digits)");
        }
    }

    static List<BigInteger> GenerateFibonacci(int n)
    {
        var fibs = new List<BigInteger> { 0, 1 };
        for (int i = 2; i < n; i++)
        {
            fibs.Add(fibs[i - 1] + fibs[i - 2]);
        }
        return fibs.GetRange(0, Math.Min(n, fibs.Count));
    }

    static long GetNthFibonacci(int n)
    {
        if (n <= 0) return 0;
        if (n == 1) return 0;
        if (n == 2) return 1;

        long a = 0, b = 1;
        for (int i = 3; i <= n; i++)
        {
            long temp = a + b;
            a = b;
            b = temp;
        }
        return b;
    }

    static BigInteger GetNthFibonacciBig(int n)
    {
        if (n <= 0) return 0;
        if (n == 1) return 0;
        if (n == 2) return 1;

        BigInteger a = 0, b = 1;
        for (int i = 3; i <= n; i++)
        {
            BigInteger temp = a + b;
            a = b;
            b = temp;
        }
        return b;
    }
}
