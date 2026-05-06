// Program: IP Address Validator
// Description: Validates IPv4 and IPv6 addresses with detailed component breakdown

using System;
using System.Net;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== IP Address Validator ===");
        Console.WriteLine("Enter IP addresses to validate (type 'exit' to quit):");

        while (true)
        {
            Console.Write("\nIP Address: ");
            string input = Console.ReadLine();

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("  Error: Input cannot be empty.");
                continue;
            }

            ValidateAndDisplay(input.Trim());
        }
    }

    static void ValidateAndDisplay(string ip)
    {
        if (IsValidIPv4(ip))
        {
            Console.WriteLine($"  Result: Valid IPv4 Address");
            string[] octets = ip.Split('.');
            Console.WriteLine($"  Version: IPv4");
            Console.WriteLine($"  Octets: {string.Join(", ", octets)}");
            Console.WriteLine($"  Binary: {ToBinary(ip)}");
            Console.WriteLine($"  Class: {GetIpClass(ip)}");
            Console.WriteLine($"  Is Private: {IsPrivateIp(ip)}");

            // Network info
            Console.WriteLine($"  First octet: {octets[0]}");
            Console.WriteLine($"  Last octet: {octets[3]}");
        }
        else if (IsValidIPv6(ip))
        {
            Console.WriteLine($"  Result: Valid IPv6 Address");
            Console.WriteLine($"  Version: IPv6");
            Console.WriteLine($"  Expanded: {ExpandIPv6(ip)}");
            Console.WriteLine($"  Is Loopback: {ip.Equals("::1", StringComparison.OrdinalIgnoreCase)}");
        }
        else
        {
            Console.WriteLine($"  Result: Invalid IP Address");
            Console.WriteLine($"  Input: {ip}");

            // Provide hints
            if (ip.Contains('.') && ip.Contains(':'))
            {
                Console.WriteLine("  Hint: Mixed IPv4/IPv6 format not supported.");
            }
            else if (ip.Contains('.'))
            {
                Console.WriteLine("  Hint: Looks like IPv4 but has invalid octets.");
            }
            else if (ip.Contains(':'))
            {
                Console.WriteLine("  Hint: Looks like IPv6 but has invalid segments.");
            }
            else
            {
                Console.WriteLine("  Hint: Does not match IPv4 or IPv6 format.");
            }
        }
    }

    static bool IsValidIPv4(string ip)
    {
        if (IPAddress.TryParse(ip, out IPAddress address))
        {
            return address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                && ip.Split('.').Length == 4;
        }
        return false;
    }

    static bool IsValidIPv6(string ip)
    {
        if (IPAddress.TryParse(ip, out IPAddress address))
        {
            return address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
        }
        return false;
    }

    static string ToBinary(string ip)
    {
        string[] octets = ip.Split('.');
        return string.Join(".", Array.ConvertAll(octets, o => Convert.ToString(int.Parse(o), 2).PadLeft(8, '0')));
    }

    static string GetIpClass(string ip)
    {
        int firstOctet = int.Parse(ip.Split('.')[0]);
        return firstOctet switch
        {
            < 128 => "A",
            < 192 => "B",
            < 224 => "C",
            < 240 => "D (Multicast)",
            _ => "E (Reserved)"
        };
    }

    static bool IsPrivateIp(string ip)
    {
        int first = int.Parse(ip.Split('.')[0]);
        int second = int.Parse(ip.Split('.')[1]);

        return first switch
        {
            10 => true,
            172 => second >= 16 && second <= 31,
            192 => second == 168,
            127 => true, // loopback
            _ => false
        };
    }

    static string ExpandIPv6(string ip)
    {
        try
        {
            var addr = IPAddress.Parse(ip);
            return addr.ToString();
        }
        catch
        {
            return "Unable to expand";
        }
    }
}
