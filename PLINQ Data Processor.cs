// Program: PLINQ Data Processor
// Description: Parallel LINQ for processing large datasets with performance comparison

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== PLINQ Data Processor ===\n");

        // Generate large dataset
        const int size = 10_000_000;
        Console.WriteLine($"Generating {size:N0} random numbers...");
        var rng = new Random(42);
        int[] data = new int[size];
        for (int i = 0; i < size; i++)
            data[i] = rng.Next(1, 1000);

        Console.WriteLine($"Dataset: {size:N0} integers (range 1-1000)\n");

        // 1. Sum comparison
        ComparePerformance("Sum",
            () => data.Sum(),
            () => data.AsParallel().Sum());

        // 2. Average comparison
        ComparePerformance("Average",
            () => data.Average(),
            () => data.AsParallel().Average());

        // 3. Count with predicate
        ComparePerformance("Count (even numbers)",
            () => data.Count(n => n % 2 == 0),
            () => data.AsParallel().Count(n => n % 2 == 0));

        // 4. Max/Min
        ComparePerformance("Max value",
            () => data.Max(),
            () => data.AsParallel().Max());

        // 5. Where + Count
        ComparePerformance("Count > 500",
            () => data.Count(n => n > 500),
            () => data.AsParallel().Count(n => n > 500));

        // 6. OrderBy (top 10)
        ComparePerformance("Top 10 largest",
            () => data.OrderByDescending(n => n).Take(10).ToArray(),
            () => data.AsParallel().OrderByDescending(n => n).Take(10).ToArray());

        // 7. GroupBy
        ComparePerformance("Group by value (count)",
            () => data.GroupBy(n => n).Count(),
            () => data.AsParallel().GroupBy(n => n).Count());

        // Statistical summary using PLINQ
        Console.WriteLine($"\n--- Statistical Summary (PLINQ) ---");
        var stats = data.AsParallel()
            .GroupBy(n => n switch
            {
                < 250 => "Low",
                < 500 => "Medium",
                < 750 => "High",
                _ => "Very High"
            })
            .OrderBy(g => g.Key)
            .Select(g => new { Range = g.Key, Count = g.Count(), Avg = g.Average() })
            .ToList();

        foreach (var s in stats)
        {
            string bar = new string('█', s.Count / 50000);
            Console.WriteLine($"  {s.Range,-12}: {s.Count,8:N0} items (avg: {s.Avg:F1}) {bar}");
        }
    }

    static void ComparePerformance<T>(string operation, Func<T> sequential, Func<T> parallel)
    {
        var sw = Stopwatch.StartNew();
        var seqResult = sequential();
        sw.Stop();
        long seqTime = sw.ElapsedMilliseconds;

        sw.Restart();
        var parResult = parallel();
        sw.Stop();
        long parTime = sw.ElapsedMilliseconds;

        double speedup = parTime > 0 ? (double)seqTime / parTime : double.PositiveInfinity;
        string resultStr = seqResult?.ToString() ?? "(result)";
        if (resultStr.Length > 40)
            resultStr = resultStr.Substring(0, 40) + "...";

        Console.WriteLine($"  {operation,-30} Seq: {seqTime,5}ms | Par: {parTime,5}ms | Speedup: {speedup:F1}x");
    }
}
