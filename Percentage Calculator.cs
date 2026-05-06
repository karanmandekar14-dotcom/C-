// Program: Percentage Calculator
// Description: Calculates X% of Y, percentage change, percentage of total, and more

using System;

class Program
{
    static void Main()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n=== Percentage Calculator ===");
            Console.WriteLine("1. What is X% of Y?");
            Console.WriteLine("2. X is what % of Y?");
            Console.WriteLine("3. Percentage change from X to Y");
            Console.WriteLine("4. X increased by Y%");
            Console.WriteLine("5. X decreased by Y%");
            Console.WriteLine("6. Percentage of total (from list)");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": PercentOf(); break;
                case "2": IsWhatPercent(); break;
                case "3": PercentChange(); break;
                case "4": PercentIncrease(); break;
                case "5": PercentDecrease(); break;
                case "6": PercentOfTotal(); break;
                case "0":
                    exit = true;
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void PercentOf()
    {
        Console.Write("Enter X (percentage): ");
        if (!double.TryParse(Console.ReadLine(), out double x)) return;
        Console.Write("Enter Y (value): ");
        if (!double.TryParse(Console.ReadLine(), out double y)) return;

        double result = x / 100 * y;
        Console.WriteLine($"  {x}% of {y:N2} = {result:N2}");
    }

    static void IsWhatPercent()
    {
        Console.Write("Enter X (part): ");
        if (!double.TryParse(Console.ReadLine(), out double x)) return;
        Console.Write("Enter Y (whole): ");
        if (!double.TryParse(Console.ReadLine(), out double y)) return;

        if (y == 0)
        {
            Console.WriteLine("Error: Cannot divide by zero.");
            return;
        }

        double result = x / y * 100;
        Console.WriteLine($"  {x:N2} is {result:F2}% of {y:N2}");
    }

    static void PercentChange()
    {
        Console.Write("Enter X (original value): ");
        if (!double.TryParse(Console.ReadLine(), out double x)) return;
        Console.Write("Enter Y (new value): ");
        if (!double.TryParse(Console.ReadLine(), out double y)) return;

        if (x == 0)
        {
            Console.WriteLine("Error: Original value cannot be zero.");
            return;
        }

        double change = (y - x) / Math.Abs(x) * 100;
        string direction = change >= 0 ? "increase" : "decrease";
        Console.WriteLine($"  {Math.Abs(change):F2}% {direction} from {x:N2} to {y:N2}");
    }

    static void PercentIncrease()
    {
        Console.Write("Enter X (value): ");
        if (!double.TryParse(Console.ReadLine(), out double x)) return;
        Console.Write("Enter Y (percentage increase): ");
        if (!double.TryParse(Console.ReadLine(), out double y)) return;

        double result = x * (1 + y / 100);
        Console.WriteLine($"  {x:N2} increased by {y}% = {result:N2}");
    }

    static void PercentDecrease()
    {
        Console.Write("Enter X (value): ");
        if (!double.TryParse(Console.ReadLine(), out double x)) return;
        Console.Write("Enter Y (percentage decrease): ");
        if (!double.TryParse(Console.ReadLine(), out double y)) return;

        double result = x * (1 - y / 100);
        Console.WriteLine($"  {x:N2} decreased by {y}% = {result:N2}");
    }

    static void PercentOfTotal()
    {
        Console.WriteLine("Enter values (comma-separated): ");
        string input = Console.ReadLine();
        string[] parts = input.Split(',');
        var values = new System.Collections.Generic.List<double>();

        foreach (string part in parts)
        {
            if (double.TryParse(part.Trim(), out double v))
                values.Add(v);
        }

        if (values.Count == 0)
        {
            Console.WriteLine("Error: No valid values entered.");
            return;
        }

        double total = 0;
        foreach (double v in values) total += v;

        Console.WriteLine($"\n  Total: {total:N2}\n");
        for (int i = 0; i < values.Count; i++)
        {
            double percent = values[i] / total * 100;
            string bar = new string('#', (int)(percent / 2));
            Console.WriteLine($"  {values[i],10:N2} = {percent,6:F2}% {bar}");
        }
    }
}
