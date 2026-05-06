using System;
using System.Linq;

class Program
{
    static void Main()
    {
        int[] arr={1,2,3,1};
        Console.WriteLine(arr.Length != arr.Distinct().Count());
    }
}