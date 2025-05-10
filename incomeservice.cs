public class IncomeService : IIncomeService
{
    // Internal list to store income entries
    private readonly List<Income> _incomes = new List<Income>();

    // Adds a new income entry
    public void AddIncome(Income income)
    {
        if (income == null)
            throw new ArgumentNullException(nameof(income));
        
        _incomes.Add(income);
        // In a real application, this would be saved via a repository or database
    }

    // Retrieves all income entries
    public List<Income> GetIncomes()
    {
        return _incomes;
    }
}