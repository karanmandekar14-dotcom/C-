using System;

class Program
{
    static void Main()
    {
        int[] arr = {1,2,3,4};
        bool sorted = true;

        for(int i=0;i<arr.Length-1;i++)
            if(arr[i] > arr[i+1]) sorted = false;

        Console.WriteLine(sorted);
    }
}