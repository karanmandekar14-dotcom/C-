// Program: Discount and Tax Calculator
// Description: Applies discounts, calculates tax, and shows final price breakdown

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Discount & Tax Calculator ===");

        Console.Write("Enter original price: ");
        if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
        {
            Console.WriteLine("Error: Enter a valid price.");
            return;
        }

        Console.Write("Enter discount percentage (0 for none): ");
        if (!double.TryParse(Console.ReadLine(), out double discountPct) || discountPct < 0)
        {
            Console.WriteLine("Error: Enter a valid discount.");
            return;
        }

        Console.Write("Enter tax rate % (0 for none): ");
        if (!double.TryParse(Console.ReadLine(), out double taxPct) || taxPct < 0)
        {
            Console.WriteLine("Error: Enter a valid tax rate.");
            return;
        }

        Console.Write("Apply discount before tax? (y/n): ");
        bool discountFirst = Console.ReadLine().Trim().ToLower() == "y";

        double discountAmount, discountedPrice, taxAmount, finalPrice;

        if (discountFirst)
        {
            discountAmount = price * discountPct / 100;
            discountedPrice = price - discountAmount;
            taxAmount = discountedPrice * taxPct / 100;
            finalPrice = discountedPrice + taxAmount;
        }
        else
        {
            taxAmount = price * taxPct / 100;
            double priceWithTax = price + taxAmount;
            discountAmount = priceWithTax * discountPct / 100;
            finalPrice = priceWithTax - discountAmount;
        }

        Console.WriteLine("\n--- Price Breakdown ---");
        Console.WriteLine($"  Original Price:     {price,10:N2}");
        Console.WriteLine($"  Discount ({discountPct:F1}%):  {-discountAmount,10:N2}");
        Console.WriteLine($"  After Discount:     {price - discountAmount,10:N2}");
        Console.WriteLine($"  Tax ({taxPct:F1}%):       {taxAmount,10:N2}");
        Console.WriteLine($"  ─────────────────────────────");
        Console.WriteLine($"  Final Price:        {finalPrice,10:N2}");
        Console.WriteLine($"  Total Savings:      {price - finalPrice,10:N2}");

        // Multiple item support
        Console.Write("\nCalculate for multiple items? (y/n): ");
        if (Console.ReadLine().Trim().ToLower() == "y")
        {
            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                return;
            }

            Console.Write("Apply bulk discount % (0 for none): ");
            if (!double.TryParse(Console.ReadLine(), out double bulkDiscount) || bulkDiscount < 0)
            {
                bulkDiscount = 0;
            }

            double subtotal = finalPrice * qty;
            double bulkDiscountAmt = subtotal * bulkDiscount / 100;
            double grandTotal = subtotal - bulkDiscountAmt;

            Console.WriteLine($"\n--- Bulk Order ({qty} items) ---");
            Console.WriteLine($"  Subtotal:           {subtotal,10:N2}");
            Console.WriteLine($"  Bulk Discount ({bulkDiscount:F1}%): {-bulkDiscountAmt,10:N2}");
            Console.WriteLine($"  Grand Total:        {grandTotal,10:N2}");
            Console.WriteLine($"  Per Item:           {grandTotal / qty,10:N2}");
        }
    }
}
