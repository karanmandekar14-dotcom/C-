// Program: Student Record with Records
// Description: Demonstrates C# record types for immutable data models with equality

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Student Records (C# Records) ===");

        // Create records
        var student1 = new Student("Alice", 20, "Computer Science", new[] { "Math", "Physics", "CS101" });
        var student2 = new Student("Bob", 22, "Mathematics", new[] { "Calculus", "Statistics", "Algebra" });
        var student3 = student1 with { Name = "Charlie" }; // Non-destructive mutation

        Console.WriteLine("--- Creating Students ---");
        Console.WriteLine(student1);
        Console.WriteLine(student2);
        Console.WriteLine(student3);

        // Value-based equality
        var student4 = new Student("Alice", 20, "Computer Science", new[] { "Math", "Physics", "CS101" });
        Console.WriteLine($"\n--- Equality Check ---");
        Console.WriteLine($"  student1 == student4: {student1 == student4}"); // True (value equality)
        Console.WriteLine($"  student1 == student2: {student1 == student2}"); // False
        Console.WriteLine($"  ReferenceEquals(student1, student4): {ReferenceEquals(student1, student4)}"); // False

        // With expression
        var updatedStudent = student1 with { Age = 21, Major = "Data Science" };
        Console.WriteLine($"\n--- With Expression ---");
        Console.WriteLine($"  Original:  {student1}");
        Console.WriteLine($"  Updated:   {updatedStudent}");

        // Deconstruction
        var (name, age, major) = student1;
        Console.WriteLine($"\n--- Deconstruction ---");
        Console.WriteLine($"  Name: {name}, Age: {age}, Major: {major}");

        // Record struct
        var point1 = new Point2D(3.5, 7.2);
        var point2 = new Point2D(3.5, 7.2);
        Console.WriteLine($"\n--- Record Struct ---");
        Console.WriteLine($"  point1: {point1}");
        Console.WriteLine($"  point2: {point2}");
        Console.WriteLine($"  point1 == point2: {point1 == point2}");

        // Collection of records
        var students = new List<Student> { student1, student2, student3 };
        students.Sort((a, b) => a.Name.CompareTo(b.Name));
        Console.WriteLine($"\n--- Sorted by Name ---");
        foreach (var s in students)
            Console.WriteLine($"  {s.Name,-10} Age: {s.Age,-3} Major: {s.Major}");
    }
}

record Student(string Name, int Age, string Major, string[] Courses)
{
    public override string ToString() =>
        $"Student({Name}, {Age}, {Major}, Courses: {Courses.Length})";
}

readonly record struct Point2D(double X, double Y);
