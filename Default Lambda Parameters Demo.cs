// Program: Default Lambda Parameters Demo
// Description: Demonstrates C# 12 default parameters in anonymous functions and lambdas

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Default Lambda Parameters (C# 12) ===\n");

        // Lambda with default parameter
        Func<int, int, int> add = (int x = 10, int y = 20) => x + y;

        Console.WriteLine("--- Default Parameters in Lambdas ---");
        // Note: C# requires explicit parameter passing when invoking lambdas
        // Default values in lambdas are mainly for delegation scenarios
        Console.WriteLine($"  add(5, 15) = {add(5, 15)}");
        Console.WriteLine($"  add(10, 20) = {add(10, 20)}");

        // More practical: factory pattern with defaults
        var createMessage = (string prefix = "Info", string suffix = "[END]") =>
            (string body) => $"{prefix}: {body} {suffix}";

        var defaultMessage = createMessage();
        var warningMessage = createMessage("WARNING", "!!!");

        Console.WriteLine($"\n--- Message Factory ---");
        Console.WriteLine($"  Default: {defaultMessage("System started")}");
        Console.WriteLine($"  Warning: {warningMessage("Low disk space")}");

        // Event handler with optional parameters
        Console.WriteLine($"\n--- Event Simulation ---");
        var buttonClick = CreateButtonHandler("Submit");
        buttonClick.Invoke(null, EventArgs.Empty);

        // LINQ with default lambdas
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var defaultSelector = (int n, int multiplier = 1) => n * multiplier;

        Console.WriteLine($"\n--- LINQ with Lambda ---");
        Console.WriteLine($"  Numbers: {string.Join(", ", numbers)}");
        Console.WriteLine($"  x1: {string.Join(", ", numbers.Select(n => defaultSelector(n)))}");
        Console.WriteLine($"  x2: {string.Join(", ", numbers.Select(n => defaultSelector(n, 2)))}");
        Console.WriteLine($"  x3: {string.Join(", ", numbers.Select(n => defaultSelector(n, 3)))}");

        // Calculator with configurable operations
        Console.WriteLine($"\n--- Configurable Calculator ---");
        var calculator = CreateCalculator((a, b, char op = '+') => op switch
        {
            '+' => a + b,
            '-' => a - b,
            '*' => a * b,
            '/' => b != 0 ? a / b : throw new DivideByZeroException(),
            _ => throw new ArgumentException($"Unknown operator: {op}")
        });

        Console.WriteLine($"  10 + 5 = {calculator(10, 5)}");
        Console.WriteLine($"  10 - 5 = {calculator(10, 5, '-')}");
        Console.WriteLine($"  10 * 5 = {calculator(10, 5, '*')}");
        Console.WriteLine($"  10 / 5 = {calculator(10, 5, '/')}");
    }

    static EventHandler CreateButtonHandler(string buttonName)
    {
        return (sender, e) =>
        {
            Console.WriteLine($"  Button '{buttonName}' clicked!");
        };
    }

    static Func<int, int, char, int> CreateCalculator(Func<int, int, char, int> operation)
    {
        return (a, b, op = '+') => operation(a, b, op);
    }
}
