// Program: Currency Converter
// Description: Converts between currencies using user-defined exchange rates

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Currency Converter ===");

        // Default exchange rates (relative to USD)
        var rates = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "USD", 1.0 },
            { "EUR", 0.92 },
            { "GBP", 0.79 },
            { "JPY", 149.50 },
            { "INR", 83.12 },
            { "CNY", 7.24 },
            { "AUD", 1.53 },
            { "CAD", 1.36 },
            { "CHF", 0.88 },
            { "AED", 3.67 },
            { "SGD", 1.34 },
            { "KRW", 1320.0 },
            { "BRL", 5.05 },
            { "MXN", 17.15 },
            { "ZAR", 19.05 }
        };

        Console.WriteLine("Available currencies: " + string.Join(", ", rates.Keys));
        Console.Write("\nConvert from (currency code): ");
        string from = Console.ReadLine();

        if (!rates.ContainsKey(from))
        {
            Console.WriteLine($"Error: Currency '{from}' not found.");
            return;
        }

        Console.Write("Convert to (currency code): ");
        string to = Console.ReadLine();

        if (!rates.ContainsKey(to))
        {
            Console.WriteLine($"Error: Currency '{to}' not found.");
            return;
        }

        Console.Write($"Enter amount in {from}: ");
        if (!double.TryParse(Console.ReadLine(), out double amount) || amount < 0)
        {
            Console.WriteLine("Error: Enter a valid amount.");
            return;
        }

        // Convert: amount -> USD -> target
        double inUSD = amount / rates[from];
        double result = inUSD * rates[to];
        double exchangeRate = rates[to] / rates[from];

        Console.WriteLine($"\n--- Conversion Result ---");
        Console.WriteLine($"  From: {amount:N2} {from}");
        Console.WriteLine($"  To:   {result:N2} {to}");
        Console.WriteLine($"  Rate: 1 {from} = {exchangeRate:F4} {to}");
        Console.WriteLine($"  Rate: 1 {to} = {1 / exchangeRate:F4} {from}");

        // Multi-currency view
        Console.WriteLine($"\n--- {amount} {from} in all currencies ---");
        foreach (var currency in rates)
        {
            double converted = inUSD * currency.Value;
            string bar = new string('█', Math.Min((int)(converted / rates[currency.Key == "USD" ? "USD" : currency.Key] * 5), 40));
            Console.WriteLine($"  {currency.Key,4}: {converted,12:N2} {bar}");
        }
    }
}
