// Program: Combination and Permutation Calculator
// Description: Calculates nCr, nPr, and related combinatorial values with factorial support

using System;
using System.Numerics;

class Program
{
    static void Main()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n=== Combination & Permutation Calculator ===");
            Console.WriteLine("1. Permutation (nPr)");
            Console.WriteLine("2. Combination (nCr)");
            Console.WriteLine("3. Factorial (n!)");
            Console.WriteLine("4. Compare nPr vs nCr");
            Console.WriteLine("5. List all combinations");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": CalcPermutation(); break;
                case "2": CalcCombination(); break;
                case "3": CalcFactorial(); break;
                case "4": CompareBoth(); break;
                case "5": ListCombinations(); break;
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

    static void CalcPermutation()
    {
        Console.Write("Enter n (total items): ");
        if (!int.TryParse(Console.ReadLine(), out int n) || n < 0) return;
        Console.Write("Enter r (items to arrange): ");
        if (!int.TryParse(Console.ReadLine(), out int r) || r < 0 || r > n)
        {
            Console.WriteLine("Error: r must be between 0 and n.");
            return;
        }

        BigInteger result = Factorial(n) / Factorial(n - r);
        Console.WriteLine($"  P({n},{r}) = {result}");
    }

    static void CalcCombination()
    {
        Console.Write("Enter n (total items): ");
        if (!int.TryParse(Console.ReadLine(), out int n) || n < 0) return;
        Console.Write("Enter r (items to choose): ");
        if (!int.TryParse(Console.ReadLine(), out int r) || r < 0 || r > n)
        {
            Console.WriteLine("Error: r must be between 0 and n.");
            return;
        }

        BigInteger result = Factorial(n) / (Factorial(r) * Factorial(n - r));
        Console.WriteLine($"  C({n},{r}) = {result}");
    }

    static void CalcFactorial()
    {
        Console.Write("Enter n: ");
        if (!int.TryParse(Console.ReadLine(), out int n) || n < 0) return;

        BigInteger result = Factorial(n);
        Console.WriteLine($"  {n}! = {result}");
        Console.WriteLine($"  Digits: {result.ToString().Length}");
    }

    static void CompareBoth()
    {
        Console.Write("Enter n: ");
        if (!int.TryParse(Console.ReadLine(), out int n) || n < 0) return;
        Console.Write("Enter r: ");
        if (!int.TryParse(Console.ReadLine(), out int r) || r < 0 || r > n)
        {
            Console.WriteLine("Error: r must be between 0 and n.");
            return;
        }

        BigInteger perm = Factorial(n) / Factorial(n - r);
        BigInteger comb = Factorial(n) / (Factorial(r) * Factorial(n - r));

        Console.WriteLine($"\n  n = {n}, r = {r}");
        Console.WriteLine($"  nPr = {perm}");
        Console.WriteLine($"  nCr = {comb}");
        Console.WriteLine($"  nPr / nCr = {r}! = {Factorial(r)}");
        Console.WriteLine($"\n  Explanation: Order matters in nPr ({perm} arrangements)");
        Console.WriteLine($"               Order doesn't matter in nCr ({comb} selections)");
    }

    static void ListCombinations()
    {
        Console.Write("Enter items (comma-separated, e.g., A,B,C,D): ");
        string input = Console.ReadLine();
        string[] items = input.Split(',');

        Console.Write("Choose r (items per combination): ");
        if (!int.TryParse(Console.ReadLine(), out int r) || r < 0 || r > items.Length)
        {
            Console.WriteLine("Error: Invalid r.");
            return;
        }

        var combos = GetCombinations(items, r);
        Console.WriteLine($"\nAll C({items.Length},{r}) = {combos.Count} combinations:");
        int idx = 1;
        foreach (var combo in combos)
        {
            Console.WriteLine($"  {idx,3}. {string.Join(", ", combo)}");
            idx++;
        }
    }

    static BigInteger Factorial(int n)
    {
        BigInteger result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    static System.Collections.Generic.List<string[]> GetCombinations(string[] items, int r)
    {
        var result = new System.Collections.Generic.List<string[]>();
        Combine(items, r, 0, new string[r], result);
        return result;
    }

    static void Combine(string[] items, int r, int start, string[] current, System.Collections.Generic.List<string[]> result)
    {
        if (r == 0)
        {
            result.Add((string[])current.Clone());
            return;
        }

        for (int i = start; i <= items.Length - r; i++)
        {
            current[current.Length - r] = items[i];
            Combine(items, r - 1, i + 1, current, result);
        }
    }
}
