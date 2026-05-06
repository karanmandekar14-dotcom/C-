// Program: Password Strength Analyzer
// Description: Analyzes password strength with detailed scoring and recommendations

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Password Strength Analyzer ===");
        Console.WriteLine("Enter passwords to analyze (type 'exit' to quit):");

        while (true)
        {
            Console.Write("\nPassword: ");
            string password = Console.ReadLine();

            if (password.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            var result = AnalyzePassword(password);
            Console.WriteLine($"\n  Strength: {result.Label} ({result.Score}/100)");
            Console.WriteLine($"  Crack Time (estimate): {result.CrackTime}");
            Console.WriteLine($"\n  Breakdown:");

            foreach (var check in result.Checks)
            {
                string icon = check.Passed ? "✓" : "✗";
                Console.WriteLine($"    {icon} {check.Name}: {check.Message}");
            }

            if (result.Recommendations.Count > 0)
            {
                Console.WriteLine($"\n  Recommendations:");
                foreach (string rec in result.Recommendations)
                {
                    Console.WriteLine($"    • {rec}");
                }
            }
        }
    }

    static PasswordResult AnalyzePassword(string password)
    {
        var checks = new List<PasswordCheck>();
        var recommendations = new List<string>();
        int score = 0;

        // Length check
        int length = password.Length;
        if (length >= 8) { score += 10; checks.Add(new PasswordCheck("Length", true, $"{length} characters")); }
        else { checks.Add(new PasswordCheck("Length", false, $"{length} characters (too short)")); recommendations.Add("Use at least 8 characters"); }

        if (length >= 12) { score += 10; checks.Add(new PasswordCheck("Good Length", true, "12+ characters")); }
        else { checks.Add(new PasswordCheck("Good Length", false, "Less than 12 characters")); recommendations.Add("Aim for 12+ characters for strong security"); }

        // Character variety
        bool hasUpper = false, hasLower = false, hasDigit = false, hasSpecial = false;
        int uniqueChars = new HashSet<char>(password).Count;

        foreach (char c in password)
        {
            if (char.IsUpper(c)) hasUpper = true;
            if (char.IsLower(c)) hasLower = true;
            if (char.IsDigit(c)) hasDigit = true;
            if (!char.IsLetterOrDigit(c)) hasSpecial = true;
        }

        int charTypes = (hasUpper ? 1 : 0) + (hasLower ? 1 : 0) + (hasDigit ? 1 : 0) + (hasSpecial ? 1 : 0);

        if (hasUpper) { score += 10; checks.Add(new PasswordCheck("Uppercase", true, "Contains uppercase letters")); }
        else { checks.Add(new PasswordCheck("Uppercase", false, "No uppercase letters")); recommendations.Add("Add uppercase letters"); }

        if (hasLower) { score += 10; checks.Add(new PasswordCheck("Lowercase", true, "Contains lowercase letters")); }
        else { checks.Add(new PasswordCheck("Lowercase", false, "No lowercase letters")); recommendations.Add("Add lowercase letters"); }

        if (hasDigit) { score += 10; checks.Add(new PasswordCheck("Digits", true, "Contains numbers")); }
        else { checks.Add(new PasswordCheck("Digits", false, "No numbers")); recommendations.Add("Add numbers"); }

        if (hasSpecial) { score += 15; checks.Add(new PasswordCheck("Special Chars", true, "Contains special characters")); }
        else { checks.Add(new PasswordCheck("Special Chars", false, "No special characters")); recommendations.Add("Add special characters (!@#$%^&*)"); }

        // Unique character ratio
        double uniqueRatio = (double)uniqueChars / length;
        if (uniqueRatio > 0.7) { score += 15; checks.Add(new PasswordCheck("Uniqueness", true, $"{(uniqueRatio * 100):F0}% unique characters")); }
        else { checks.Add(new PasswordCheck("Uniqueness", false, $"{(uniqueRatio * 100):F0}% unique characters")); recommendations.Add("Use more unique characters"); }

        // No common patterns
        bool hasPattern = HasCommonPattern(password);
        if (!hasPattern) { score += 15; checks.Add(new PasswordCheck("No Common Patterns", true, "No dictionary words or patterns")); }
        else { checks.Add(new PasswordCheck("No Common Patterns", false, "Contains common pattern")); recommendations.Add("Avoid common words, keyboard patterns, or sequences"); }

        // Character variety bonus
        if (charTypes >= 4) { score += 5; checks.Add(new PasswordCheck("Full Variety", true, "All 4 character types used")); }
        else { checks.Add(new PasswordCheck("Full Variety", false, $"{charTypes}/4 character types used")); }

        score = Math.Min(score, 100);
        string label = score >= 80 ? "Strong" : score >= 60 ? "Moderate" : score >= 40 ? "Weak" : "Very Weak";
        string crackTime = EstimateCrackTime(score, length);

        return new PasswordResult { Score = score, Label = label, CrackTime = crackTime, Checks = checks, Recommendations = recommendations };
    }

    static bool HasCommonPattern(string password)
    {
        string lower = password.ToLower();
        string[] commonPatterns = { "password", "123456", "qwerty", "abc123", "letmein", "admin", "welcome", "monkey", "dragon", "master", "iloveyou", "football", "baseball", "shadow", "sunshine", "trustno1" };
        string[] sequences = { "abcdef", "123456", "qwerty", "asdfgh", "zxcvbn" };

        foreach (string pattern in commonPatterns)
            if (lower.Contains(pattern)) return true;

        foreach (string seq in sequences)
            if (lower.Contains(seq)) return true;

        return false;
    }

    static string EstimateCrackTime(int score, int length)
    {
        return score switch
        {
            >= 90 => "Centuries",
            >= 80 => "Years",
            >= 70 => "Months",
            >= 60 => "Days",
            >= 50 => "Hours",
            >= 40 => "Minutes",
            >= 30 => "Seconds",
            _ => "Instantly"
        };
    }
}

class PasswordCheck
{
    public string Name { get; }
    public bool Passed { get; }
    public string Message { get; }
    public PasswordCheck(string name, bool passed, string message) { Name = name; Passed = passed; Message = message; }
}

class PasswordResult
{
    public int Score { get; set; }
    public string Label { get; set; }
    public string CrackTime { get; set; }
    public List<PasswordCheck> Checks { get; set; }
    public List<string> Recommendations { get; set; }
}
