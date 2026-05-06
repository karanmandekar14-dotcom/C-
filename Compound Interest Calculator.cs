// Program: Compound Interest Calculator
// Description: Calculates future value with compounding frequency and growth visualization

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Compound Interest Calculator ===");

        Console.Write("Enter principal amount: ");
        if (!double.TryParse(Console.ReadLine(), out double principal) || principal <= 0)
        {
            Console.WriteLine("Error: Enter a valid amount.");
            return;
        }

        Console.Write("Enter annual interest rate (%): ");
        if (!double.TryParse(Console.ReadLine(), out double rate) || rate <= 0)
        {
            Console.WriteLine("Error: Enter a valid rate.");
            return;
        }

        Console.Write("Enter time period (years): ");
        if (!int.TryParse(Console.ReadLine(), out int years) || years <= 0)
        {
            Console.WriteLine("Error: Enter a valid time period.");
            return;
        }

        Console.WriteLine("\nCompounding frequency:");
        Console.WriteLine("1. Annually");
        Console.WriteLine("2. Semi-annually");
        Console.WriteLine("3. Quarterly");
        Console.WriteLine("4. Monthly");
        Console.WriteLine("5. Daily");
        Console.Write("Choose (1-5): ");
        string freqChoice = Console.ReadLine();

        int n = freqChoice switch
        {
            "1" => 1,
            "2" => 2,
            "3" => 4,
            "4" => 12,
            "5" => 365,
            _ => 1
        };

        // A = P(1 + r/n)^(nt)
        double amount = principal * Math.Pow(1 + rate / 100 / n, n * years);
        double interest = amount - principal;
        double simpleInterest = principal * rate / 100 * years;

        Console.WriteLine("\n--- Results ---");
        Console.WriteLine($"  Principal:      {principal,12:N2}");
        Console.WriteLine($"  Rate:           {rate,12:F2}%");
        Console.WriteLine($"  Time:           {years,12} years");
        Console.WriteLine($"  Compounding:    {n,12} times/year");
        Console.WriteLine($"  Future Value:   {amount,12:N2}");
        Console.WriteLine($"  Interest Earned:{interest,12:N2}");
        Console.WriteLine($"  Simple Interest:{simpleInterest,12:N2}");
        Console.WriteLine($"  Extra from compounding: {interest - simpleInterest,12:N2}");

        Console.Write("\nView year-by-year growth? (y/n): ");
        if (Console.ReadLine().Trim().ToLower() == "y")
        {
            Console.WriteLine($"\n--- Year-by-Year Growth ---");
            Console.WriteLine($"  {"Year",-8} {"Amount",-16} {"Interest",-16} {"Bar"}");
            Console.WriteLine(new string('-', 60));

            double maxAmount = amount;
            for (int y = 1; y <= years; y++)
            {
                double yearAmount = principal * Math.Pow(1 + rate / 100 / n, n * y);
                double yearInterest = yearAmount - principal;
                int barLength = (int)(yearAmount / maxAmount * 30);
                string bar = new string('█', barLength);
                Console.WriteLine($"  {y,-8} {yearAmount,-16:N2} {yearInterest,-16:N2} {bar}");
            }
        }

        // Rule of 72
        double doublingTime = 72 / rate;
        Console.WriteLine($"\n--- Rule of 72 ---");
        Console.WriteLine($"  Your money doubles approximately every {doublingTime:F1} years at {rate}%");
    }
}
