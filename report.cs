using System;
using System.Collections.Generic;
using System.Linq;

// ENUM
public enum TransactionType
{
    Income,
    Expense
}

// MODELS
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Transaction
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public string Category { get; set; }
}

public class Budget
{
    public decimal Limit { get; set; }
    public decimal Spent { get; set; }

    public bool IsExceeded()
    {
        return Spent > Limit;
    }
}

// REPOSITORY INTERFACES
public interface ITransactionRepository
{
    List<Transaction> GetTransactionsByUser(int userId);
}

public interface IBudgetRepository
{
    Budget GetBudgetByUser(int userId);
}

// FAKE REPOSITORY IMPLEMENTATIONS
public class FakeTransactionRepository : ITransactionRepository
{
    public List<Transaction> GetTransactionsByUser(int userId)
    {
        return new List<Transaction>
        {
            new Transaction { Amount = 1000, Date = DateTime.Now.AddDays(-10), Type = TransactionType.Income, Category = "Salary" },
            new Transaction { Amount = 200, Date = DateTime.Now.AddDays(-5), Type = TransactionType.Expense, Category = "Food" },
            new Transaction { Amount = 150, Date = DateTime.Now.AddDays(-3), Type = TransactionType.Expense, Category = "Transport" },
        };
    }
}

public class FakeBudgetRepository : IBudgetRepository
{
    public Budget GetBudgetByUser(int userId)
    {
        return new Budget
        {
            Limit = 500,
            Spent = 350
        };
    }
}

// CORE CLASSES
public class FinancialReport
{
    public User User { get; set; }
    public List<Transaction> Transactions { get; set; }
    public Budget Budget { get; set; }

    public FinancialReport(User user, List<Transaction> transactions, Budget budget)
    {
        User = user;
        Transactions = transactions;
        Budget = budget;
    }

    public decimal GetTotalIncome()
    {
        return Transactions
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount);
    }

    public decimal GetTotalExpenses()
    {
        return Transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);
    }

    public decimal GetBalance()
    {
        return GetTotalIncome() - GetTotalExpenses();
    }

    public string GetBudgetStatus()
    {
        return Budget.IsExceeded() ? "Exceeded" : "Within Budget";
    }

    public string GenerateSummary()
    {
        return $"User: {User.Name}\n" +
               $"Total Income: {GetTotalIncome()} EGP\n" +
               $"Total Expenses: {GetTotalExpenses()} EGP\n" +
               $"Balance: {GetBalance()} EGP\n" +
               $"Budget Status: {GetBudgetStatus()}";
    }
}

public class ReportingService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;

    public ReportingService(ITransactionRepository transactionRepository, IBudgetRepository budgetRepository)
    {
        _transactionRepository = transactionRepository;
        _budgetRepository = budgetRepository;
    }

    public FinancialReport GenerateReport(User user)
    {
        var transactions = _transactionRepository.GetTransactionsByUser(user.Id);
        var budget = _budgetRepository.GetBudgetByUser(user.Id);
        return new FinancialReport(user, transactions, budget);
    }
}

// MAIN PROGRAM
public class Program
{
    public static void Main()
    {
        // Simulate a user
        var user = new User { Id = 1, Name = "Ali Mohamed" };

        // Services with fake repositories
        var transactionRepo = new FakeTransactionRepository();
        var budgetRepo = new FakeBudgetRepository();
        var reportService = new ReportingService(transactionRepo, budgetRepo);

        // Generate and display report
        var report = reportService.GenerateReport(user);
        Console.WriteLine(report.GenerateSummary());
    }
}