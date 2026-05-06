// Program: Tip Split Calculator
// Description: Splits restaurant bill among people with customizable tip percentage

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Tip Split Calculator ===");

        Console.Write("Enter total bill amount: ");
        if (!double.TryParse(Console.ReadLine(), out double bill) || bill <= 0)
        {
            Console.WriteLine("Error: Enter a valid bill amount.");
            return;
        }

        Console.Write("Enter tip percentage (e.g., 15): ");
        if (!double.TryParse(Console.ReadLine(), out double tipPercent) || tipPercent < 0)
        {
            Console.WriteLine("Error: Enter a valid tip percentage.");
            return;
        }

        Console.Write("Number of people splitting: ");
        if (!int.TryParse(Console.ReadLine(), out int people) || people <= 0)
        {
            Console.WriteLine("Error: Enter a valid number of people.");
            return;
        }

        Console.Write("Split evenly? (y=yes, n=by individual amounts): ");
        bool even = Console.ReadLine().Trim().ToLower() == "y";

        double tipAmount = bill * tipPercent / 100;
        double total = bill + tipAmount;

        Console.WriteLine("\n--- Bill Summary ---");
        Console.WriteLine($"  Subtotal:   {bill:N2}");
        Console.WriteLine($"  Tip ({tipPercent:F1}%): {tipAmount:N2}");
        Console.WriteLine($"  Total:      {total:N2}");

        if (even)
        {
            double perPerson = total / people;
            double tipPerPerson = tipAmount / people;
            Console.WriteLine($"\n--- Split Among {people} People ---");
            Console.WriteLine($"  Per person (bill): {bill / people:N2}");
            Console.WriteLine($"  Per person (tip):  {tipPerPerson:N2}");
            Console.WriteLine($"  Per person (total): {perPerson:N2}");
        }
        else
        {
            Console.WriteLine($"\n--- Individual Amounts ---");
            double totalIndividual = 0;
            for (int i = 0; i < people; i++)
            {
                Console.Write($"  Person {i + 1} meal cost: ");
                if (!double.TryParse(Console.ReadLine(), out double meal) || meal < 0)
                {
                    Console.WriteLine("  Invalid amount, skipping.");
                    continue;
                }
                double personTip = meal * tipPercent / 100;
                double personTotal = meal + personTip;
                totalIndividual += personTotal;
                Console.WriteLine($"    Meal: {meal:N2} | Tip: {personTip:N2} | Total: {personTotal:N2}");
            }
            Console.WriteLine($"\n  Sum of individual totals: {totalIndividual:N2}");
            Console.WriteLine($"  Full bill total:          {total:N2}");
        }
    }
}
