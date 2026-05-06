// Program: Phone Number Validator
// Description: International phone number format validation with country code detection

using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Phone Number Validator ===");
        Console.WriteLine("Enter phone numbers to validate (type 'exit' to quit):");

        while (true)
        {
            Console.Write("\nPhone: ");
            string phone = Console.ReadLine();

            if (phone.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            (bool isValid, string formatted, string country, string error) = ValidatePhone(phone);
            if (isValid)
            {
                Console.WriteLine($"  Result: Valid");
                Console.WriteLine($"  Formatted: {formatted}");
                Console.WriteLine($"  Region: {country}");
            }
            else
            {
                Console.WriteLine($"  Result: Invalid");
                Console.WriteLine($"  Error: {error}");
            }
        }
    }

    static (bool isValid, string formatted, string country, string error) ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return (false, "", "", "Phone number cannot be empty.");

        // Remove spaces, dashes, parentheses
        string cleaned = Regex.Replace(phone, @"[\s\-\(\)]", "");

        // Remove leading + or 00
        string digits = cleaned;
        if (digits.StartsWith("+"))
            digits = digits.Substring(1);
        else if (digits.StartsWith("00"))
            digits = digits.Substring(2);

        if (!Regex.IsMatch(digits, @"^\d+$"))
            return (false, "", "", "Phone number contains invalid characters.");

        if (digits.Length < 7)
            return (false, "", "", "Phone number too short (minimum 7 digits).");

        if (digits.Length > 15)
            return (false, "", "", "Phone number too long (maximum 15 digits).");

        // Detect country by prefix
        (string country, string format) = DetectCountry(digits);

        string formatted = format.Replace("XXXXXXXXXXX", PadDigits(digits, format.Length));
        return (true, formatted, country, null);
    }

    static (string country, string format) DetectCountry(string digits)
    {
        var prefixes = new (string prefix, string country, string format)[]
        {
            ("1", "US/Canada", "+1 (XXX) XXX-XXXX"),
            ("44", "United Kingdom", "+44 XXXX XXXXXX"),
            ("91", "India", "+91 XXXXX XXXXX"),
            ("86", "China", "+86 XXX XXXX XXXX"),
            ("81", "Japan", "+81 XX XXXX XXXX"),
            ("49", "Germany", "+49 XXX XXXXXXX"),
            ("33", "France", "+33 X XX XX XX XX"),
            ("61", "Australia", "+61 X XXXX XXXX"),
            ("55", "Brazil", "+55 XX XXXXX-XXXX"),
            ("7", "Russia", "+7 (XXX) XXX-XX-XX"),
            ("82", "South Korea", "+82 XX XXXX XXXX"),
            ("39", "Italy", "+39 XXX XXX XXXX"),
            ("34", "Spain", "+34 XXX XXX XXX"),
            ("27", "South Africa", "+27 XX XXX XXXX"),
            ("971", "UAE", "+971 XX XXX XXXX"),
            ("966", "Saudi Arabia", "+966 XX XXX XXXX"),
            ("65", "Singapore", "+65 XXXX XXXX"),
            ("60", "Malaysia", "+60 XX XXX XXXX"),
            ("62", "Indonesia", "+62 XXX XXXX XXXX"),
            ("234", "Nigeria", "+234 XXX XXX XXXX"),
        };

        foreach (var (prefix, country, format) in prefixes)
        {
            if (digits.StartsWith(prefix))
            {
                return (country, format);
            }
        }

        return ("Unknown", $"+{digits.Substring(0, Math.Min(3, digits.Length))} {digits.Substring(Math.Min(3, digits.Length))}");
    }

    static string PadDigits(string digits, int targetLength)
    {
        return digits.PadRight(targetLength, 'X').Substring(0, targetLength);
    }
}
