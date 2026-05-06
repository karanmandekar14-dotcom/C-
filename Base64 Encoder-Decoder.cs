// Program: Base64 Encoder/Decoder
// Description: Encodes text to Base64 and decodes Base64 back to text

using System;
using System.Text;

class Program
{
    static void Main()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n=== Base64 Encoder/Decoder ===");
            Console.WriteLine("1. Encode text to Base64");
            Console.WriteLine("2. Decode Base64 to text");
            Console.WriteLine("3. Encode file to Base64");
            Console.WriteLine("4. Decode Base64 to file");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": EncodeText(); break;
                case "2": DecodeText(); break;
                case "3": EncodeFile(); break;
                case "4": DecodeFile(); break;
                case "0":
                    exit = true;
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void EncodeText()
    {
        Console.Write("Enter text to encode: ");
        string text = Console.ReadLine();

        if (string.IsNullOrEmpty(text))
        {
            Console.WriteLine("Error: Text cannot be empty.");
            return;
        }

        byte[] bytes = Encoding.UTF8.GetBytes(text);
        string base64 = Convert.ToBase64String(bytes);

        Console.WriteLine($"\nOriginal text: {text}");
        Console.WriteLine($"Base64 encoded: {base64}");
        Console.WriteLine($"Byte size: {bytes.Length}");
    }

    static void DecodeText()
    {
        Console.Write("Enter Base64 string to decode: ");
        string base64 = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(base64))
        {
            Console.WriteLine("Error: Input cannot be empty.");
            return;
        }

        try
        {
            byte[] bytes = Convert.FromBase64String(base64);
            string text = Encoding.UTF8.GetString(bytes);

            Console.WriteLine($"\nBase64 input: {base64}");
            Console.WriteLine($"Decoded text: {text}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Invalid Base64 string.");
        }
    }

    static void EncodeFile()
    {
        Console.Write("Enter file path to encode: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        try
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string base64 = Convert.ToBase64String(fileBytes);

            Console.WriteLine($"\nFile: {System.IO.Path.GetFileName(filePath)}");
            Console.WriteLine($"Size: {fileBytes.Length:N0} bytes");
            Console.WriteLine($"Base64 length: {base64.Length:N0} characters");

            // Show first 100 chars
            string preview = base64.Length > 100 ? base64.Substring(0, 100) + "..." : base64;
            Console.WriteLine($"\nBase64 (preview): {preview}");

            Console.Write("\nSave to file? (y/n): ");
            if (Console.ReadLine().Trim().ToLower() == "y")
            {
                string outputPath = filePath + ".b64";
                System.IO.File.WriteAllText(outputPath, base64);
                Console.WriteLine($"Saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void DecodeFile()
    {
        Console.Write("Enter Base64 file path: ");
        string b64Path = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(b64Path) || !System.IO.File.Exists(b64Path))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        Console.Write("Enter output file path: ");
        string outputPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            Console.WriteLine("Error: Output path cannot be empty.");
            return;
        }

        try
        {
            string base64 = System.IO.File.ReadAllText(b64Path);
            byte[] bytes = Convert.FromBase64String(base64);
            System.IO.File.WriteAllBytes(outputPath, bytes);

            Console.WriteLine($"\nDecoded successfully!");
            Console.WriteLine($"Output: {outputPath}");
            Console.WriteLine($"Size: {bytes.Length:N0} bytes");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
