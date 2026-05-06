// Program: Type Alias Any Type Demo
// Description: Demonstrates C# 12 type alias improvements - can alias any type, not just named types

using System;
using System.Collections.Generic;
using System.Numerics;

// C# 12: Can now alias any type including tuples, arrays, nullable, generics, etc.
using Point = (int X, int Y);
using NumberList = System.Collections.Generic.List<int>;
using StringDict = System.Collections.Generic.Dictionary<string, string>;
using Matrix = int[,];
using BigIntegerList = System.Collections.Generic.List<System.Numerics.BigInteger>;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Type Alias Improvements (C# 12) ===\n");

        // Tuple alias
        Point origin = (0, 0);
        Point target = (10, 20);
        Console.WriteLine("--- Tuple Alias ---");
        Console.WriteLine($"  Origin: ({origin.X}, {origin.Y})");
        Console.WriteLine($"  Target: ({target.X}, {target.Y})");
        Console.WriteLine($"  Distance: {Distance(origin, target):F2}");

        // Generic collection alias
        NumberList numbers = [1, 2, 3, 4, 5];
        Console.WriteLine($"\n--- Generic Collection Alias ---");
        Console.WriteLine($"  NumberList: [{string.Join(", ", numbers)}]");
        Console.WriteLine($"  Sum: {numbers.Sum()}");

        // Dictionary alias
        StringDict config = new()
        {
            ["Database"] = "localhost",
            ["Port"] = "5432",
            ["Username"] = "admin"
        };
        Console.WriteLine($"\n--- Dictionary Alias ---");
        foreach (var (key, value) in config)
        {
            Console.WriteLine($"  {key}: {value}");
        }

        // 2D array alias
        Matrix matrix = {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 }
        };
        Console.WriteLine($"\n--- Matrix Alias ---");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"  [{matrix[i, 0]}, {matrix[i, 1]}, {matrix[i, 2]}]");
        }
        Console.WriteLine($"  Determinant: {Determinant3x3(matrix)}");

        // BigInteger list alias
        BigIntegerList bigNumbers = new()
        {
            BigInteger.Parse("123456789012345678901234567890"),
            BigInteger.Parse("987654321098765432109876543210"),
            BigInteger.Parse("111111111111111111111111111111")
        };
        Console.WriteLine($"\n--- BigInteger List Alias ---");
        foreach (var num in bigNumbers)
        {
            Console.WriteLine($"  {num}");
        }
        Console.WriteLine($"  Product has {bigNumbers.Aggregate(BigInteger.One, (a, b) => a * b).ToString().Length} digits");
    }

    static double Distance(Point a, Point b)
    {
        double dx = b.X - a.X;
        double dy = b.Y - a.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    static int Determinant3x3(Matrix m)
    {
        return m[0, 0] * (m[1, 1] * m[2, 2] - m[1, 2] * m[2, 1])
             - m[0, 1] * (m[1, 0] * m[2, 2] - m[1, 2] * m[2, 0])
             + m[0, 2] * (m[1, 0] * m[2, 1] - m[1, 1] * m[2, 0]);
    }
}
