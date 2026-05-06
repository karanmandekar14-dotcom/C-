// Program: Expression Trees Demo
// Description: Demonstrates building and compiling expression trees for dynamic queries

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Expression Trees Demo ===\n");

        // 1. Simple expression tree
        Console.WriteLine("--- Simple Expression Tree ---");
        Expression<Func<int, int, int>> addExpr = (a, b) => a + b;
        Console.WriteLine($"  Expression: {addExpr}");
        Console.WriteLine($"  Type: {addExpr.NodeType}");

        Func<int, int, int> addFunc = addExpr.Compile();
        Console.WriteLine($"  Compiled: addFunc(3, 4) = {addFunc(3, 4)}");

        // 2. Building expression trees manually
        Console.WriteLine($"\n--- Manual Expression Tree ---");
        ParameterExpression paramA = Expression.Parameter(typeof(int), "a");
        ParameterExpression paramB = Expression.Parameter(typeof(int), "b");

        // (a + b) * 2
        BinaryExpression add = Expression.Add(paramA, paramB);
        ConstantExpression two = Expression.Constant(2, typeof(int));
        BinaryExpression multiply = Expression.Multiply(add, two);

        var lambda = Expression.Lambda<Func<int, int, int>>(multiply, paramA, paramB);
        Console.WriteLine($"  Expression: {lambda}");
        var compiled = lambda.Compile();
        Console.WriteLine($"  Result: compiled(5, 3) = {compiled(5, 3)}");

        // 3. Dynamic filtering with expression trees
        Console.WriteLine($"\n--- Dynamic Query Builder ---");
        var products = new List<Product>
        {
            new("Laptop", 999.99m, "Electronics"),
            new("Phone", 699.99m, "Electronics"),
            new("Desk", 249.99m, "Furniture"),
            new("Chair", 149.99m, "Furniture"),
            new("Monitor", 399.99m, "Electronics"),
            new("Keyboard", 79.99m, "Electronics")
        };

        // Build: p => p.Price > 200 && p.Category == "Electronics"
        var dynamicFilter = BuildFilter<Product>(
            BuildGreaterThan<Product>("Price", 200m),
            BuildEqual<Product>("Category", "Electronics")
        );

        var filtered = products.Where(dynamicFilter.Compile()).ToList();
        Console.WriteLine($"  Filter: Price > 200 AND Category == 'Electronics'");
        foreach (var p in filtered)
            Console.WriteLine($"    {p.Name}: ${p.Price}");

        // 4. Dynamic sorting
        Console.WriteLine($"\n--- Dynamic Sorting ---");
        var sortByNameAsc = BuildPropertySelector<Product, string>("Name");
        var sortByPriceDesc = BuildPropertySelector<Product, decimal>("Price");

        var sortedByName = products.OrderBy(sortByNameAsc.Compile()).ToList();
        var sortedByPrice = products.OrderByDescending(sortByPriceDesc.Compile()).ToList();

        Console.WriteLine($"  Sorted by Name:");
        foreach (var p in sortedByName)
            Console.WriteLine($"    {p.Name}: ${p.Price}");

        Console.WriteLine($"\n  Sorted by Price (desc):");
        foreach (var p in sortedByPrice)
            Console.WriteLine($"    {p.Name}: ${p.Price}");

        // 5. Building arithmetic expression
        Console.WriteLine($"\n--- Arithmetic Expression Builder ---");
        // Build: x => x * x + 2 * x + 1
        var quadraticExpr = BuildQuadraticExpression();
        Console.WriteLine($"  Expression: {quadraticExpr}");
        var quadraticFunc = quadraticExpr.Compile();
        for (int x = 0; x <= 5; x++)
        {
            Console.WriteLine($"  f({x}) = {quadraticFunc(x)}");
        }
    }

    static Expression<Func<T, bool>> BuildFilter<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var param = Expression.Parameter(typeof(T), "x");

        var body = Expression.AndAlso(
            Expression.Invoke(expr1, param),
            Expression.Invoke(expr2, param)
        );

        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    static Expression<Func<T, bool>> BuildGreaterThan<T>(string propertyName, decimal value)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(param, propertyName);
        var constant = Expression.Constant(value, typeof(decimal));
        var comparison = Expression.GreaterThan(property, constant);
        return Expression.Lambda<Func<T, bool>>(comparison, param);
    }

    static Expression<Func<T, bool>> BuildEqual<T>(string propertyName, string value)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(param, propertyName);
        var constant = Expression.Constant(value, typeof(string));
        var comparison = Expression.Equal(property, constant);
        return Expression.Lambda<Func<T, bool>>(comparison, param);
    }

    static Expression<Func<T, TKey>> BuildPropertySelector<T, TKey>(string propertyName)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(param, propertyName);
        return Expression.Lambda<Func<T, TKey>>(property, param);
    }

    static Expression<Func<int, int>> BuildQuadraticExpression()
    {
        var x = Expression.Parameter(typeof(int), "x");
        var xSquared = Expression.Multiply(x, x);
        var twoX = Expression.Multiply(Expression.Constant(2), x);
        var one = Expression.Constant(1);

        var body = Expression.Add(Expression.Add(xSquared, twoX), one);
        return Expression.Lambda<Func<int, int>>(body, x);
    }
}

record Product(string Name, decimal Price, string Category);
