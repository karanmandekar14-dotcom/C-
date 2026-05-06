// Program: Email Validator
// Description: RFC-style email validation with detailed error messages and domain checking

using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Email Validator ===");
        Console.WriteLine("Enter email addresses to validate (type 'exit' to quit):");

        while (true)
        {
            Console.Write("\nEmail: ");
            string email = Console.ReadLine();

            if (email.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            (bool isValid, var errors) = ValidateEmail(email);
            if (isValid)
            {
                Console.WriteLine("  Result: Valid email address.");
            }
            else
            {
                Console.WriteLine("  Result: Invalid email address.");
                foreach (string err in errors)
                {
                    Console.WriteLine($"    - {err}");
                }
            }
        }
    }

    static (bool isValid, string[] errors) ValidateEmail(string email)
    {
        var errors = new System.Collections.Generic.List<string>();

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add("Email cannot be empty.");
            return (false, errors.ToArray());
        }

        // Check overall format
        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!regex.IsMatch(email))
        {
            errors.Add("Email must follow format: localpart@domain.extension");
        }

        string[] parts = email.Split('@');
        if (parts.Length != 2)
        {
            errors.Add("Email must contain exactly one '@' symbol.");
            return (false, errors.ToArray());
        }

        string localPart = parts[0];
        string domainPart = parts[1];

        // Local part checks
        if (localPart.Length == 0)
            errors.Add("Local part (before @) cannot be empty.");
        if (localPart.Length > 64)
            errors.Add($"Local part too long ({localPart.Length} chars, max 64).");
        if (localPart.StartsWith(".") || localPart.EndsWith("."))
            errors.Add("Local part cannot start or end with a dot.");
        if (localPart.Contains(".."))
            errors.Add("Local part cannot contain consecutive dots.");
        if (localPart.IndexOfAny(new[] { ' ', ',', ':', ';', '<', '>', '[', ']', '(', ')', '\\' }) >= 0)
            errors.Add("Local part contains invalid characters.");

        // Domain part checks
        if (domainPart.Length == 0)
            errors.Add("Domain part (after @) cannot be empty.");
        if (domainPart.Length > 255)
            errors.Add($"Domain part too long ({domainPart.Length} chars, max 255).");
        if (domainPart.StartsWith("-") || domainPart.EndsWith("-"))
            errors.Add("Domain cannot start or end with a hyphen.");
        if (domainPart.Contains(".."))
            errors.Add("Domain cannot contain consecutive dots.");

        // Extension check
        int lastDot = domainPart.LastIndexOf('.');
        if (lastDot >= 0)
        {
            string ext = domainPart.Substring(lastDot + 1);
            if (ext.Length < 2)
                errors.Add("Domain extension must be at least 2 characters.");
            if (ext.Length > 6)
                errors.Add("Domain extension seems too long (may be invalid).");
        }
        else
        {
            errors.Add("Domain must contain at least one dot with an extension.");
        }

        return (errors.Count == 0, errors.ToArray());
    }
}
