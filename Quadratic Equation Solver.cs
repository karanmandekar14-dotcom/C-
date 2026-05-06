// Program: Quadratic Equation Solver
// Description: Solves ax² + bx + c = 0 with real and complex roots, graph info

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Quadratic Equation Solver ===");
        Console.WriteLine("Solving: ax² + bx + c = 0\n");

        Console.Write("Enter a (coefficient of x²): ");
        if (!double.TryParse(Console.ReadLine(), out double a) || a == 0)
        {
            Console.WriteLine("Error: 'a' cannot be zero (not quadratic).");
            return;
        }

        Console.Write("Enter b (coefficient of x): ");
        if (!double.TryParse(Console.ReadLine(), out double b)) return;

        Console.Write("Enter c (constant): ");
        if (!double.TryParse(Console.ReadLine(), out double c)) return;

        double discriminant = b * b - 4 * a * c;
        double vertexX = -b / (2 * a);
        double vertexY = a * vertexX * vertexX + b * vertexX + c;

        Console.WriteLine($"\n--- Equation ---");
        Console.WriteLine($"  {FormatCoefficient(a, "x²")} {FormatSign(b)} {FormatCoefficient(Math.Abs(b), "x")} {FormatSign(c)} {FormatCoefficient(Math.Abs(c), "")} = 0");

        Console.WriteLine($"\n--- Discriminant ---");
        Console.WriteLine($"  D = b² - 4ac = {discriminant:F4}");

        if (discriminant > 0)
        {
            double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            Console.WriteLine($"  D > 0: Two distinct real roots");
            Console.WriteLine($"  x₁ = {root1:F6}");
            Console.WriteLine($"  x₂ = {root2:F6}");
        }
        else if (discriminant == 0)
        {
            double root = -b / (2 * a);
            Console.WriteLine($"  D = 0: One repeated real root");
            Console.WriteLine($"  x = {root:F6}");
        }
        else
        {
            double realPart = -b / (2 * a);
            double imagPart = Math.Sqrt(-discriminant) / (2 * a);
            Console.WriteLine($"  D < 0: Two complex conjugate roots");
            Console.WriteLine($"  x₁ = {realPart:F6} + {imagPart:F6}i");
            Console.WriteLine($"  x₂ = {realPart:F6} - {imagPart:F6}i");
        }

        Console.WriteLine($"\n--- Parabola Properties ---");
        Console.WriteLine($"  Vertex: ({vertexX:F4}, {vertexY:F4})");
        Console.WriteLine($"  Axis of symmetry: x = {vertexX:F4}");
        Console.WriteLine($"  Opens: {(a > 0 ? "Upward (minimum)" : "Downward (maximum)")}");
        Console.WriteLine($"  Y-intercept: (0, {c})");

        // X-intercepts
        if (discriminant >= 0)
        {
            double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            if (discriminant == 0)
                Console.WriteLine($"  X-intercept: ({root1:F4}, 0)");
            else
                Console.WriteLine($"  X-intercepts: ({root1:F4}, 0) and ({root2:F4}, 0)");
        }
        else
        {
            Console.WriteLine($"  X-intercepts: None (parabola doesn't cross x-axis)");
        }

        // Sum and product of roots
        double sumOfRoots = -b / a;
        double productOfRoots = c / a;
        Console.WriteLine($"\n--- Root Properties ---");
        Console.WriteLine($"  Sum of roots: {sumOfRoots:F4} (= -b/a)");
        Console.WriteLine($"  Product of roots: {productOfRoots:F4} (= c/a)");
    }

    static string FormatCoefficient(double val, string variable)
    {
        if (val == 1 && !string.IsNullOrEmpty(variable)) return variable;
        if (val == 0) return "0";
        return $"{val:F2}{variable}".Trim();
    }

    static string FormatSign(double val)
    {
        return val >= 0 ? "+" : "-";
    }
}
