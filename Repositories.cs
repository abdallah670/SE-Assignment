using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    public class IncomeRepositoryImpl : IIncomeRepository
    {

        public void Save(IncomeDTO income)
        {
            if (income == null)
            {
                Console.WriteLine("Income is null");
                return;
            }
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "incomes.txt");
            if (!File.Exists(Filepath))
            {
                File.Create(Filepath).Close();
                income.Id = 0;
            }
            else
            {
                List<IncomeDTO> incomes = new List<IncomeDTO>();
                using (StreamReader reader = new StreamReader(Filepath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                        if (parts.Length == 5)
                        {
                            IncomeDTO existingIncome = new IncomeDTO
                            {
                                Id = int.Parse(parts[0]),
                                amount = decimal.Parse(parts[1]),
                                date = DateTime.Parse(parts[2]),
                                isRecurring = bool.Parse(parts[3]),
                            };
                            incomes.Add(existingIncome);
                        }
                    }
                }
                income.Id = incomes.Count + 1;
            }
            string message = $"{income.Id}#//#{income.Source}#//#{income.amount}#//#{income.date}#//#{income.isRecurring}#//#{income.User.Id}";
            using (StreamWriter writer = new StreamWriter(Filepath, true))
            {
                writer.WriteLine(message);
            }
            Console.WriteLine("Income saved successfully.");
        }

        public List<IncomeDTO> FindByUser(int userId)
        {
            if (userId <= 0)
            {
                Console.WriteLine("Invalid user ID");
                return null;
            }
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "incomes.txt");
            List<IncomeDTO> incomes = new List<IncomeDTO>();
            using (StreamReader reader = new StreamReader(Filepath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                    if (parts.Length == 6)
                    {

                        if (userId == int.Parse(parts[5]))
                        {
                            IncomeDTO existingIncome = new IncomeDTO
                            {

                                Id = int.Parse(parts[0]),
                                Source = parts[1],
                                amount = decimal.Parse(parts[2]),
                                date = DateTime.Parse(parts[3]),
                                isRecurring = bool.Parse(parts[4]),
                                User = new UserDTO
                                {
                                    Id = int.Parse(parts[5])
                                }

                            };
                            incomes.Add(existingIncome);
                        }
                       
                    }
                }
            }
            return incomes;
        }

    }
    public class ExpenseRepositoryImpl : IExpenseRepository
    {
        public void Save(ExpenseDTO expense)
        {
            if (expense == null)
            {
                Console.WriteLine("Expense is null");
                return;
            }
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "expenses.txt");
            if (!File.Exists(Filepath))
            {
                File.Create(Filepath).Close();
                expense.Id = 0;
            }
            else
            {
                List<ExpenseDTO> expenses = new List<ExpenseDTO>();
                using (StreamReader reader = new StreamReader(Filepath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                        if (parts.Length == 6)
                        {
                            ExpenseDTO existingExpense = new ExpenseDTO
                            {
                                Id = int.Parse(parts[0]),
                                amount = decimal.Parse(parts[2]),
                                category = parts[1],
                                date = DateTime.Parse(parts[3]),
                                isRecurring = bool.Parse(parts[4]),
                                User = new UserDTO
                                {
                                    Id = int.Parse(parts[5])
                                }

                            };
                            expenses.Add(existingExpense);
                        }
                    }
                }
                expense.Id = expenses.Count + 1;
            }
            string message = $"{expense.Id}#//#{expense.category}#//#{expense.amount}#//#{expense.date}#//#{expense.isRecurring}#//#{expense.User.Id}";
            using (StreamWriter writer = new StreamWriter(Filepath, true))
            {
                writer.WriteLine(message);
            }
            Console.WriteLine("Expense saved successfully.");
        }

        public List<ExpenseDTO> FindByUser(int userId)
        {
            if (userId <= 0)
            {
                Console.WriteLine("Invalid user ID");
                return null;
            }

            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "expenses.txt");
            List<ExpenseDTO> expenses = new List<ExpenseDTO>();
            using (StreamReader reader = new StreamReader(Filepath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                    if (parts.Length == 6)
                    {
                        ExpenseDTO existingExpense = new ExpenseDTO
                        {
                            Id = int.Parse(parts[0]),
                            category = parts[1],

                            amount = decimal.Parse(parts[2]),
                            date = DateTime.Parse(parts[3]),
                            isRecurring = bool.Parse(parts[4]),
                            User = new UserDTO
                            {
                                Id = int.Parse(parts[5])
                            }
                        };
                        expenses.Add(existingExpense);
                    }
                }
            }
            var userExpenses = expenses.Where(e => e.User.Id == userId).ToList();
            return userExpenses;
        }
    }
    public class UserRepositoryImpl : IUserRepository
    {
        public void Save(UserDTO user)
        {
            if (user == null)
            {
                Console.WriteLine("User is null");
                return;
            }
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "users.txt");
            if (!File.Exists(Filepath))
            {
                File.Create(Filepath).Close();
                user.Id = 0;

            }
            else
            {
                List<UserDTO> users = new List<UserDTO>();
                using (StreamReader reader = new StreamReader(Filepath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                        if (parts.Length == 4)
                        {
                            if (parts[2] == user.Email)
                            {

                                user.Id = int.Parse(parts[0]);
                                user.Name = parts[1];
                                user.Email = parts[2];
                                File.Delete(Filepath);
                                using (StreamWriter writer = new StreamWriter(Filepath))
                                {
                                    writer.WriteLine($"{user.Id}#//#{user.Name}#//#{user.Email}#//#{user.PasswordHash}");
                                }
                                return;
                            }
                            else
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
                    }
                }
                user.Id = users.Count + 1;
            }
            string message = $"{user.Id}#//#{user.Name}#//#{user.Email}#//#{user.PasswordHash}";
            using (StreamWriter writer = new StreamWriter(Filepath, true))
            {
                writer.WriteLine(message);
            }
            Console.WriteLine("User saved successfully.");
        }

        public UserDTO findById(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("Invalid user ID");
                return null;
            }
            List<UserDTO> users = new List<UserDTO>();
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "users.txt");
            using (StreamReader reader = new StreamReader(Filepath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
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
            }
            var user = users.FirstOrDefault(u => u.Id == id);
            return user;
        }
    }
    public class ReminderReopsitoryImpl : IReminderRepository
    {
        public void Save(ReminderDTO reminder)
        {
            if (reminder == null)
            {
                Console.WriteLine("Reminder is null");
                return;
            }
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "reminders.txt");
            if (!File.Exists(Filepath))
            {
                File.Create(Filepath).Close();
                reminder.Id = 0;
            }
            else
            {
                List<ReminderDTO> reminders = new List<ReminderDTO>();
                using (StreamReader reader = new StreamReader(Filepath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                        if (parts.Length == 4)
                        {
                            ReminderDTO existingReminder = new ReminderDTO
                            {
                                Id = int.Parse(parts[0]),
                                Message = parts[1],
                                Date = DateTime.Parse(parts[2]),
                                User = new UserDTO
                                {
                                    Id = int.Parse(parts[3])
                                }
                            };
                            reminders.Add(existingReminder);
                        }
                    }
                }
                reminder.Id = reminders.Count + 1;
            }
            string message = $"{reminder.Id}#//#{reminder.Message}#//#{reminder.Date}#//#{reminder.User.Id}";
            using (StreamWriter writer = new StreamWriter(Filepath, true))
            {
                writer.WriteLine(message);
            }
            Console.WriteLine("Reminder saved successfully.");
        }

        public List<ReminderDTO> FindByUser(int userId)
        {
            if (userId <= 0)
            {
                Console.WriteLine("Invalid user ID");
                return null;
            }
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "reminders.txt");
            List<ReminderDTO> reminders = new List<ReminderDTO>();
            using (StreamReader reader = new StreamReader(Filepath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                    if (parts.Length == 4)
                    {
                        ReminderDTO existingReminder = new ReminderDTO
                        {
                            Id = int.Parse(parts[0]),
                            Message = parts[1],
                            Date = DateTime.Parse(parts[2]),
                            User = new UserDTO
                            {
                                Id = int.Parse(parts[3])
                            },

                        };
                        reminders.Add(existingReminder);
                    }


                }

            }
            var userReminders = reminders.Where(r => r.User.Id == userId).ToList();
            return userReminders;

        }
    }
    public class BudgetRepositoryImpl : IBudgeRepository
    {
        public LinkedList<BudgetDTO> FindByUser(int userId)
        {
            if (userId <= 0)
            {
                Console.WriteLine("Invalid user ID");
                return null;
            }
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "budgets.txt");
            LinkedList<BudgetDTO> budgets = new LinkedList<BudgetDTO>();
            using (StreamReader reader = new StreamReader(Filepath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                    if (parts.Length == 7)
                    {
                        BudgetDTO existingBudget = new BudgetDTO
                        {
                            Id = int.Parse(parts[0]),
                            category = parts[1],
                            periodStart = DateTime.Parse(parts[2]),
                            periodEnd = DateTime.Parse(parts[3]),
                            limit = decimal.Parse(parts[4]),
                            spent = decimal.Parse(parts[5]),
                            User = new UserDTO
                            {
                                Id = int.Parse(parts[6])
                            }
                        };
                        budgets.AddLast(existingBudget);
                    }
                }
            }
            return budgets;

        }

        public void Save(BudgetDTO budget)
        {
            if (budget == null)
            {
                Console.WriteLine("budget is null");
                return;
            }
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "budgets.txt");
            if (!File.Exists(Filepath))
            {
                File.Create(Filepath).Close();
                budget.Id = 0;

            }
            else
            {
                List<BudgetDTO> list = new List<BudgetDTO>();
                using (StreamReader reader = new StreamReader(Filepath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { "#//#" }, StringSplitOptions.None);
                        if (parts.Length == 5)
                        {
                            BudgetDTO budgetDTO = new BudgetDTO
                            {
                                Id = int.Parse(parts[0]),
                                category = parts[1],
                                periodStart = DateTime.Parse(parts[2]),
                                periodEnd = DateTime.Parse(parts[3]),
                                limit = decimal.Parse(parts[4]),
                                spent = decimal.Parse(parts[5]),
                                User = new UserDTO
                                {
                                    Id = int.Parse(parts[6]),
                                }
                            };
                            list.Add(budgetDTO);
                        }
                    }
                }
                budget.Id = list.Count + 1;
                string Message=$"{budget.Id}#//#{budget.category}#//#{budget.periodStart}#//#{budget.periodEnd}#//#{budget.limit}#//#{budget.spent}#//#{budget.User.Id}";
                using (StreamWriter writer = new StreamWriter(Filepath, true))
                {
                    writer.WriteLine(Message);
                }
                Console.WriteLine("Budget saved successfully.");
            }
        }
    }
}