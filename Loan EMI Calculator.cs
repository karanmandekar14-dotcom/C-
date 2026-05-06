// Program: Loan EMI Calculator
// Description: Calculates monthly EMI, total interest, and generates amortization schedule

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Loan EMI Calculator ===");

        Console.Write("Enter loan amount: ");
        if (!double.TryParse(Console.ReadLine(), out double principal) || principal <= 0)
        {
            Console.WriteLine("Error: Enter a valid loan amount.");
            return;
        }

        Console.Write("Enter annual interest rate (%): ");
        if (!double.TryParse(Console.ReadLine(), out double annualRate) || annualRate <= 0)
        {
            Console.WriteLine("Error: Enter a valid interest rate.");
            return;
        }

        Console.Write("Enter loan tenure (years): ");
        if (!int.TryParse(Console.ReadLine(), out int years) || years <= 0)
        {
            Console.WriteLine("Error: Enter a valid tenure.");
            return;
        }

        int months = years * 12;
        double monthlyRate = annualRate / 12 / 100;

        // EMI = P * r * (1+r)^n / ((1+r)^n - 1)
        double emi = principal * monthlyRate * Math.Pow(1 + monthlyRate, months) / (Math.Pow(1 + monthlyRate, months) - 1);
        double totalPayment = emi * months;
        double totalInterest = totalPayment - principal;

        Console.WriteLine("\n--- Loan Summary ---");
        Console.WriteLine($"  Loan Amount:    {principal:N2}");
        Console.WriteLine($"  Interest Rate:  {annualRate:F2}% per year");
        Console.WriteLine($"  Tenure:         {years} years ({months} months)");
        Console.WriteLine($"  Monthly EMI:    {emi:N2}");
        Console.WriteLine($"  Total Payment:  {totalPayment:N2}");
        Console.WriteLine($"  Total Interest: {totalInterest:N2}");
        Console.WriteLine($"  Interest/Principal Ratio: {totalInterest / principal * 100:F1}%");

        Console.Write("\nView amortization schedule? (y/n): ");
        if (Console.ReadLine().Trim().ToLower() == "y")
        {
            Console.WriteLine("\n--- Amortization Schedule ---");
            Console.WriteLine($"  {"Month",-8} {"EMI",-12} {"Principal",-14} {"Interest",-12} {"Balance",-14}");
            Console.WriteLine(new string('-', 62));

            double balance = principal;
            for (int m = 1; m <= months; m++)
            {
                double interestPart = balance * monthlyRate;
                double principalPart = emi - interestPart;
                balance -= principalPart;

                if (balance < 0) balance = 0;

                Console.WriteLine($"  {m,-8} {emi,-12:F2} {principalPart,-14:F2} {interestPart,-12:F2} {balance,-14:F2}");
            }
        }
    }
}
