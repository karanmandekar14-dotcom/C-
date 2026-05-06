// Program: BMI Calculator
// Description: Calculates Body Mass Index with category and health recommendations

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== BMI Calculator ===");

        Console.Write("Enter weight (kg): ");
        if (!double.TryParse(Console.ReadLine(), out double weightKg) || weightKg <= 0)
        {
            Console.WriteLine("Error: Enter a valid weight.");
            return;
        }

        Console.Write("Enter height (cm): ");
        if (!double.TryParse(Console.ReadLine(), out double heightCm) || heightCm <= 0)
        {
            Console.WriteLine("Error: Enter a valid height.");
            return;
        }

        double heightM = heightCm / 100.0;
        double bmi = weightKg / (heightM * heightM);
        (string category, string color, string recommendation) = GetBmiCategory(bmi);

        double idealMin = 18.5 * heightM * heightM;
        double idealMax = 24.9 * heightM * heightM;

        Console.WriteLine("\n--- BMI Results ---");
        Console.WriteLine($"  Weight: {weightKg:F1} kg");
        Console.WriteLine($"  Height: {heightCm:F1} cm ({heightM:F2} m)");
        Console.WriteLine($"  BMI: {bmi:F1}");
        Console.WriteLine($"  Category: {category}");
        Console.WriteLine($"  Ideal weight range: {idealMin:F1} - {idealMax:F1} kg");
        Console.WriteLine($"  Recommendation: {recommendation}");
    }

    static (string category, string color, string recommendation) GetBmiCategory(double bmi)
    {
        return bmi switch
        {
            < 16.0 => ("Severe Underweight", "Blue", "Consult a doctor immediately. You need significant weight gain."),
            < 18.5 => ("Underweight", "Yellow", "Consider a calorie-surplus diet with strength training."),
            < 25.0 => ("Normal Weight", "Green", "Maintain your current healthy lifestyle!"),
            < 30.0 => ("Overweight", "Orange", "Consider increasing physical activity and reviewing your diet."),
            < 35.0 => ("Obese Class I", "Red", "Consult a nutritionist. Aim for gradual weight loss."),
            < 40.0 => ("Obese Class II", "Red", "Seek medical advice. Structured weight loss program recommended."),
            _ => ("Obese Class III", "Red", "Urgent medical consultation recommended.")
        };
    }
}
