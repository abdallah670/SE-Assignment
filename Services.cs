using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    public class BudgetService : IBudgetService
    {
        // Holds the current budget
        private Budget _budget=new Budget();
        private int Id = 0;
        public bool IsExceeded()
        {
            return _budget.isExceeded();
        }
        public void AddBudget(BudgetDTO budget)
        {
            if (budget == null)
            {
                Console.WriteLine("Budget is null");
                return;
            }
            if (Id == 0)
            {
                _budget._budgetDTO = budget;
                _budget.User = budget.User;
                BudgetRepositoryImpl budgetRepositoryImpl = new BudgetRepositoryImpl();
                budgetRepositoryImpl.Save(budget);
                Id++;
            }
            else
            {
               budget.Id=Id;
               _budget._budgetDTO = budget;
               _budget.User = _budget.User;
                BudgetRepositoryImpl budgetRepositoryImpl = new BudgetRepositoryImpl();
                budgetRepositoryImpl.Save(budget);
                Id++;
            }

        }

       public  decimal CalculateProgress()
        {
            return _budget.getprogress();
        }


    }
    public class IncomeService : IIncomeService
    {
        private List<IncomeDTO> _incomes = new List<IncomeDTO>();
        private IncomeRepositoryImpl IncomeRepositoryImpl = new IncomeRepositoryImpl();
        private int CurrentUserID;
        private int Id = 0;
        public void AddIncome(IncomeDTO income)
        {
            if (income == null)
            {
                Console.WriteLine("Income is null");
                return;
            }
            if (Id == 0)
            {
              
                IncomeRepositoryImpl.Save(income);
                CurrentUserID = income.User.Id;
                Id++;

            }

            else
            {
                income.Id= Id++;
             
                IncomeRepositoryImpl.Save(income);
                CurrentUserID = income.User.Id;
                Id++;
            }
        }
        public void MarkNonRecurring(IncomeDTO income)
        {
            if (income == null)
            {
                Console.WriteLine("Income is null");
                return;
            }
            _incomes = IncomeRepositoryImpl.FindByUser(income.User.Id);
            foreach (var inc in _incomes)
            {
                if (inc == income)
                {
                    inc.isRecurring = false;
                    Console.WriteLine("Income marked as non-recurring.");
                    CurrentUserID=inc.User.Id;
                    break;
                }

            }
        }
        public List<IncomeDTO> GetIncomes()
        {
            List<IncomeDTO> incomeList = IncomeRepositoryImpl.FindByUser(CurrentUserID);
            return incomeList;
        }
    }
    public class ExpenseService : IExpenseService
    {

        int CurrentUserID = -1;
        private int Id = 0;
        public void AddExpense(ExpenseDTO expense)
        {
            if (expense == null)
            {
                Console.WriteLine("Expense is null");
                return;
            }
            if (Id == 0)
            {
               
                ExpenseRepositoryImpl expenseRepositoryImpl = new ExpenseRepositoryImpl();
                expenseRepositoryImpl.Save(expense);
                CurrentUserID = expense.User.Id;
                Id++;
            }
            else
            {
                expense.Id = Id;
                ExpenseRepositoryImpl expenseRepositoryImpl = new ExpenseRepositoryImpl();
                expenseRepositoryImpl.Save(expense);
                CurrentUserID = expense.User.Id;
                Id++;
            }
           

        }
        public void MarkNonRecurring(ExpenseDTO expense)
        {
            if (expense == null)
            {
                Console.WriteLine("Expense is null");
                return;
            }
            List<ExpenseDTO> _expenses = new List<ExpenseDTO>();
            ExpenseRepositoryImpl expenseRepositoryImpl = new ExpenseRepositoryImpl();
            _expenses = expenseRepositoryImpl.FindByUser(expense.User.Id);
            foreach (var exp in _expenses)
            {
                if (exp == expense)
                {
                    exp.isRecurring = false;
                    Console.WriteLine("Expense marked as non-recurring.");
                    break;
                }
            }
        }
        public List<ExpenseDTO> GetExpenses()
        {
            List<ExpenseDTO> _expenses = new List<ExpenseDTO>();
            ExpenseRepositoryImpl expenseRepositoryImpl = new ExpenseRepositoryImpl();
            _expenses = expenseRepositoryImpl.FindByUser(CurrentUserID);
            return _expenses;

        }
    }
    public class AuthenticateService : IAuthenticateService
    {
        UserRepositoryImpl userRepository;
        public void register(string name, string email, string password, ref int Id)
        { 
            string file = "users.txt";
            string filepath = Path.Combine(Environment.CurrentDirectory, file);
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Close();
            }
            UserDTO user = new UserDTO();
            user.Name = name;
            user.Email = email;
            user.PasswordHash = HashPassword(password);
            user.Id = 0;
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "users.txt");
            string content = File.ReadAllText(FilePath);
            List<UserDTO> users = new List<UserDTO>();
            if (string.IsNullOrEmpty(content))
            {
                users = new List<UserDTO>();
            }
            else
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                        if (parts.Length == 4)
                        {
                            if(parts[2] == email)
                            {
                                Console.WriteLine("User already exists");
                                return;
                            }
                            UserDTO existingUser = new UserDTO
                            {
                                Id = int.Parse(parts[0]),
                                Name = parts[1],
                                Email = parts[2],
                                PasswordHash = parts[3]
                            };
                            users.Add(existingUser);
                        }
                    }
                }


            }
            user.Id = users.Count + 1;
            Id= user.Id;
            userRepository = new UserRepositoryImpl();
            userRepository.Save(user);

        }
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {

                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
        public User login(string email, string password)
        {
            string file = "users.txt";
            string filepath = Path.Combine(Environment.CurrentDirectory, file);
            UserDTO user = new UserDTO();
            user.Email = email;
            user.PasswordHash = HashPassword(password);
            string content = File.ReadAllText(filepath);
            List<UserDTO> users = new List<UserDTO>();
            foreach (string line in content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                if (parts.Length == 4)
                {
                    UserDTO existingUser = new UserDTO
                    {
                        Id = int.Parse(parts[0]),
                        Name = parts[1],
                        Email = parts[2],
                        PasswordHash = parts[3]
                    };
                    users.Add(existingUser);
                }
            }

            foreach (var u in users)
            {
                if (u.Email == email)
                {
                    if (u.PasswordHash == user.PasswordHash)
                    {
                        user.Id = u.Id;
                        user.Name = u.Name;
                        user.PasswordHash = password;
                        return new User(u);
                    }
                    else
                    {
                        Console.WriteLine("Invalid password");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("User not found");
                }
            }
            return null;

        }

      
    }
    public class ReminderService : IReminderService
    {


        private int Id = 0;
        public void ScheduleReminder(ReminderDTO reminder)
        {
            if (reminder == null)
            {
                Console.WriteLine("Reminder is null");
                return;
            }
            if (Id == 0)
            {
                
                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "reminders.txt");
                IReminderRepository reminderRepository = new ReminderReopsitoryImpl();
                reminderRepository.Save(reminder);
                Id++;
            }
            else
            {
                reminder.Id = Id;
             
                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "reminders.txt");
                IReminderRepository reminderRepository = new ReminderReopsitoryImpl();
                reminderRepository.Save(reminder);
                Id++;
            }
        }
        public void Notify(int userId, string message)
        {
            if (userId <= 0)
            {
                Console.WriteLine("Invalid user ID");
                return;
            }
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "remindernotification.txt");
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
            }
            ReminderDTO reminder = new ReminderDTO();
            reminder.Id = userId;
            reminder.Message = message;
            reminder.Date = DateTime.Now;
            reminder.User = new UserDTO();
            reminder.User.Id = userId;
            using(StreamWriter writer = new StreamWriter(FilePath, true))
            {
                writer.WriteLine($"{reminder.User.Id}#//# {reminder.Message}#//# {reminder.Date}#//#");
            }
            Console.WriteLine("Notification sent successfully.");
        }
    }

}

