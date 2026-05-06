// Program: Primary Constructor Bank Account
// Description: Demonstrates C# 12 primary constructors for classes

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Bank Account (Primary Constructors - C# 12) ===\n");

        // Create accounts using primary constructors
        var account1 = new BankAccount("Alice", "CHK-001", 1000m);
        var account2 = new SavingsAccount("Bob", "SAV-001", 5000m, 0.035m);

        Console.WriteLine("--- Account Creation ---");
        Console.WriteLine(account1);
        Console.WriteLine(account2);

        // Operations
        account1.Deposit(500m);
        account1.Withdraw(200m);
        account2.Deposit(1000m);
        account2.Withdraw(100m);
        account2.ApplyInterest();

        Console.WriteLine("\n--- After Operations ---");
        Console.WriteLine(account1);
        Console.WriteLine(account2);

        // Transfer between accounts
        account1.TransferTo(account2, 300m);
        Console.WriteLine("\n--- After Transfer ($300 from Alice to Bob) ---");
        Console.WriteLine(account1);
        Console.WriteLine(account2);

        // Demonstrate encapsulation - balance is read-only through property
        Console.WriteLine("\n--- Encapsulation ---");
        Console.WriteLine($"  Alice's account number: {account1.AccountNumber}");
        Console.WriteLine($"  Alice's balance (via property): ${account1.Balance}");
        Console.WriteLine($"  Cannot directly set balance - must use Deposit/Withdraw");

        // Transaction history
        Console.WriteLine("\n--- Transaction History ---");
        Console.WriteLine($"  Alice's transactions:");
        foreach (var txn in account1.GetTransactionHistory())
        {
            Console.WriteLine($"    {txn.Timestamp:HH:mm:ss} | {txn.Type,-10} | {txn.Amount,10:C} | Balance: {txn.BalanceAfter,10:C}");
        }
    }
}

// Primary constructor (C# 12)
class BankAccount(string ownerName, string accountNumber, decimal initialBalance)
{
    // Properties initialized from primary constructor parameters
    public string OwnerName { get; } = ownerName;
    public string AccountNumber { get; } = accountNumber;
    public decimal Balance { get; private set; } = initialBalance;

    private readonly List<Transaction> _history = new()
    {
        new Transaction("Opening", initialBalance, initialBalance)
    };

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.");

        Balance += amount;
        _history.Add(new Transaction("Deposit", amount, Balance));
        Console.WriteLine($"  {OwnerName}: Deposited {amount:C}. New balance: {Balance:C}");
    }

    public virtual void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.");

        if (amount > Balance)
        {
            Console.WriteLine($"  {OwnerName}: Insufficient funds. Requested: {amount:C}, Available: {Balance:C}");
            return;
        }

        Balance -= amount;
        _history.Add(new Transaction("Withdrawal", amount, Balance));
        Console.WriteLine($"  {OwnerName}: Withdrew {amount:C}. New balance: {Balance:C}");
    }

    public void TransferTo(BankAccount target, decimal amount)
    {
        Console.WriteLine($"\n  Transferring {amount:C} from {OwnerName} to {target.OwnerName}");
        Withdraw(amount);
        target.Deposit(amount);
    }

    public List<Transaction> GetTransactionHistory() => new(_history);

    public override string ToString() =>
        $"{OwnerName} ({AccountNumber}): Balance = {Balance:C}";
}

// Inherited class with its own primary constructor
class SavingsAccount(string owner, string acctNum, decimal initial, decimal rate)
    : BankAccount(owner, acctNum, initial)
{
    public decimal InterestRate { get; } = rate;

    public void ApplyInterest()
    {
        decimal interest = Balance * InterestRate;
        Deposit(interest);
        Console.WriteLine($"  {OwnerName}: Interest applied: {interest:C} at {InterestRate:P2}");
    }
}

record Transaction(string Type, decimal Amount, decimal BalanceAfter)
{
    public DateTime Timestamp { get; } = DateTime.Now;
}
