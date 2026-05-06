// Program: Age Calculator
// Description: Calculates exact age in years, months, days from birthdate

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Age Calculator ===");
        Console.Write("Enter your birthdate (YYYY-MM-DD): ");
        string input = Console.ReadLine();

        if (!DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime birthDate))
        {
            Console.WriteLine("Error: Invalid date format. Use YYYY-MM-DD.");
            return;
        }

        if (birthDate > DateTime.Now)
        {
            Console.WriteLine("Error: Birthdate cannot be in the future.");
            return;
        }

        DateTime today = DateTime.Now;
        int years = today.Year - birthDate.Year;
        int months = today.Month - birthDate.Month;
        int days = today.Day - birthDate.Day;

        if (days < 0)
        {
            months--;
            days += DateTime.DaysInMonth(today.AddMonths(-1).Year, today.AddMonths(-1).Month);
        }
        if (months < 0)
        {
            years--;
            months += 12;
        }

        TimeSpan totalDays = today - birthDate;
        int totalMonths = years * 12 + months;
        int totalWeeks = (int)(totalDays.TotalDays / 7);
        int totalHours = (int)totalDays.TotalHours;
        int nextBirthdayDays = DaysUntilNextBirthday(birthDate, today);

        Console.WriteLine("\n--- Age Details ---");
        Console.WriteLine($"  Birthdate: {birthDate:dddd, MMMM dd, yyyy}");
        Console.WriteLine($"  Current Age: {years} years, {months} months, {days} days");
        Console.WriteLine($"  Total months: {totalMonths:N0}");
        Console.WriteLine($"  Total weeks: {totalWeeks:N0}");
        Console.WriteLine($"  Total days: {totalDays.Days:N0}");
        Console.WriteLine($"  Total hours: {totalHours:N0}");
        Console.WriteLine($"  Total minutes: {(int)totalDays.TotalMinutes:N0}");
        Console.WriteLine($"  Next birthday: {nextBirthdayDays} days away");

        // Fun facts
        Console.WriteLine("\n--- Fun Facts ---");
        Console.WriteLine($"  Heartbeats (approx): {(totalDays.TotalDays * 100000):N0}");
        Console.WriteLine($"  Breaths (approx): {(totalDays.TotalDays * 20000):N0}");
        Console.WriteLine($"  Sleeps (approx, 8hrs/day): {(totalDays.TotalDays / 3):N0} days sleeping");
        Console.WriteLine($"  Born on a: {birthDate:dddd}");

        // Chinese zodiac
        string[] animals = { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };
        Console.WriteLine($"  Chinese Zodiac: {animals[(birthDate.Year - 4) % 12]}");
    }

    static int DaysUntilNextBirthday(DateTime birthDate, DateTime today)
    {
        DateTime nextBirthday = new DateTime(today.Year, birthDate.Month, birthDate.Day);
        if (nextBirthday < today)
            nextBirthday = nextBirthday.AddYears(1);
        return (nextBirthday - today).Days;
    }
}
