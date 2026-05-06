// Program: Ref Struct and Ref Fields Demo
// Description: Demonstrates C# 11 ref struct with ref fields for high-performance scenarios

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Ref Struct & Ref Fields (C# 11) ===\n");

        // Basic ref struct usage
        int value = 42;
        var wrapper = new IntRefWrapper(ref value);

        Console.WriteLine("--- Ref Struct Basics ---");
        Console.WriteLine($"  Original value: {value}");
        Console.WriteLine($"  Through wrapper: {wrapper.Value}");

        wrapper.Value = 100;
        Console.WriteLine($"  After setting via wrapper: {value}");

        // Ref struct for string parsing (no heap allocation)
        Console.WriteLine($"\n--- Ref String Parser ---");
        string input = "Alice:30:Engineer,Bob:25:Designer,Charlie:35:Manager";
        var parser = new RecordParser(input);

        while (parser.TryGetNext(out var record))
        {
            Console.WriteLine($"  Name: {record.Name}, Age: {record.Age}, Role: {record.Role}");
        }

        // Ref struct for memory view
        Console.WriteLine($"\n--- Memory View ---");
        int[] data = { 10, 20, 30, 40, 50 };
        var view = new ArrayView<int>(data);

        Console.WriteLine($"  Original: [{string.Join(", ", data)}]");
        view.MultiplyAll(2);
        Console.WriteLine($"  After MultiplyAll(2): [{string.Join(", ", data)}]");

        // Ref struct with multiple ref fields
        Console.WriteLine($"\n--- Multiple Ref Fields ---");
        int a = 10, b = 20;
        var pair = new IntPair(ref a, ref b);

        Console.WriteLine($"  Before: a={a}, b={b}");
        pair.Swap();
        Console.WriteLine($"  After Swap: a={a}, b={b}");

        // Stack-only enforcement
        Console.WriteLine($"\n--- Stack-Only Enforcement ---");
        Console.WriteLine($"  Ref structs cannot be boxed or used as fields in regular classes.");
        Console.WriteLine($"  They can only exist on the stack, ensuring zero GC pressure.");
        UseRefStructOnStack();
    }

    static void UseRefStructOnStack()
    {
        int localValue = 999;
        var localWrapper = new IntRefWrapper(ref localValue);
        Console.WriteLine($"  Local value through ref struct: {localWrapper.Value}");
    }
}

// Simple ref struct holding a reference to an int
ref struct IntRefWrapper
{
    private ref int _value;

    public IntRefWrapper(ref int value)
    {
        _value = ref value;
    }

    public int Value
    {
        readonly get => _value;
        set => _value = value;
    }
}

// Ref struct for parsing records without allocations
ref struct RecordParser
{
    private ReadOnlySpan<char> _remaining;

    public RecordParser(string input)
    {
        _remaining = input.AsSpan();
    }

    public bool TryGetNext(out Record record)
    {
        record = default;

        if (_remaining.IsEmpty)
            return false;

        int commaIndex = _remaining.IndexOf(',');
        ReadOnlySpan<char> currentRecord;

        if (commaIndex >= 0)
        {
            currentRecord = _remaining.Slice(0, commaIndex);
            _remaining = _remaining.Slice(commaIndex + 1);
        }
        else
        {
            currentRecord = _remaining;
            _remaining = ReadOnlySpan<char>.Empty;
        }

        // Parse fields
        int colon1 = currentRecord.IndexOf(':');
        if (colon1 < 0) return false;

        int colon2 = currentRecord.Slice(colon1 + 1).IndexOf(':');
        if (colon2 < 0) return false;
        colon2 += colon1 + 1;

        record.Name = currentRecord.Slice(0, colon1).ToString();
        record.Age = int.Parse(currentRecord.Slice(colon1 + 1, colon2 - colon1 - 1));
        record.Role = currentRecord.Slice(colon2 + 1).ToString();

        return true;
    }
}

struct Record
{
    public string Name;
    public int Age;
    public string Role;
}

// Ref struct array view - modifies array in place
ref struct ArrayView<T> where T : unmanaged
{
    private Span<T> _data;

    public ArrayView(T[] array)
    {
        _data = array.AsSpan();
    }

    public readonly int Length => _data.Length;

    public ref T this[int index] => ref _data[index];

    public void MultiplyAll(T factor)
    {
        foreach (ref T item in _data)
        {
            item = (T)(dynamic)item * (dynamic)factor;
        }
    }
}

// Ref struct with ref fields (C# 11)
ref struct IntPair
{
    public ref int First;
    public ref int Second;

    public IntPair(ref int first, ref int second)
    {
        First = ref first;
        Second = ref second;
    }

    public void Swap()
    {
        int temp = First;
        First = Second;
        Second = temp;
    }
}
