// Program: GPA Calculator
// Description: Calculates grade point average from course grades and credit hours

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== GPA Calculator ===");

        Console.Write("Enter number of courses: ");
        if (!int.TryParse(Console.ReadLine(), out int courseCount) || courseCount <= 0)
        {
            Console.WriteLine("Error: Enter a valid number.");
            return;
        }

        var courses = new List<Course>();
        double totalCredits = 0;
        double totalGradePoints = 0;

        for (int i = 0; i < courseCount; i++)
        {
            Console.WriteLine($"\n--- Course {i + 1} ---");
            Console.Write("Course name: ");
            string name = Console.ReadLine();

            Console.Write("Credit hours: ");
            if (!int.TryParse(Console.ReadLine(), out int credits) || credits <= 0)
            {
                Console.WriteLine("Invalid credits, skipping.");
                continue;
            }

            Console.Write("Grade (A+, A, A-, B+, B, B-, C+, C, C-, D+, D, F): ");
            string grade = Console.ReadLine().Trim().ToUpper();

            double gradePoint = GetGradePoint(grade);
            if (gradePoint < 0)
            {
                Console.WriteLine("Invalid grade, skipping.");
                continue;
            }

            courses.Add(new Course { Name = name, Credits = credits, Grade = grade, GradePoint = gradePoint });
            totalCredits += credits;
            totalGradePoints += gradePoint * credits;
        }

        if (courses.Count == 0)
        {
            Console.WriteLine("No valid courses entered.");
            return;
        }

        double gpa = totalGradePoints / totalCredits;

        Console.WriteLine("\n=== GPA Report ===");
        Console.WriteLine($"  {"Course",-20} {"Credits",-10} {"Grade",-8} {"Points",-8}");
        Console.WriteLine(new string('-', 50));

        foreach (var course in courses)
        {
            Console.WriteLine($"  {course.Name,-20} {course.Credits,-10} {course.Grade,-8} {course.GradePoint,-8:F1}");
        }

        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"  {"Total",-20} {totalCredits,-10} {"",-8} {totalGradePoints,-8:F1}");
        Console.WriteLine($"\n  Cumulative GPA: {gpa:F2} / 4.00");
        Console.WriteLine($"  Standing: {GetStanding(gpa)}");
    }

    static double GetGradePoint(string grade)
    {
        return grade switch
        {
            "A+" or "A" => 4.0,
            "A-" => 3.7,
            "B+" => 3.3,
            "B" => 3.0,
            "B-" => 2.7,
            "C+" => 2.3,
            "C" => 2.0,
            "C-" => 1.7,
            "D+" => 1.3,
            "D" => 1.0,
            "F" => 0.0,
            _ => -1
        };
    }

    static string GetStanding(double gpa)
    {
        return gpa switch
        {
            >= 3.9 => "Summa Cum Laude",
            >= 3.7 => "Magna Cum Laude",
            >= 3.5 => "Cum Laude",
            >= 3.0 => "Dean's List",
            >= 2.0 => "Good Standing",
            >= 1.0 => "Academic Probation",
            _ => "Academic Dismissal"
        };
    }
}

class Course
{
    public string Name { get; set; }
    public int Credits { get; set; }
    public string Grade { get; set; }
    public double GradePoint { get; set; }
}
