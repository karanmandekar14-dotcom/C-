// Program: File Permission Checker
// Description: Shows read/write/execute permissions for files and directories

using System;
using System.IO;
using System.Security.AccessControl;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== File Permission Checker ===");
        Console.Write("Enter file or directory path: ");
        string path = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path) && !Directory.Exists(path)))
        {
            Console.WriteLine("Error: Path not found.");
            return;
        }

        try
        {
            if (File.Exists(path))
            {
                CheckFilePermissions(path);
            }
            else
            {
                CheckDirectoryPermissions(path);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void CheckFilePermissions(string filePath)
    {
        var info = new FileInfo(filePath);
        Console.WriteLine($"\n--- File Permissions ---");
        Console.WriteLine($"  Path: {filePath}");
        Console.WriteLine($"  Size: {info.Length:N0} bytes");
        Console.WriteLine($"  Is Read Only: {info.IsReadOnly}");
        Console.WriteLine();

        // Test actual access
        Console.WriteLine("--- Access Tests ---");

        // Read test
        try
        {
            using (var stream = File.OpenRead(filePath)) { }
            Console.WriteLine($"  Read:    ✓ Allowed");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"  Read:    ✗ Denied");
        }
        catch { }

        // Write test
        try
        {
            using (var stream = File.OpenWrite(filePath)) { }
            Console.WriteLine($"  Write:   ✓ Allowed");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"  Write:   ✗ Denied");
        }
        catch { }

        // Delete test
        try
        {
            // We won't actually delete, just check if we could
            Console.WriteLine($"  Delete:  ? (would need to test)");
        }
        catch { }

        // Get ACL info if available
        try
        {
            var security = File.GetAccessControl(filePath);
            var rules = security.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

            Console.WriteLine($"\n--- Access Control List ({rules.Count} rules) ---");
            foreach (FileSystemAccessRule rule in rules)
            {
                string identity = rule.IdentityReference.Value;
                string type = rule.AccessControlType == AccessControlType.Allow ? "Allow" : "Deny";
                string rights = rule.FileSystemRights.ToString();
                Console.WriteLine($"  {identity,-30} {type,-6} {rights}");
            }
        }
        catch
        {
            Console.WriteLine("\n  (Could not read ACL - may require elevated permissions)");
        }
    }

    static void CheckDirectoryPermissions(string dirPath)
    {
        Console.WriteLine($"\n--- Directory Permissions ---");
        Console.WriteLine($"  Path: {dirPath}");
        Console.WriteLine();

        // Test actual access
        Console.WriteLine("--- Access Tests ---");

        try
        {
            Directory.GetFiles(dirPath);
            Console.WriteLine($"  List:    ✓ Allowed");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"  List:    ✗ Denied");
        }

        string testFile = Path.Combine(dirPath, "_perm_test.tmp");
        try
        {
            File.WriteAllText(testFile, "test");
            Console.WriteLine($"  Write:   ✓ Allowed");
            File.Delete(testFile);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"  Write:   ✗ Denied");
        }
        catch
        {
            try { File.Delete(testFile); } catch { }
        }

        try
        {
            Directory.CreateDirectory(Path.Combine(dirPath, "_perm_test_dir"));
            Directory.Delete(Path.Combine(dirPath, "_perm_test_dir"));
            Console.WriteLine($"  Create:  ✓ Allowed");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"  Create:  ✗ Denied");
        }
        catch { }

        // List contents with permissions
        try
        {
            string[] files = Directory.GetFiles(dirPath);
            Console.WriteLine($"\n--- Contents ({files.Length} files) ---");
            foreach (string file in files)
            {
                var fileInfo = new FileInfo(file);
                string readOnly = fileInfo.IsReadOnly ? "R" : "W";
                Console.WriteLine($"  {readOnly} {fileInfo.Name,-30} {fileInfo.Length,10:N0} bytes");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n  Error listing contents: {ex.Message}");
        }
    }
}
