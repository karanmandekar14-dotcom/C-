// Program: Credit Card Validator
// Description: Validates credit card numbers using Luhn algorithm with card type detection

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Credit Card Validator ===");
        Console.WriteLine("Enter credit card numbers to validate (type 'exit' to quit):");

        while (true)
        {
            Console.Write("\nCard Number: ");
            string input = Console.ReadLine();

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            string cleaned = input.Replace(" ", "").Replace("-", "");
            (bool isValid, string cardType, string error) = ValidateCard(cleaned);

            if (isValid)
            {
                Console.WriteLine($"  Result: Valid");
                Console.WriteLine($"  Card Type: {cardType}");
                Console.WriteLine($"  Masked: {MaskCard(cleaned)}");
            }
            else
            {
                Console.WriteLine($"  Result: Invalid");
                Console.WriteLine($"  Error: {error}");
                if (!string.IsNullOrEmpty(cardType))
                {
                    Console.WriteLine($"  Detected Type: {cardType}");
                }
            }
        }
    }

    static (bool isValid, string cardType, string error) ValidateCard(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return (false, "", "Card number cannot be empty.");

        if (!IsAllDigits(number))
            return (false, "", "Card number must contain only digits.");

        if (number.Length < 13 || number.Length > 19)
            return (false, "", $"Card number length invalid ({number.Length} digits).");

        string cardType = DetectCardType(number);
        bool luhnValid = LuhnCheck(number);

        return (luhnValid, cardType, luhnValid ? null : "Failed Luhn check.");
    }

    static bool LuhnCheck(string number)
    {
        int sum = 0;
        bool alternate = false;

        for (int i = number.Length - 1; i >= 0; i--)
        {
            int n = number[i] - '0';
            if (alternate)
            {
                n *= 2;
                if (n > 9)
                    n -= 9;
            }
            sum += n;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }

    static string DetectCardType(string number)
    {
        if (number.StartsWith("4"))
            return "Visa";
        if (StartsWithRange(number, 51, 55))
            return "MasterCard";
        if (number.StartsWith("34") || number.StartsWith("37"))
            return "American Express";
        if (number.StartsWith("6011") || StartsWithRange(number, 644, 649) || number.StartsWith("65"))
            return "Discover";
        if (number.StartsWith("3528") || number.StartsWith("3589"))
            return "JCB";
        if (StartsWithRange(number, 300, 305) || number.StartsWith("36") || number.StartsWith("38"))
            return "Diners Club";
        return "Unknown";
    }

    static bool StartsWithRange(string number, int min, int max)
    {
        if (number.Length < 2) return false;
        int prefix = int.Parse(number.Substring(0, 2));
        return prefix >= min && prefix <= max;
    }

    static bool IsAllDigits(string s)
    {
        foreach (char c in s)
        {
            if (c < '0' || c > '9')
                return false;
        }
        return true;
    }

    static string MaskCard(string number)
    {
        if (number.Length <= 4)
            return number;

        string masked = new string('*', number.Length - 4);
        return masked + number.Substring(number.Length - 4);
    }
}
