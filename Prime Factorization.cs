// Program: Prime Factorization
// Description: Finds all prime factors of a number with visualization and analysis

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Prime Factorization ===");
        Console.WriteLine("Enter numbers to factorize (type 'exit' to quit):");

        while (true)
        {
            Console.Write("\nNumber: ");
            string input = Console.ReadLine();

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            if (!long.TryParse(input, out long n) || n <= 1)
            {
                Console.WriteLine("Error: Enter an integer greater than 1.");
                continue;
            }

            var factors = GetPrimeFactors(n);
            var factorCounts = new Dictionary<long, int>();
            foreach (long f in factors)
            {
                if (!factorCounts.ContainsKey(f))
                    factorCounts[f] = 0;
                factorCounts[f]++;
            }

            Console.WriteLine($"  Prime factors: {string.Join(", ", factors)}");
            Console.WriteLine($"  Factorization: {string.Join(" × ", factorCounts.Select(kvp => kvp.Value > 1 ? $"{kvp.Key}^{kvp.Value}" : $"{kvp.Key}"))}");
            Console.WriteLine($"  Number of factors: {factors.Count}");
            Console.WriteLine($"  Distinct primes: {factorCounts.Count}");
            Console.WriteLine($"  Largest prime factor: {factors[factors.Count - 1]}");
            Console.WriteLine($"  Sum of prime factors: {factors.Sum()}");

            // Check if it's a perfect power
            for (int exp = 2; exp <= 32; exp++)
            {
                double root = Math.Pow(n, 1.0 / exp);
                if (Math.Abs(root - Math.Round(root)) < 1e-9)
                {
                    Console.WriteLine($"  Perfect power: {Math.Round(root)}^{exp}");
                    break;
                }
            }
        }
    }

    static List<long> GetPrimeFactors(long n)
    {
        var factors = new List<long>();

        // Divide by 2
        while (n % 2 == 0)
        {
            factors.Add(2);
            n /= 2;
        }

        // Divide by odd numbers
        for (long i = 3; i <= Math.Sqrt(n); i += 2)
        {
            while (n % i == 0)
            {
                factors.Add(i);
                n /= i;
            }
        }

        if (n > 1)
            factors.Add(n);

        return factors;
    }
}
