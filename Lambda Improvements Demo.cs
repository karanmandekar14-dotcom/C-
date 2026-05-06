// Program: Lambda Improvements Demo
// Description: Demonstrates C# 11 lambda improvements: attributes, natural types, default parameters

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Lambda Improvements (C# 11) ===\n");

        // Natural type lambdas - compiler infers delegate type
        var add = (int x, int y) => x + y;
        var greet = (string name) => $"Hello, {name}!";
        var isEven = (int n) => n % 2 == 0;

        Console.WriteLine("--- Natural Type Lambdas ---");
        Console.WriteLine($"  add(3, 4) = {add(3, 4)}");
        Console.WriteLine($"  greet(\"Alice\") = {greet("Alice")}");
        Console.WriteLine($"  isEven(7) = {isEven(7)}");

        // Lambda with explicit return type (C# 11)
        var multiply = int (int x, int y) => x * y;
        var divide = double (double x, double y) => y != 0 ? x / y : double.NaN;

        Console.WriteLine($"\n--- Explicit Return Type ---");
        Console.WriteLine($"  multiply(5, 6) = {multiply(5, 6)}");
        Console.WriteLine($"  divide(10, 3) = {divide(10, 3):F2}");

        // Lambda with attributes on parameters
        // Note: This demonstrates the concept; custom attributes would be needed for full effect

        // Lambdas in method groups
        var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        Console.WriteLine($"\n--- Lambda LINQ ---");
        Console.WriteLine($"  Numbers: {string.Join(", ", numbers)}");
        Console.WriteLine($"  Even: {string.Join(", ", numbers.Where(n => isEven(n)))}");
        Console.WriteLine($"  Squared: {string.Join(", ", numbers.Select(n => multiply(n, n)))}");
        Console.WriteLine($"  Sum: {numbers.Aggregate(0, (acc, n) => acc + n)}");

        // Lambda as method parameter
        Console.WriteLine($"\n--- Lambda as Callback ---");
        ProcessWithCallback("Test Item",
            item => Console.WriteLine($"  Processing: {item}"),
            item => Console.WriteLine($"  Completed: {item}"),
            error => Console.WriteLine($"  Error: {error}")
        );

        // Lambda with capture and mutation
        Console.WriteLine($"\n--- Lambda Closures ---");
        int counter = 0;
        var increment = () => ++counter;
        var getCounter = () => counter;

        for (int i = 0; i < 5; i++)
            increment();

        Console.WriteLine($"  Counter: {getCounter()}");

        // Span-compatible lambdas (ref struct lambdas)
        Console.WriteLine($"\n--- Span-Compatible Lambdas ---");
        Span<int> span = stackalloc int[] { 10, 20, 30, 40, 50 };
        ProcessSpan(span, x => x * 2);
        Console.WriteLine($"  After transform: {string.Join(", ", span.ToArray())}");
    }

    static void ProcessWithCallback<T>(T item, Action<T> onStart, Action<T> onComplete, Action<string> onError)
    {
        try
        {
            onStart(item);
            // Simulate work
            System.Threading.Thread.Sleep(100);
            onComplete(item);
        }
        catch (Exception ex)
        {
            onError(ex.Message);
        }
    }

    static void ProcessSpan(Span<int> span, Func<int, int> transform)
    {
        for (int i = 0; i < span.Length; i++)
        {
            span[i] = transform(span[i]);
        }
    }
}
