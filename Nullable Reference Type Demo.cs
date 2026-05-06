// Program: Nullable Reference Type Demo
// Description: Demonstrates safe nullable handling in a data processing pipeline

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Nullable Reference Types Demo ===\n");

        // Simulating data from an external source (e.g., database, API)
        var rawData = new List<Dictionary<string, string?>>
        {
            new() { ["Name"] = "Alice", ["Email"] = "alice@example.com", ["Phone"] = null, ["Age"] = "30" },
            new() { ["Name"] = null, ["Email"] = "bob@example.com", ["Phone"] = "555-0102", ["Age"] = null },
            new() { ["Name"] = "Charlie", ["Email"] = null, ["Phone"] = "555-0103", ["Age"] = "25" },
            new() { ["Name"] = "Diana", ["Email"] = "diana@example.com", ["Phone"] = null, ["Age"] = "35" },
            new() { ["Name"] = null, ["Email"] = null, ["Phone"] = null, ["Age"] = null }
        };

        Console.WriteLine("--- Processing Nullable Data ---\n");

        var processedUsers = new List<User>();

        foreach (var record in rawData)
        {
            // Safe nullable handling with pattern matching
            string? name = record.GetValueOrDefault("Name");
            string? email = record.GetValueOrDefault("Email");
            string? phone = record.GetValueOrDefault("Phone");
            string? ageStr = record.GetValueOrDefault("Age");

            // Using null-coalescing for defaults
            string displayName = name ?? "Unknown";
            int? age = int.TryParse(ageStr, out int parsedAge) ? parsedAge : null;

            // Using null-conditional operators
            string contactInfo = email ?? phone ?? "No contact info";

            // Using null-forgiving operator (when we're sure it's not null)
            if (name is not null)
            {
                var user = new User
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    Age = age
                };
                processedUsers.Add(user);
            }
            else
            {
                Console.WriteLine($"  ⚠ Skipping record: Name is null");
            }
        }

        Console.WriteLine("\n--- Processed Users ---");
        foreach (var user in processedUsers)
        {
            Console.WriteLine($"  Name: {user.Name}");
            Console.WriteLine($"    Email: {user.Email ?? "N/A"}");
            Console.WriteLine($"    Phone: {user.Phone ?? "N/A"}");
            Console.WriteLine($"    Age: {(user.Age?.ToString() ?? "Unknown")}");
            Console.WriteLine();
        }

        // Nullable analysis with LINQ
        Console.WriteLine("--- Nullable LINQ Operations ---");
        var usersWithEmail = processedUsers.Where(u => u.Email != null).ToList();
        var usersWithAge = processedUsers.Where(u => u.Age.HasValue).ToList();
        double? avgAge = usersWithAge.Count > 0
            ? usersWithAge.Average(u => u.Age!.Value)
            : null;

        Console.WriteLine($"  Users with email: {usersWithEmail.Count}");
        Console.WriteLine($"  Users with age: {usersWithAge.Count}");
        Console.WriteLine($"  Average age: {(avgAge?.ToString("F1") ?? "N/A")}");

        // Pattern matching for null
        Console.WriteLine("\n--- Null Pattern Matching ---");
        foreach (var user in processedUsers)
        {
            string status = user switch
            {
                { Email: not null, Phone: not null } => "Fully contactable",
                { Email: not null } => "Email only",
                { Phone: not null } => "Phone only",
                _ => "Unreachable (we filtered out names)"
            };
            Console.WriteLine($"  {user.Name}: {status}");
        }
    }
}

class User
{
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public int? Age { get; init; }
}
