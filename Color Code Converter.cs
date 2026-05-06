// Program: Color Code Converter
// Description: Converts between HEX, RGB, and HSL color formats

using System;

class Program
{
    static void Main()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n=== Color Code Converter ===");
            Console.WriteLine("1. HEX to RGB");
            Console.WriteLine("2. RGB to HEX");
            Console.WriteLine("3. RGB to HSL");
            Console.WriteLine("4. HSL to RGB");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": HexToRgb(); break;
                case "2": RgbToHex(); break;
                case "3": RgbToHsl(); break;
                case "4": HslToRgb(); break;
                case "0":
                    exit = true;
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void HexToRgb()
    {
        Console.Write("Enter HEX color (e.g., #FF5733): ");
        string hex = Console.ReadLine().Trim().TrimStart('#');

        if (hex.Length != 6 || !int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out _))
        {
            Console.WriteLine("Error: Invalid HEX format. Use 6 hex digits (e.g., FF5733).");
            return;
        }

        int r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        int b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        Console.WriteLine($"  RGB: ({r}, {g}, {b})");
    }

    static void RgbToHex()
    {
        Console.Write("Enter R (0-255): ");
        if (!int.TryParse(Console.ReadLine(), out int r) || r < 0 || r > 255)
        {
            Console.WriteLine("Error: Invalid value.");
            return;
        }
        Console.Write("Enter G (0-255): ");
        if (!int.TryParse(Console.ReadLine(), out int g) || g < 0 || g > 255)
        {
            Console.WriteLine("Error: Invalid value.");
            return;
        }
        Console.Write("Enter B (0-255): ");
        if (!int.TryParse(Console.ReadLine(), out int b) || b < 0 || b > 255)
        {
            Console.WriteLine("Error: Invalid value.");
            return;
        }

        Console.WriteLine($"  HEX: #{r:X2}{g:X2}{b:X2}");
    }

    static void RgbToHsl()
    {
        Console.Write("Enter R (0-255): ");
        if (!int.TryParse(Console.ReadLine(), out int r)) return;
        Console.Write("Enter G (0-255): ");
        if (!int.TryParse(Console.ReadLine(), out int g)) return;
        Console.Write("Enter B (0-255): ");
        if (!int.TryParse(Console.ReadLine(), out int b)) return;

        double rf = r / 255.0, gf = g / 255.0, bf = b / 255.0;
        double max = Math.Max(rf, Math.Max(gf, bf));
        double min = Math.Min(rf, Math.Min(gf, bf));
        double h, s, l = (max + min) / 2;

        if (max == min)
        {
            h = s = 0;
        }
        else
        {
            double d = max - min;
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
            h = max switch
            {
                var m when m == rf => (gf - bf) / d + (gf < bf ? 6 : 0),
                var m when m == gf => (bf - rf) / d + 2,
                _ => (rf - gf) / d + 4
            };
            h /= 6;
        }

        Console.WriteLine($"  HSL: ({(int)(h * 360)}°, {(int)(s * 100)}%, {(int)(l * 100)}%)");
    }

    static void HslToRgb()
    {
        Console.Write("Enter H (0-360): ");
        if (!int.TryParse(Console.ReadLine(), out int h)) return;
        Console.Write("Enter S (0-100): ");
        if (!int.TryParse(Console.ReadLine(), out int s)) return;
        Console.Write("Enter L (0-100): ");
        if (!int.TryParse(Console.ReadLine(), out int l)) return;

        double sh = h / 360.0, ss = s / 100.0, sl = l / 100.0;
        double r, g, b;

        if (ss == 0)
        {
            r = g = b = sl;
        }
        else
        {
            double q = sl < 0.5 ? sl * (1 + ss) : sl + ss - sl * ss;
            double p = 2 * sl - q;
            r = HueToRgb(p, q, sh + 1.0 / 3);
            g = HueToRgb(p, q, sh);
            b = HueToRgb(p, q, sh - 1.0 / 3);
        }

        Console.WriteLine($"  RGB: ({(int)(r * 255)}, {(int)(g * 255)}, {(int)(b * 255)})");
    }

    static double HueToRgb(double p, double q, double t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1.0 / 6) return p + (q - p) * 6 * t;
        if (t < 1.0 / 2) return q;
        if (t < 2.0 / 3) return p + (q - p) * (2.0 / 3 - t) * 6;
        return p;
    }
}
