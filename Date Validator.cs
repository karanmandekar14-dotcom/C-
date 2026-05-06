// Program: Date Validator
// Description: Validates date formats, checks valid ranges, and provides date information

using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Date Validator ===");
        Console.WriteLine("Enter dates to validate (type 'exit' to quit):");

        while (true)
        {
            Console.Write("\nDate (e.g., 2024-03-15, 15/03/2024, March 15 2024): ");
            string input = Console.ReadLine();

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            (bool isValid, DateTime date, string format, string error) = ValidateDate(input);

            if (isValid)
            {
                Console.WriteLine($"  Result: Valid date");
                Console.WriteLine($"  Parsed as: {date:dddd, MMMM dd, yyyy}");
                Console.WriteLine($"  Detected format: {format}");
                Console.WriteLine($"  Day of year: {date.DayOfYear}");
                Console.WriteLine($"  Week number: {GetWeekNumber(date)}");
                Console.WriteLine($"  Is leap year: {DateTime.IsLeapYear(date.Year)}");
                Console.WriteLine($"  Quarter: Q{GetQuarter(date)}");
                Console.WriteLine($"  Days in month: {DateTime.DaysInMonth(date.Year, date.Month)}");
                Console.WriteLine($"  Zodiac: {GetChineseZodiac(date.Year)}");
            }
            else
            {
                Console.WriteLine($"  Result: Invalid date");
                Console.WriteLine($"  Error: {error}");
            }
        }
    }

    static (bool isValid, DateTime date, string format, string error) ValidateDate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, DateTime.MinValue, "", "Input cannot be empty.");

        // Try ISO format: YYYY-MM-DD
        if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1))
            return (true, date1, "ISO 8601 (YYYY-MM-DD)", null);

        // Try DD/MM/YYYY
        if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date2))
            return (true, date2, "DD/MM/YYYY", null);

        // Try MM/DD/YYYY
        if (DateTime.TryParseExact(input, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date3))
            return (true, date3, "MM/DD/YYYY", null);

        // Try long format: Month DD, YYYY
        if (DateTime.TryParseExact(input, "MMMM dd, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date4))
            return (true, date4, "Long (Month DD, YYYY)", null);

        // Try short format: Mon DD, YYYY
        if (DateTime.TryParseExact(input, "MMM dd, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date5))
            return (true, date5, "Short (Mon DD, YYYY)", null);

        // Try generic parse
        if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date6))
            return (true, date6, "Auto-detected", null);

        return (false, DateTime.MinValue, "", "Could not parse date. Try formats: YYYY-MM-DD, DD/MM/YYYY, MM/DD/YYYY, Month DD YYYY");
    }

    static int GetWeekNumber(DateTime date)
    {
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
            date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    static int GetQuarter(DateTime date)
    {
        return (date.Month - 1) / 3 + 1;
    }

    static string GetChineseZodiac(int year)
    {
        string[] animals = { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake",
                             "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };
        return animals[(year - 4) % 12];
    }
}
