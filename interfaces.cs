public interface IIncomeService
{
    void AddIncome(Income income);
    List<Income> GetIncomes();
}

public interface IExpenseService
{
    void AddExpense(Expense expense);
    List<Expense> GetExpenses();
}

public interface IBudgetService
{
    void AddBudget(Budget budget);
    decimal CalculateProgress();
}