// Program: Employee with Required Members
// Description: Demonstrates C# 11 'required' keyword for mandatory fields

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Employee Records (Required Members) ===\n");

        // Required members must be initialized
        var employees = new List<Employee>
        {
            new Employee
            {
                Id = 101,
                FirstName = "Alice",
                LastName = "Johnson",
                Department = "Engineering",
                Salary = 85000m,
                Email = "alice@company.com"
            },
            new Employee
            {
                Id = 102,
                FirstName = "Bob",
                LastName = "Smith",
                Department = "Marketing",
                Salary = 65000m,
                Email = "bob@company.com"
            },
            new Employee
            {
                Id = 103,
                FirstName = "Charlie",
                LastName = "Brown",
                Department = "Engineering",
                Salary = 92000m,
                Email = "charlie@company.com"
            }
        };

        // Display employees
        Console.WriteLine("--- Employee Directory ---");
        foreach (var emp in employees)
        {
            Console.WriteLine($"  {emp.Id} | {emp.FullName,-20} | {emp.Department,-15} | ${emp.Salary,10:N0} | {emp.Email}");
        }

        // Promotion with set-only property (not required)
        var alice = employees[0];
        alice.Promote("Senior Engineer", 95000m);
        Console.WriteLine($"\n--- After Promotion ---");
        Console.WriteLine($"  {alice.FullName}: {alice.Title} (${alice.Salary:N0})");

        // Department summary
        var byDept = new Dictionary<string, List<Employee>>();
        foreach (var emp in employees)
        {
            if (!byDept.ContainsKey(emp.Department))
                byDept[emp.Department] = new List<Employee>();
            byDept[emp.Department].Add(emp);
        }

        Console.WriteLine($"\n--- Department Summary ---");
        foreach (var dept in byDept)
        {
            double avgSalary = (double)dept.Value.Average(e => e.Salary);
            Console.WriteLine($"  {dept.Key,-15}: {dept.Value.Count} employees, avg salary: ${avgSalary:N0}");
        }

        // Note: Uncommenting the code below would cause a compile error:
        // var incomplete = new Employee(); // Error: Required member 'FirstName' must be set

        Console.WriteLine($"\n--- Required Members Benefit ---");
        Console.WriteLine($"  The compiler enures all required fields are set at compile time.");
        Console.WriteLine($"  No more null reference exceptions from missing required data!");
    }
}

class Employee
{
    public int Id { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Department { get; set; }
    public required decimal Salary { get; set; }
    public required string Email { get; init; }

    // Not required - has a default or is set later
    public string Title { get; private set; } = "Associate";

    public string FullName => $"{FirstName} {LastName}";

    public void Promote(string newTitle, decimal newSalary)
    {
        Title = newTitle;
        Salary = newSalary;
    }

    public override string ToString() => $"{FullName} ({Department})";
}
