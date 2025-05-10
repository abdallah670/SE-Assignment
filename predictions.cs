using System;
using System.Collections.Generic;
using System.Linq;

// Prediction mode enum (optional)
public enum PredictionMode
{
    Linear,
    MachineLearning
}

// Basic transaction model
public class Transaction
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
}

public enum TransactionType
{
    Income,
    Expense
}

// Prediction strategy interface
public interface IPredictionStrategy
{
    decimal PredictNextMonthExpense(List<Transaction> transactions);
}

// Linear regression strategy (average of past months)
public class LinearRegressionStrategy : IPredictionStrategy
{
    public decimal PredictNextMonthExpense(List<Transaction> transactions)
    {
        var monthlyExpenses = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .Select(g => g.Sum(t => t.Amount))
            .ToList();

        if (monthlyExpenses.Count == 0) return 0;

        return monthlyExpenses.Average();
    }
}

// Mock ML strategy using trend from last 3 expenses
public class MachineLearningStrategy : IPredictionStrategy
{
    public decimal PredictNextMonthExpense(List<Transaction> transactions)
    {
        var expenses = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .OrderBy(t => t.Date)
            .ToList();

        if (expenses.Count < 3)
            return expenses.Sum(e => e.Amount) / expenses.Count;

        var last3 = expenses.TakeLast(3).Select(t => t.Amount).ToList();
        var trend = (last3[2] - last3[0]) / 2;
        return last3[2] + trend;
    }
}

// Prediction service with strategy pattern
public class PredictionService
{
    private IPredictionStrategy _strategy;

    // Set prediction strategy
    public void SetStrategy(IPredictionStrategy strategy)
    {
        _strategy = strategy;
    }

    // Predict using selected strategy
    public decimal Predict(List<Transaction> transactions)
    {
        if (_strategy == null)
            throw new InvalidOperationException("Strategy not set.");

        return _strategy.PredictNextMonthExpense(transactions);
    }
}