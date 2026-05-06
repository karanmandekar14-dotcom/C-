// Program: Password Generator
// Description: Generates secure random passwords with customizable length and character sets

using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Password Generator ===");

        Console.Write("Password length (8-128): ");
        if (!int.TryParse(Console.ReadLine(), out int length) || length < 8 || length > 128)
        {
            Console.WriteLine("Error: Length must be between 8 and 128.");
            return;
        }

        Console.Write("Include uppercase? (y/n): ");
        bool uppercase = Console.ReadLine().Trim().ToLower() == "y";

        Console.Write("Include lowercase? (y/n): ");
        bool lowercase = Console.ReadLine().Trim().ToLower() == "y";

        Console.Write("Include digits? (y/n): ");
        bool digits = Console.ReadLine().Trim().ToLower() == "y";

        Console.Write("Include special characters? (y/n): ");
        bool special = Console.ReadLine().Trim().ToLower() == "y";

        if (!uppercase && !lowercase && !digits && !special)
        {
            Console.WriteLine("Error: At least one character type must be selected.");
            return;
        }

        Console.Write("Number of passwords to generate (1-20): ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count < 1 || count > 20)
        {
            Console.WriteLine("Error: Enter a number between 1 and 20.");
            return;
        }

        Console.WriteLine("\n--- Generated Passwords ---");
        for (int i = 0; i < count; i++)
        {
            string password = GeneratePassword(length, uppercase, lowercase, digits, special);
            int strength = EvaluateStrength(password);
            Console.WriteLine($"  {i + 1,2}. {password,-30}  Strength: {GetStrengthLabel(strength)}");
        }
    }

    static string GeneratePassword(int length, bool uppercase, bool lowercase, bool digits, bool special)
    {
        var charPool = new StringBuilder();
        if (uppercase) charPool.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        if (lowercase) charPool.Append("abcdefghijklmnopqrstuvwxyz");
        if (digits) charPool.Append("0123456789");
        if (special) charPool.Append("!@#$%^&*()_-+=<>?/[]{}|");

        string pool = charPool.ToString();
        var result = new char[length];

        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] randomBytes = new byte[length];
            rng.GetBytes(randomBytes);

            for (int i = 0; i < length; i++)
            {
                result[i] = pool[randomBytes[i] % pool.Length];
            }
        }

        return new string(result);
    }

    static int EvaluateStrength(string password)
    {
        int score = 0;
        if (password.Length >= 8) score++;
        if (password.Length >= 12) score++;
        if (password.Length >= 16) score++;
        if (password.ContainsUpper()) score++;
        if (password.ContainsLower()) score++;
        if (password.ContainsDigit()) score++;
        if (password.ContainsSpecial()) score++;
        return score;
    }

    static string GetStrengthLabel(int score)
    {
        return score switch
        {
            <= 2 => "Weak",
            <= 4 => "Moderate",
            <= 5 => "Strong",
            _    => "Very Strong"
        };
    }

    static bool ContainsUpper(this string s)
    {
        foreach (char c in s)
            if (char.IsUpper(c)) return true;
        return false;
    }

    static bool ContainsLower(this string s)
    {
        foreach (char c in s)
            if (char.IsLower(c)) return true;
        return false;
    }

    static bool ContainsDigit(this string s)
    {
        foreach (char c in s)
            if (char.IsDigit(c)) return true;
        return false;
    }

    static bool ContainsSpecial(this string s)
    {
        foreach (char c in s)
            if (!char.IsLetterOrDigit(c)) return true;
        return false;
    }
}
