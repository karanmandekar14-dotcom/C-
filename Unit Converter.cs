// Program: Unit Converter
// Description: Converts between length, weight, temperature, and volume units with a menu

using System;

class Program
{
    static void Main()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n=== Unit Converter ===");
            Console.WriteLine("1. Length (m, km, miles, feet, inches, cm)");
            Console.WriteLine("2. Weight (kg, lbs, grams, ounces)");
            Console.WriteLine("3. Temperature (Celsius, Fahrenheit, Kelvin)");
            Console.WriteLine("4. Volume (Liters, Gallons, Milliliters, Cups)");
            Console.WriteLine("0. Exit");
            Console.Write("Choose a category: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ConvertLength();
                    break;
                case "2":
                    ConvertWeight();
                    break;
                case "3":
                    ConvertTemperature();
                    break;
                case "4":
                    ConvertVolume();
                    break;
                case "0":
                    exit = true;
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    static void ConvertLength()
    {
        Console.WriteLine("\n--- Length Converter ---");
        Console.Write("Enter value: ");
        if (!double.TryParse(Console.ReadLine(), out double value))
        {
            Console.WriteLine("Error: Invalid number.");
            return;
        }

        Console.WriteLine($"  Kilometers: {value / 1000:F4}");
        Console.WriteLine($"  Meters: {value:F4}");
        Console.WriteLine($"  Centimeters: {value * 100:F4}");
        Console.WriteLine($"  Millimeters: {value * 1000:F4}");
        Console.WriteLine($"  Miles: {value / 1609.344:F4}");
        Console.WriteLine($"  Feet: {value * 3.28084:F4}");
        Console.WriteLine($"  Inches: {value * 39.3701:F4}");
    }

    static void ConvertWeight()
    {
        Console.WriteLine("\n--- Weight Converter ---");
        Console.Write("Enter value in kilograms: ");
        if (!double.TryParse(Console.ReadLine(), out double kg))
        {
            Console.WriteLine("Error: Invalid number.");
            return;
        }

        Console.WriteLine($"  Kilograms: {kg:F4}");
        Console.WriteLine($"  Grams: {kg * 1000:F4}");
        Console.WriteLine($"  Pounds: {kg * 2.20462:F4}");
        Console.WriteLine($"  Ounces: {kg * 35.274:F4}");
    }

    static void ConvertTemperature()
    {
        Console.WriteLine("\n--- Temperature Converter ---");
        Console.Write("Enter temperature value: ");
        if (!double.TryParse(Console.ReadLine(), out double temp))
        {
            Console.WriteLine("Error: Invalid number.");
            return;
        }

        Console.Write("Enter unit (C/F/K): ");
        string unit = Console.ReadLine().Trim().ToUpper();

        double celsius = unit switch
        {
            "C" => temp,
            "F" => (temp - 32) * 5 / 9,
            "K" => temp - 273.15,
            _ => temp
        };

        Console.WriteLine($"  Celsius: {celsius:F2}°C");
        Console.WriteLine($"  Fahrenheit: {celsius * 9 / 5 + 32:F2}°F");
        Console.WriteLine($"  Kelvin: {celsius + 273.15:F2}K");
    }

    static void ConvertVolume()
    {
        Console.WriteLine("\n--- Volume Converter ---");
        Console.Write("Enter value in liters: ");
        if (!double.TryParse(Console.ReadLine(), out double liters))
        {
            Console.WriteLine("Error: Invalid number.");
            return;
        }

        Console.WriteLine($"  Liters: {liters:F4}");
        Console.WriteLine($"  Milliliters: {liters * 1000:F4}");
        Console.WriteLine($"  Gallons (US): {liters * 0.264172:F4}");
        Console.WriteLine($"  Cups (US): {liters * 4.22675:F4}");
        Console.WriteLine($"  Fluid Ounces (US): {liters * 33.814:F4}");
    }
}
