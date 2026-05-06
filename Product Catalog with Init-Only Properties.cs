// Program: Product Catalog with Init-Only Properties
// Description: Demonstrates C# init-only properties for safe object construction

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Product Catalog (Init-Only Properties) ===\n");

        // Object initializer with init-only properties
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 999.99m,
                Category = "Electronics",
                CreatedAt = DateTime.Now,
                IsActive = true
            },
            new Product
            {
                Id = 2,
                Name = "Headphones",
                Price = 49.99m,
                Category = "Electronics",
                CreatedAt = DateTime.Now,
                IsActive = true
            },
            new Product
            {
                Id = 3,
                Name = "Coffee Mug",
                Price = 12.99m,
                Category = "Kitchen",
                CreatedAt = DateTime.Now,
                IsActive = true
            },
            new Product
            {
                Id = 4,
                Name = "Notebook",
                Price = 5.99m,
                Category = "Stationery",
                CreatedAt = DateTime.Now,
                IsActive = false // Discontinued
            }
        };

        // Display all products
        Console.WriteLine("--- All Products ---");
        foreach (var product in products)
        {
            Console.WriteLine($"  {product.Id}. {product.Name,-15} ${product.Price,8:F2} [{product.Category}] {(product.IsActive ? "" : "(Discontinued)")}");
        }

        // Demonstrate that init-only properties can't be changed after construction
        var laptop = products[0];
        Console.WriteLine($"\n--- Immutability Check ---");
        Console.WriteLine($"  Original laptop price: ${laptop.Price}");

        // Uncommenting the line below would cause a compile error:
        // laptop.Price = 899.99m; // CS8852: Init-only property can only be set in an object initializer

        // But we can create a modified copy using 'with' expression
        var discountedLaptop = laptop with { Price = 899.99m };
        Console.WriteLine($"  Discounted laptop price: ${discountedLaptop.Price}");
        Console.WriteLine($"  Original unchanged: ${laptop.Price}");

        // Filter using LINQ
        var activeElectronics = products.Where(p => p.IsActive && p.Category == "Electronics").ToList();
        Console.WriteLine($"\n--- Active Electronics ---");
        foreach (var product in activeElectronics)
        {
            Console.WriteLine($"  {product.Name}: ${product.Price}");
        }

        // Statistics
        var avgPrice = products.Where(p => p.IsActive).Average(p => p.Price);
        var mostExpensive = products.Where(p => p.IsActive).OrderByDescending(p => p.Price).First();

        Console.WriteLine($"\n--- Statistics ---");
        Console.WriteLine($"  Average price (active): ${avgPrice:F2}");
        Console.WriteLine($"  Most expensive: {mostExpensive.Name} (${mostExpensive.Price})");
        Console.WriteLine($"  Total products: {products.Count}");
        Console.WriteLine($"  Active products: {products.Count(p => p.IsActive)}");
    }
}

class Product
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public string Category { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool IsActive { get; init; }

    public override string ToString() => $"{Name} (${Price})";
}
