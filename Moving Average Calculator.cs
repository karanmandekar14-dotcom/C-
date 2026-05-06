// Program: Moving Average Calculator
// Description: Calculates simple, weighted, and exponential moving averages for datasets

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Moving Average Calculator ===");
        Console.WriteLine("Enter data values (comma-separated numbers): ");
        string input = Console.ReadLine();
        string[] parts = input.Split(',');
        var data = new List<double>();

        foreach (string part in parts)
        {
            if (double.TryParse(part.Trim(), out double v))
                data.Add(v);
        }

        if (data.Count < 2)
        {
            Console.WriteLine("Error: Enter at least 2 values.");
            return;
        }

        Console.Write("Enter window size: ");
        if (!int.TryParse(Console.ReadLine(), out int window) || window < 2 || window > data.Count)
        {
            Console.WriteLine("Error: Window must be between 2 and data count.");
            return;
        }

        Console.Write("Enter smoothing factor for EMA (0-1, e.g., 0.2): ");
        if (!double.TryParse(Console.ReadLine(), out double alpha) || alpha <= 0 || alpha > 1)
        {
            alpha = 0.2;
        }

        var sma = CalcSMA(data, window);
        var wma = CalcWMA(data, window);
        var ema = CalcEMA(data, alpha);

        Console.WriteLine($"\n--- Original Data ---");
        Console.WriteLine($"  {string.Join(", ", data.Select(d => d.ToString("F2")))}");

        Console.WriteLine($"\n--- Simple Moving Average (SMA, window={window}) ---");
        PrintWithPadding("SMA", sma);

        Console.WriteLine($"\n--- Weighted Moving Average (WMA, window={window}) ---");
        PrintWithPadding("WMA", wma);

        Console.WriteLine($"\n--- Exponential Moving Average (EMA, α={alpha}) ---");
        PrintWithPadding("EMA", ema);

        // Comparison
        Console.WriteLine($"\n--- Comparison (last values) ---");
        Console.WriteLine($"  SMA: {sma.LastOrDefault():F4}");
        Console.WriteLine($"  WMA: {wma.LastOrDefault():F4}");
        Console.WriteLine($"  EMA: {ema.Last():F4}");
        Console.WriteLine($"  Current: {data.Last():F4}");

        // Trend analysis
        if (ema.Count >= 2)
        {
            double trend = ema.Last() - ema[ema.Count - 2];
            Console.WriteLine($"  Trend: {(trend > 0 ? "Bullish ↑" : trend < 0 ? "Bearish ↓" : "Neutral →")}");
        }
    }

    static List<double> CalcSMA(List<double> data, int window)
    {
        var result = new List<double>();
        for (int i = 0; i <= data.Count - window; i++)
        {
            double sum = 0;
            for (int j = i; j < i + window; j++)
                sum += data[j];
            result.Add(sum / window);
        }
        return result;
    }

    static List<double> CalcWMA(List<double> data, int window)
    {
        var result = new List<double>();
        for (int i = 0; i <= data.Count - window; i++)
        {
            double sum = 0, weightSum = 0;
            for (int j = 0; j < window; j++)
            {
                double weight = j + 1;
                sum += data[i + j] * weight;
                weightSum += weight;
            }
            result.Add(sum / weightSum);
        }
        return result;
    }

    static List<double> CalcEMA(List<double> data, double alpha)
    {
        var result = new List<double> { data[0] };
        for (int i = 1; i < data.Count; i++)
        {
            double ema = alpha * data[i] + (1 - alpha) * result.Last();
            result.Add(ema);
        }
        return result;
    }

    static void PrintWithPadding(string label, List<double> values)
    {
        Console.WriteLine($"  {label}: {string.Join(", ", values.Select(v => v.ToString("F2")))}");
    }
}
