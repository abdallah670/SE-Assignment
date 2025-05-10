public class BudgetService : IBudgetService
{
    // Holds the current budget
    private Budget _budget;

    // Sets the budget
    public void AddBudget(Budget budget)
    {
        if (budget == null)
            throw new ArgumentNullException(nameof(budget));

        _budget = budget;
    }

    // Calculates the percentage of the budget used
    public decimal CalculateProgress()
    {
        if (_budget == null || _budget.Total == 0)
            return 0;

        return (_budget.Spent / _budget.Total) * 100;
    }
}