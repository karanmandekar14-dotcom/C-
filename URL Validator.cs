// Program: URL Validator
// Description: Validates URL structure including protocol, domain, port, and path

using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== URL Validator ===");
        Console.WriteLine("Enter URLs to validate (type 'exit' to quit):");

        while (true)
        {
            Console.Write("\nURL: ");
            string url = Console.ReadLine();

            if (url.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            (bool isValid, var details) = ValidateUrl(url);
            Console.WriteLine($"  Result: {(isValid ? "Valid URL" : "Invalid URL")}");
            foreach (var detail in details)
            {
                Console.WriteLine($"    {detail.Key}: {detail.Value}");
            }
        }
    }

    static (bool isValid, (string Key, string Value)[] details) ValidateUrl(string url)
    {
        var details = new System.Collections.Generic.List<(string, string)>();
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(url))
        {
            details.Add(("Error", "URL cannot be empty"));
            return (false, details.ToArray());
        }

        var regex = new Regex(@"^(https?|ftp)://(?<host>[^:/\s]+)(:(?<port>\d+))?(?<path>/[^\s?]*)?(\?(?<query>[^\s#]*))?(#(?<fragment>[^\s]*))?$", RegexOptions.IgnoreCase);
        var match = regex.Match(url);

        if (!match.Success)
        {
            details.Add(("Format", "Does not match URL pattern"));
            details.Add(("Expected", "protocol://host[:port]/path[?query][#fragment]"));
            return (false, details.ToArray());
        }

        string protocol = match.Groups[1].Value;
        string host = match.Groups["host"].Value;
        string portStr = match.Groups["port"].Value;
        string path = match.Groups["path"].Value;
        string query = match.Groups["query"].Value;
        string fragment = match.Groups["fragment"].Value;

        details.Add(("Protocol", protocol));
        details.Add(("Host", host));

        if (!string.IsNullOrEmpty(portStr))
        {
            int port = int.Parse(portStr);
            if (port < 1 || port > 65535)
            {
                details.Add(("Port", $"{port} - INVALID (must be 1-65535)"));
                isValid = false;
            }
            else
            {
                details.Add(("Port", portStr));
            }
        }

        if (!string.IsNullOrEmpty(path))
        {
            details.Add(("Path", path));
        }

        if (!string.IsNullOrEmpty(query))
        {
            details.Add(("Query", query));
        }

        if (!string.IsNullOrEmpty(fragment))
        {
            details.Add(("Fragment", fragment));
        }

        // Host validation
        if (host.Contains(".."))
        {
            details.Add(("Host", $"{host} - INVALID (consecutive dots)"));
            isValid = false;
        }

        // TLD check
        int lastDot = host.LastIndexOf('.');
        if (lastDot >= 0 && lastDot < host.Length - 1)
        {
            string tld = host.Substring(lastDot + 1);
            if (tld.Length < 2)
            {
                details.Add(("TLD", $"{tld} - INVALID (too short)"));
                isValid = false;
            }
        }

        return (isValid, details.ToArray());
    }
}
