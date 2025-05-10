public class ExpenseService : IExpenseService
{
    // Internal list to store expense entries
    private readonly List<Expense> _expenses = new List<Expense>();

    // Adds a new expense entry
    public void AddExpense(Expense expense)
    {
        if (expense == null)
            throw new ArgumentNullException(nameof(expense));

        _expenses.Add(expense);
    }

    // Retrieves all expense entries
    public List<Expense> GetExpenses()
    {
        return _expenses;
    }
}