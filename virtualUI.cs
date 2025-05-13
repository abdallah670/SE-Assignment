using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    internal class virtualUI
    {
        //This class is used to create a virtual UI for the application
        private static BudgetService _budgetService = new BudgetService();
        private static IncomeService _incomeService = new IncomeService();
        private static ExpenseService _expenseService = new ExpenseService();
        private static ReminderService _reminderService = new ReminderService();
        private static User _userService;
        private static AuthenticateService _authenticateService = new AuthenticateService();
        private static UserDTO userDTO = new UserDTO();
        private static ReminderReopsitoryImpl _reminderReopsitoryImpl = new ReminderReopsitoryImpl();
        private static UserRepositoryImpl _userRepositoryImpl = new UserRepositoryImpl();
        private static void FirstUserStory()//Sign up
        {
            Console.WriteLine("Sign up page");
            int Id = -1;
            while (Id < 0)
            {
                Console.WriteLine("Please enter your name:");
                string name = Console.ReadLine();
                Console.WriteLine(
                    "Please enter your email address:");
                string email = Console.ReadLine();
                Console.WriteLine("Please enter your password:");
                string password = Console.ReadLine();
                Console.WriteLine(
                    "Please confirm your password:");
                string confirmPassword = Console.ReadLine();

                if (password != confirmPassword)
                {
                    Console.WriteLine("Passwords do not match. Please try again.");
                    return;
                }
                _authenticateService.register(name, email, password, ref Id);
                if (Id > -1)
                {

                    userDTO.Name = name;
                    userDTO.Email = email;
                    userDTO.PasswordHash = password;
                    userDTO.Id = Id;
                    _userService = new User(userDTO);
                    if (_userService.authenticate())
                    {
                        Console.WriteLine("User registered successfully.");
                        break;
                    }

                }
                else
                {
                    Console.WriteLine("Try again.");
                }

            }
        }
        private static void SecondUserStory()//Log In
        {
            Console.WriteLine("Log in Page");
            Console.WriteLine("Please enter your email address:");
            string email = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();
            _userService = new User(email, password);
            if (_userService.authenticate())
            {
                userDTO = _userService.user;
                Console.WriteLine("User Logged in successfully.");
            }
            else
            {
                return;
            }
        }
        //Track all Incomes
        private static void PrintncomeList(List<IncomeDTO> incomeList)
        {
            Console.WriteLine("All Incomes:");
            foreach (var income in incomeList)
            {
                Console.WriteLine($"Source: {income.Source}, Amount: {income.amount}, Date: {income.date}");
            }
        }
        private static void ThirdUserStory()//Track Income
        {
            //User can track income
            IncomeDTO incomeDTO = new IncomeDTO();
            Console.WriteLine("Income Tracking Page");
          

           
                Console.Write("What is the source of income? ");
                incomeDTO.Source = Console.ReadLine();

            // Prompt for the amount
                decimal amount = 0;
                Console.Write("What is the amount? ");
                while (!decimal.TryParse(Console.ReadLine(), out amount))
                {
                    Console.WriteLine("Invalid input. Please enter a valid decimal value for the amount.");
                    Console.Write("What is the amount? ");
                }
                incomeDTO.amount= amount;
                // Prompt for the date
                Console.Write("Enter the date of income (yyyy-MM-dd): ");
                DateTime dateTime = DateTime.Now;
                while (!DateTime.TryParse(Console.ReadLine(), out dateTime))
                {
                    Console.WriteLine("Invalid input. Please enter a valid date in the format yyyy-MM-dd.");
                    Console.Write("Enter the date of income (yyyy-MM-dd): ");
                }
                 incomeDTO.date = dateTime;
                // Prompt for whether the income is recurring
                Console.Write("Is this income recurring? (y/n): ");
                string isRecurringInput = Console.ReadLine();
                incomeDTO.isRecurring = isRecurringInput.Equals("y", StringComparison.OrdinalIgnoreCase);

                // Associate the income with the current user
                incomeDTO.User = userDTO;

                // Add the income to the service
                _incomeService.AddIncome(incomeDTO);

                // Display the entered income details
                Console.WriteLine("Income details:");
                Console.WriteLine($"Source: {incomeDTO.Source}, Amount: {incomeDTO.amount}, Date: {incomeDTO.date}, Recurring: {incomeDTO.isRecurring}");


            //IncomeDTO incomeDTO = new IncomeDTO();
            //incomeDTO.amount = 1000;
            //incomeDTO.date = DateTime.Now;
            //incomeDTO.isRecurring = true;
            //incomeDTO.Source = "Salary";
            //incomeDTO.User = userDTO;
            //_incomeService.AddIncome(incomeDTO);
            //Console.WriteLine($"Income details");
            //Console.WriteLine($"Income Source: {incomeDTO.Source}, Amount:{incomeDTO.amount}");
            ////another income
            //incomeDTO.amount += 1000;
            //incomeDTO.date = DateTime.Now;
            //incomeDTO.isRecurring = false;
            //incomeDTO.Source = "Bonus";
            //incomeDTO.User = userDTO;
            //_incomeService.AddIncome(incomeDTO);
            //Console.WriteLine($"Income details");
          //  Console.WriteLine($"Source: {incomeDTO.Source}, Amount: {incomeDTO.amount}, Date: {incomeDTO.date}, Recurring: {incomeDTO.isRecurring}");
            if (incomeDTO.Id > 1)
            {
                Console.WriteLine("Do you want to Show all incomes?y/n");
                string Answer = Console.ReadLine();
                if (Answer == "y")
                {
                    List<IncomeDTO> incomeList = _incomeService.GetIncomes();
                    PrintncomeList(incomeList);
                }
            }
        }
        //4 User Story Budget
        private static void FourthUserStory() //Tracking Budgeting
        {
            Console.WriteLine("Bugeting Page");
           
           BudgetDTO budgetDTO = new BudgetDTO();
            // Prompt for the budget category
            Console.Write("Enter the budget category: ");
            budgetDTO.category = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(budgetDTO.category))
            {
                Console.WriteLine("Category cannot be empty. Please enter a valid category.");
                budgetDTO.category = Console.ReadLine();
            }

            // Prompt for the start date
            DateTime dateTime = DateTime.Now;
            Console.Write("Enter the start date of the budget (yyyy-MM-dd): ");
            while (!DateTime.TryParse(Console.ReadLine(), out dateTime))
            {
                Console.WriteLine("Invalid input. Please enter a valid date in the format yyyy-MM-dd.");
                Console.Write("Enter the start date of the budget (yyyy-MM-dd): ");
            }
            budgetDTO.periodStart= dateTime;
            // Prompt for the end date
            Console.Write("Enter the end date of the budget (yyyy-MM-dd): ");
            while (!DateTime.TryParse(Console.ReadLine(), out dateTime ) || budgetDTO.periodEnd <= budgetDTO.periodStart)
            {
                Console.WriteLine("Invalid input. The end date must be after the start date.");
                Console.Write("Enter the end date of the budget (yyyy-MM-dd): ");
            }
            budgetDTO.periodEnd= dateTime;
            // Prompt for the budget limit
            decimal limit = 0;
            Console.Write("Enter the budget limit: ");
            while (!decimal.TryParse(Console.ReadLine(), out limit ) || budgetDTO.limit <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive decimal value for the budget limit.");
                Console.Write("Enter the budget limit: ");
            }
            budgetDTO.limit= limit;
            // Prompt for the amount already spent
            decimal spent = 0;
            Console.Write("Enter the amount already spent: ");
            while (!decimal.TryParse(Console.ReadLine(), out spent) || budgetDTO.spent < 0 || budgetDTO.spent > budgetDTO.limit)
            {
                Console.WriteLine("Invalid input. The spent amount must be a positive value and less than or equal to the budget limit.");
                Console.Write("Enter the amount already spent: ");
            }
            budgetDTO.spent = spent;

            // Associate the budget with the current user
            budgetDTO.User = userDTO;

            // Add the budget to the service
            _budgetService.AddBudget(budgetDTO);

            // Display the entered budget details
            Console.WriteLine("Budget details:");
            Console.WriteLine($"Category: {budgetDTO.category}");
            Console.WriteLine($"Start Date: {budgetDTO.periodStart}");
            Console.WriteLine($"End Date: {budgetDTO.periodEnd}");
            Console.WriteLine($"Limit: {budgetDTO.limit}");
            Console.WriteLine($"Spent: {budgetDTO.spent}");

            // Display the progress of the budget
            Console.WriteLine("Progress: " + _budgetService.CalculateProgress() + "%");








            //BudgetDTO budgetDTO = new BudgetDTO();
            //budgetDTO.category = "income";
            //budgetDTO.periodStart = DateTime.Now;
            //budgetDTO.periodEnd = DateTime.Now.AddDays(2);
            //budgetDTO.limit = 1000;
            //budgetDTO.spent = 500;
            //budgetDTO.User = userDTO;
            //_budgetService.AddBudget(budgetDTO);
            //Console.WriteLine("Budget detalis");
            //Console.WriteLine($"Category: {budgetDTO.category}, StartPeriod: {budgetDTO.periodStart}, EndPeriod: {budgetDTO.periodEnd}");
            //Console.WriteLine("Progress: " + _budgetService.CalculateProgress() + "%");
        }
        //5 User Story Reminder
        //Print Notifications
        private static void PrintNoti()
        {

            List<ReminderDTO> reminderList = _reminderReopsitoryImpl.FindByUser(userDTO.Id);
            Console.WriteLine("Notifications");
            foreach(var i in reminderList) {

                Console.WriteLine($"Message: {i.Message}, Date: {i.Date}");
            }
        }

        private static void FifthUserStory()//Reminder
        {
            Console.WriteLine("Reminder Page");
         
                ReminderDTO reminderDTO = new ReminderDTO();

                // Prompt for the reminder message
                Console.Write("Enter the reminder message: ");
                reminderDTO.Message = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(reminderDTO.Message))
                {
                    Console.WriteLine("Message cannot be empty. Please enter a valid reminder message.");
                    Console.Write("Enter the reminder message: ");
                    reminderDTO.Message = Console.ReadLine();
                }

                // Prompt for the reminder date
                Console.Write("Enter the reminder date (yyyy-MM-dd): ");
                DateTime reminderDate;
                while (!DateTime.TryParse(Console.ReadLine(), out reminderDate) || reminderDate < DateTime.Now)
                {
                    Console.WriteLine("Invalid input. The reminder date must be a valid future date in the format yyyy-MM-dd.");
                    Console.Write("Enter the reminder date (yyyy-MM-dd): ");
                }
                reminderDTO.Date = reminderDate;

                // Associate the reminder with the current user
                reminderDTO.User = userDTO;

                // Schedule the reminder
                _reminderService.ScheduleReminder(reminderDTO);

                // Simulate a notification if the reminder date is today
                if (reminderDTO.Date.Date == DateTime.Now.Date)
                {
                    _reminderService.Notify(userDTO.Id, "Reminder: " + reminderDTO.Message);
                }

                // Print all notifications for the user
                PrintNoti();
            

            //ReminderDTO reminderDTO = new ReminderDTO();
            //reminderDTO.Message = "Pay your bills";
            //reminderDTO.Date = DateTime.Now.AddDays(7);
            //reminderDTO.User = userDTO;
            //_reminderService.ScheduleReminder(reminderDTO);
            //Console.WriteLine("Suppose The remider day is today");
            //_reminderService.Notify(userDTO.Id, "you should pay your bell now");
            //PrintNoti();

        }
        //Senventh UserStory
        private static void PrintExpenseList(List<ExpenseDTO> expenseList)
        {
            Console.WriteLine("All Expenses:");
            foreach (var expense in expenseList)
            {
                Console.WriteLine($"Category: {expense.category}, Amount: {expense.amount}, Date: {expense.date}, Recurring: {expense.isRecurring}");
            }
        }
        private static void SeventhUserStory()//Expense Tracking
        {
            Console.WriteLine("Expense Page");
          
                ExpenseDTO expenseDTO = new ExpenseDTO();

                // Prompt for the expense category
                Console.Write("Enter the expense category: ");
                expenseDTO.category = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(expenseDTO.category))
                {
                    Console.WriteLine("Category cannot be empty. Please enter a valid category.");
                    Console.Write("Enter the expense category: ");
                    expenseDTO.category = Console.ReadLine();
                }

                // Prompt for the expense amount
                decimal amount = 0;
                Console.Write("Enter the expense amount: ");
                while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a positive decimal value for the amount.");
                    Console.Write("Enter the expense amount: ");
                }
                expenseDTO.amount = amount;

                // Prompt for the expense date
                Console.Write("Enter the expense date (yyyy-MM-dd): ");
                DateTime expenseDate;
                while (!DateTime.TryParse(Console.ReadLine(), out expenseDate))
                {
                    Console.WriteLine("Invalid input. Please enter a valid date in the format yyyy-MM-dd.");
                    Console.Write("Enter the expense date (yyyy-MM-dd): ");
                }
                expenseDTO.date = expenseDate;

                // Prompt for whether the expense is recurring
                Console.Write("Is this expense recurring? (y/n): ");
                string isRecurringInput = Console.ReadLine();
                expenseDTO.isRecurring = isRecurringInput.Equals("y", StringComparison.OrdinalIgnoreCase);

                // Associate the expense with the current user
                expenseDTO.User = userDTO;

                // Add the expense to the service
                _expenseService.AddExpense(expenseDTO);

                // Display the entered expense details
                Console.WriteLine("Expense details:");
                Console.WriteLine($"Category: {expenseDTO.category}");
                Console.WriteLine($"Amount: {expenseDTO.amount}");
                Console.WriteLine($"Date: {expenseDTO.date}");
                Console.WriteLine($"Recurring: {expenseDTO.isRecurring}");

                // Optionally, display all expenses for the user
                Console.Write("Do you want to view all your expenses? (y/n): ");
                string viewExpenses = Console.ReadLine();
                if (viewExpenses.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    List<ExpenseDTO> expenseList = _expenseService.GetExpenses();
                    PrintExpenseList(expenseList);
                }
         



            //ExpenseDTO expenseDTO = new ExpenseDTO();
            //expenseDTO.amount = 100;
            //expenseDTO.date = DateTime.Now;
            //expenseDTO.isRecurring = true;
            //expenseDTO.User = userDTO;
            //_expenseService.AddExpense(expenseDTO);
            //Console.WriteLine(
            //    $"Expense details");
            //Console.WriteLine(expenseDTO.amount + " " + expenseDTO.category + " " + expenseDTO.date + " " + expenseDTO.isRecurring);
            ////another Expense
            //expenseDTO.amount += 100;
            //expenseDTO.date = DateTime.Now;
            //expenseDTO.isRecurring = false;
            //expenseDTO.User = userDTO;
            //_expenseService.AddExpense(expenseDTO);
            //Console.WriteLine(
            //    $"Expense details");
            //Console.WriteLine(expenseDTO.amount + " " + expenseDTO.date);

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Personal Budgeting Application!");
            Console.WriteLine("1. Sign Up(First User Story)");
            Console.WriteLine("2. Login(Second User Story)");
            Console.WriteLine("3. Exit");
            Console.Write("Please choose an option:");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                FirstUserStory();
                string ans = "n";
                do
                {
                    Console.WriteLine("1.  Third User Story (Tracking Income)");
                    Console.WriteLine("2.  Fourth User Story (Tracking Budgeting)");
                    Console.WriteLine("3.  Fifth User Story (Reminder)");
                    Console.WriteLine("4.  Seventh User Story (Tracking Expense)");
                    Console.Write("Which User Story To test?");
                    choice= Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            ThirdUserStory();
                            break;
                        case "2":
                            FourthUserStory();
                            break;
                        case "3":
                            FifthUserStory();
                            break;
                        case "4":
                            SeventhUserStory();
                            break;

                    }
                    Console.Write("Want to test another user story?y/n");
                    ans=Console.ReadLine();
                } while (ans == "y" || ans == "Y");


            }
            else if (choice == "2")
            {
                SecondUserStory();
                string ans = "n";
                do
                {
                    Console.WriteLine("1.  Third User Story (Tracking Income)");
                    Console.WriteLine("2.  Fourth User Story (Tracking Budgeting)");
                    Console.WriteLine("3.  Fifth User Story (Reminder)");
                    Console.WriteLine("4.  Seventh User Story (Tracking Expense)");
                    Console.Write("Which User Story To test?");
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            ThirdUserStory();
                            break;
                        case "2":
                            FourthUserStory();
                            break;
                        case "3":
                            FifthUserStory();
                            break;
                        case "4":
                            SeventhUserStory();
                            break;

                    }
                    Console.Write("Want to test another user story?y/n? ");
                    ans = Console.ReadLine();
                } while (ans == "y" || ans == "Y");
            }
            else if (choice == "3")
            {
                Environment.Exit(0);
            }


        }
    }
}
