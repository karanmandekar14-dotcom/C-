// Program: Collection Expression Demo
// Description: Demonstrates C# 12 collection expressions for concise array initialization

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Collection Expressions (C# 12) ===\n");

        // Collection expressions - new concise syntax
        int[] numbers = [1, 2, 3, 4, 5];
        string[] fruits = ["Apple", "Banana", "Cherry", "Date"];
        double[] prices = [19.99, 29.99, 9.99, 49.99];

        Console.WriteLine("--- Basic Collection Expressions ---");
        Console.WriteLine($"  numbers: [{string.Join(", ", numbers)}]");
        Console.WriteLine($"  fruits: [{string.Join(", ", fruits)}]");
        Console.WriteLine($"  prices: [{string.Join(", ", prices)}]");

        // Spread operator - combine collections
        int[] moreNumbers = [.. numbers, 6, 7, 8];
        string[] allFruits = [.. fruits, "Elderberry", "Fig"];

        Console.WriteLine($"\n--- Spread Operator ---");
        Console.WriteLine($"  [..numbers, 6, 7, 8]: [{string.Join(", ", moreNumbers)}]");
        Console.WriteLine($"  [..fruits, \"Elderberry\", \"Fig\"]: [{string.Join(", ", allFruits)}]");

        // Combine multiple collections
        int[] odds = [1, 3, 5];
        int[] evens = [2, 4, 6];
        int[] combined = [.. odds, .. evens];

        Console.WriteLine($"\n--- Combining Collections ---");
        Console.WriteLine($"  odds:  [{string.Join(", ", odds)}]");
        Console.WriteLine($"  evens: [{string.Join(", ", evens)}]");
        Console.WriteLine($"  combined: [{string.Join(", ", combined)}]");

        // Span and ReadOnlySpan with collection expressions
        Span<int> span = [10, 20, 30, 40, 50];
        Console.WriteLine($"\n--- Span with Collection Expressions ---");
        Console.WriteLine($"  span: [{string.Join(", ", span.ToArray())}]");

        // Empty collection
        int[] empty = [];
        Console.WriteLine($"\n--- Empty Collection ---");
        Console.WriteLine($"  empty.Length: {empty.Length}");

        // Nested collections
        int[][] matrix =
        [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9]
        ];

        Console.WriteLine($"\n--- Matrix (Nested Collections) ---");
        foreach (var row in matrix)
        {
            Console.WriteLine($"  [{string.Join(", ", row)}]");
        }

        // Using with LINQ
        var expensiveItems = prices.Where(p => p > 15).ToArray();
        Console.WriteLine($"\n--- LINQ with Collection Expressions ---");
        Console.WriteLine($"  Expensive items (>$15): [{string.Join(", ", expensiveItems)}]");

        // Stack and Queue initialization
        Stack<string> stack = ["First", "Second", "Third"];
        Queue<string> queue = ["Alice", "Bob", "Charlie"];

        Console.WriteLine($"\n--- Stack & Queue ---");
        Console.WriteLine($"  stack (pop order): {string.Join(", ", stack.Reverse())}");
        Console.WriteLine($"  queue (dequeue order): {string.Join(", ", queue)}");

        // Practical example: building a result set
        Console.WriteLine($"\n--- Practical Example: Building Report Data ---");
        var reportData = BuildReportData(
            ["Q1", "Q2", "Q3", "Q4"],
            [100, 150, 200, 175],
            [90, 140, 180, 160]
        );

        foreach (var row in reportData)
        {
            Console.WriteLine($"  {row.Quarter}: Budget={row.Budget}, Actual={row.Actual}, Variance={row.Budget - row.Actual}");
        }
    }

    static List<ReportRow> BuildReportData(string[] quarters, int[] budgets, int[] actuals)
    {
        var results = new List<ReportRow>();
        for (int i = 0; i < quarters.Length; i++)
        {
            results.Add(new ReportRow
            {
                Quarter = quarters[i],
                Budget = budgets[i],
                Actual = actuals[i]
            });
        }
        return results;
    }
}

record ReportRow
{
    public string Quarter { get; init; }
    public int Budget { get; init; }
    public int Actual { get; init; }
}
