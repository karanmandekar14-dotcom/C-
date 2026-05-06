// Program: Lorem Ipsum Generator
// Description: Generates placeholder Lorem Ipsum text for design and testing

using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Lorem Ipsum Generator ===");

        Console.Write("Number of paragraphs (1-10): ");
        if (!int.TryParse(Console.ReadLine(), out int paragraphs) || paragraphs < 1 || paragraphs > 10)
        {
            Console.WriteLine("Error: Enter a number between 1 and 10.");
            return;
        }

        Console.Write("Sentences per paragraph (3-10): ");
        if (!int.TryParse(Console.ReadLine(), out int sentences) || sentences < 3 || sentences > 10)
        {
            Console.WriteLine("Error: Enter a number between 3 and 10.");
            return;
        }

        Console.Write("Start with classic Lorem Ipsum? (y/n): ");
        bool classic = Console.ReadLine().Trim().ToLower() == "y";

        Console.WriteLine("\n--- Generated Lorem Ipsum ---\n");

        for (int p = 0; p < paragraphs; p++)
        {
            var sb = new StringBuilder();

            for (int s = 0; s < sentences; s++)
            {
                if (p == 0 && s == 0 && classic)
                {
                    sb.Append("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
                }
                else
                {
                    sb.Append(GenerateSentence());
                }

                if (s < sentences - 1)
                    sb.Append(" ");
            }

            Console.WriteLine(sb.ToString());
            Console.WriteLine();
        }
    }

    static readonly string[] Words = {
        "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit",
        "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore",
        "magna", "aliqua", "enim", "ad", "minim", "veniam", "quis", "nostrud",
        "exercitation", "ullamco", "laboris", "nisi", "aliquip", "ex", "ea", "commodo",
        "consequat", "duis", "aute", "irure", "in", "reprehenderit", "voluptate",
        "velit", "esse", "cillum", "fugiat", "nulla", "pariatur", "excepteur", "sint",
        "occaecat", "cupidatat", "non", "proident", "sunt", "culpa", "qui", "officia",
        "deserunt", "mollit", "anim", "id", "est", "laborum", "perspiciatis", "unde",
        "omnis", "iste", "natus", "error", "voluptatem", "accusantium", "doloremque",
        "laudantium", "totam", "rem", "aperiam", "eaque", "ipsa", "quae", "ab", "illo",
        "inventore", "veritatis", "quasi", "architecto", "beatae", "vitae", "dicta",
        "sunt", "explicabo", "asperiores", "aut", "odit", "fugit", "consequuntur",
        "magni", "dolores", "eos", "qui", "ratione", "sequi", "nesciunt"
    };

    static readonly Random Rng = new Random();

    static string GenerateSentence()
    {
        int wordCount = Rng.Next(8, 16);
        var sb = new StringBuilder();

        for (int i = 0; i < wordCount; i++)
        {
            if (i > 0)
                sb.Append(" ");
            sb.Append(Words[Rng.Next(Words.Length)]);
        }

        sb.Append(".");
        return char.ToUpper(sb[0]) + sb.ToString(1, sb.Length - 1);
    }
}
