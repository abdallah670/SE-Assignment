using System.Collections.Generic;

namespace Personal_Bugeting{
    public interface IIncomeService
    {
        void AddIncome(IncomeDTO income);
        List<IncomeDTO> GetIncomes();
    }
    public interface IReminderService
    {
        void ScheduleReminder(ReminderDTO reminder);
        void Notify(int userId, string Message);

    }
    public interface IExpenseService
    {
        void AddExpense(ExpenseDTO expense);
        List<ExpenseDTO> GetExpenses();
    }

    public interface IBudgetService
    {
        void AddBudget(BudgetDTO budget);
        decimal CalculateProgress();
    }
    public interface IAuthenticateService
    {

        void register(string name, string email, string password,ref int Id);
        User login(string email, string password);
    }
    public interface IUserRepository
    {
        void Save(UserDTO user);
        UserDTO findById(int id);
    }
    public interface IIncomeRepository
    {
        void Save(IncomeDTO income);
        List<IncomeDTO> FindByUser(int userId);
    }
    public interface IExpenseRepository
    {
        void Save(ExpenseDTO expense);
        List<ExpenseDTO> FindByUser(int userId);
    }
 
    public interface IReminderRepository {
        void Save(ReminderDTO reminder);
        List<ReminderDTO> FindByUser(int userId);

    }
    public interface IBudgeRepository
    {
        void Save(BudgetDTO budget);
        LinkedList<BudgetDTO> FindByUser(int userId);
    }

}