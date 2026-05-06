// Program: Pattern Matching Shape Calculator
// Description: Demonstrates C# switch expressions, positional patterns, and when guards

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Pattern Matching Shape Calculator ===");

        // Create shapes using positional patterns
        Shape[] shapes =
        {
            new Circle(5),
            new Rectangle(4, 6),
            new Square(3),
            new Triangle(3, 4, 5),
            new Circle(0),
            new Rectangle(-2, 5),
            null,
            new Triangle(1, 1, 10) // Invalid triangle
        };

        Console.WriteLine("--- Shape Analysis Using Pattern Matching ---\n");

        foreach (var shape in shapes)
        {
            // Switch expression with patterns
            string description = shape switch
            {
                null => "Null shape",
                Circle(var r) when r < 0 => $"Circle with invalid radius: {r}",
                Circle(0) => "Circle with zero radius (degenerate)",
                Circle(var r) => $"Circle with radius {r}",
                Rectangle(var w, var h) when w <= 0 || h <= 0 => $"Rectangle with invalid dimensions: {w}x{h}",
                Rectangle(var w, var h) => $"Rectangle {w}x{h}",
                Square(var s) when s <= 0 => $"Square with invalid side: {s}",
                Square(var s) => $"Square with side {s}",
                Triangle(var a, var b, var c) when !IsValidTriangle(a, b, c) => $"Invalid triangle: {a},{b},{c}",
                Triangle(var a, var b, var c) => $"Triangle ({a},{b},{c})",
                _ => "Unknown shape"
            };

            // Area calculation with type patterns
            double area = shape switch
            {
                Circle { Radius: var r } when r > 0 => Math.PI * r * r,
                Rectangle { Width: var w, Height: var h } when w > 0 && h > 0 => w * h,
                Square { Side: var s } when s > 0 => s * s,
                Triangle(var a, var b, var c) when IsValidTriangle(a, b, c) => HeronFormula(a, b, c),
                _ => 0
            };

            // Classification with property patterns
            string sizeCategory = area switch
            {
                0 => "None/Degenerate",
                < 10 => "Small",
                < 50 => "Medium",
                < 100 => "Large",
                _ => "Very Large"
            };

            Console.WriteLine($"  {description}");
            Console.WriteLine($"    Area: {area:F2} [{sizeCategory}]");
            Console.WriteLine();
        }

        // Tuple pattern matching
        Console.WriteLine("--- Tuple Pattern Matching ---");
        var point = (3, 4);
        string location = point switch
        {
            (0, 0) => "Origin",
            (0, _) => "On Y-axis",
            (_, 0) => "On X-axis",
            (var x, var y) when x > 0 && y > 0 => "Quadrant I",
            (var x, var y) when x < 0 && y > 0 => "Quadrant II",
            (var x, var y) when x < 0 && y < 0 => "Quadrant III",
            _ => "Quadrant IV"
        };
        Console.WriteLine($"  Point {point} is in {location}");

        // List pattern matching (C# 11)
        Console.WriteLine("\n--- List Pattern Matching ---");
        int[] numbers = { 1, 2, 3, 4, 5 };
        Console.WriteLine($"  Array: [{string.Join(", ", numbers)}]");
        Console.WriteLine($"  Pattern: {MatchArrayPattern(numbers)}");
    }

    static bool IsValidTriangle(double a, double b, double c) =>
        a > 0 && b > 0 && c > 0 && a + b > c && a + c > b && b + c > a;

    static double HeronFormula(double a, double b, double c)
    {
        double s = (a + b + c) / 2;
        return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
    }

    static string MatchArrayPattern(int[] arr) => arr switch
    {
        [] => "Empty array",
        [var single] => $"Single element: {single}",
        [var first, var second, ..] => $"Starts with {first}, {second} (length: {arr.Length})",
        _ => "Unknown"
    };
}

abstract record Shape;
record Circle(double Radius) : Shape;
record Rectangle(double Width, double Height) : Shape;
record Square(double Side) : Shape;
record Triangle(double A, double B, double C) : Shape;
