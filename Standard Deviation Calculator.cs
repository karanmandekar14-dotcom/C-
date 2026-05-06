// Program: Standard Deviation Calculator
// Description: Calculates mean, variance, standard deviation, and statistical summary of a dataset

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Standard Deviation Calculator ===");
        Console.WriteLine("Enter numbers (comma-separated): ");
        string input = Console.ReadLine();
        string[] parts = input.Split(',');
        var values = new List<double>();

        foreach (string part in parts)
        {
            if (double.TryParse(part.Trim(), out double v))
                values.Add(v);
        }

        if (values.Count < 2)
        {
            Console.WriteLine("Error: Enter at least 2 numbers.");
            return;
        }

        int n = values.Count;
        double mean = values.Average();
        double sumSquaredDiff = values.Sum(v => Math.Pow(v - mean, 2));
        double variance = sumSquaredDiff / (n - 1);       // Sample variance
        double stdDev = Math.Sqrt(variance);               // Sample std dev
        double popVariance = sumSquaredDiff / n;           // Population variance
        double popStdDev = Math.Sqrt(popVariance);         // Population std dev
        double median = GetMedian(values);
        double range = values.Max() - values.Min();
        double coefficientOfVariation = (stdDev / Math.Abs(mean)) * 100;

        Console.WriteLine("\n--- Statistical Summary ---");
        Console.WriteLine($"  Count:              {n}");
        Console.WriteLine($"  Sum:                {values.Sum():N2}");
        Console.WriteLine($"  Mean:               {mean:N4}");
        Console.WriteLine($"  Median:             {median:N4}");
        Console.WriteLine($"  Mode:               {GetMode(values):N4}");
        Console.WriteLine($"  Min:                {values.Min():N4}");
        Console.WriteLine($"  Max:                {values.Max():N4}");
        Console.WriteLine($"  Range:              {range:N4}");
        Console.WriteLine();
        Console.WriteLine($"  Sample Variance:    {variance:N4}");
        Console.WriteLine($"  Sample Std Dev:     {stdDev:N4}");
        Console.WriteLine($"  Population Variance:{popVariance:N4}");
        Console.WriteLine($"  Population Std Dev: {popStdDev:N4}");
        Console.WriteLine($"  Coeff. of Variation:{coefficientOfVariation:F2}%");

        // Distribution visualization
        Console.WriteLine($"\n--- Distribution ---");
        int barWidth = 40;
        foreach (double v in values.OrderBy(x => x))
        {
            double deviation = (v - mean) / stdDev;
            string bar = new string('#', Math.Max(1, (int)(Math.Abs(deviation) / 3 * barWidth)));
            string side = deviation >= 0 ? "above" : "below";
            Console.WriteLine($"  {v,10:N2} | {bar} ({Math.Abs(deviation):F2} σ {side} mean)");
        }
    }

    static double GetMedian(List<double> values)
    {
        var sorted = values.OrderBy(x => x).ToList();
        int mid = sorted.Count / 2;
        return sorted.Count % 2 == 0
            ? (sorted[mid - 1] + sorted[mid]) / 2.0
            : sorted[mid];
    }

    static double GetMode(List<double> values)
    {
        return values.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;
    }
}
