public class Income
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Source { get; set; }
}

public class Expense
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; }
}

public class Budget
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    public decimal Spent { get; set; }
    public string Category { get; set; }
}