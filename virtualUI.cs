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
            Console.WriteLine("Income Tracking Page");
            IncomeDTO incomeDTO = new IncomeDTO();
            incomeDTO.amount = 1000;
            incomeDTO.date = DateTime.Now;
            incomeDTO.isRecurring = true;
            incomeDTO.Source = "Salary";
            incomeDTO.User = userDTO;
            _incomeService.AddIncome(incomeDTO);
            Console.WriteLine($"Income details");
            Console.WriteLine($"Income Source: {incomeDTO.Source}, Amount:{incomeDTO.amount}");
            //another income
            incomeDTO.amount += 1000;
            incomeDTO.date = DateTime.Now;
            incomeDTO.isRecurring = false;
            incomeDTO.Source = "Bonus";
            incomeDTO.User = userDTO;
            _incomeService.AddIncome(incomeDTO);
            Console.WriteLine($"Income details");
            Console.WriteLine($"Income Source: {incomeDTO.Source}, Amount:{incomeDTO.amount}");
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
            budgetDTO.category = "income";
            budgetDTO.periodStart = DateTime.Now;
            budgetDTO.periodEnd = DateTime.Now.AddDays(2);
            budgetDTO.limit = 1000;
            budgetDTO.spent = 500;
            budgetDTO.User = userDTO;
            _budgetService.AddBudget(budgetDTO);
            Console.WriteLine("Budget detalis");
            Console.WriteLine($"Category: {budgetDTO.category}, StartPeriod: {budgetDTO.periodStart}, EndPeriod: {budgetDTO.periodEnd}");
            Console.WriteLine("Progress: " + _budgetService.CalculateProgress() + "%");
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
            reminderDTO.Message = "Pay your bills";
            reminderDTO.Date = DateTime.Now.AddDays(7);
            reminderDTO.User = userDTO;
            _reminderService.ScheduleReminder(reminderDTO);
            Console.WriteLine("Suppose The remider day is today");
            _reminderService.Notify(userDTO.Id, "you should pay your bell now");
            PrintNoti();

        }
        //Senventh UserStory
        private static void SeventhUserStory()//Expense Tracking
        {
            Console.WriteLine("Expense Page");
            ExpenseDTO expenseDTO = new ExpenseDTO();
            expenseDTO.amount = 100;
            expenseDTO.date = DateTime.Now;
            expenseDTO.isRecurring = true;
            expenseDTO.User = userDTO;
            _expenseService.AddExpense(expenseDTO);
            Console.WriteLine(
                $"Expense details");
            Console.WriteLine(expenseDTO.amount + " " + expenseDTO.category + " " + expenseDTO.date + " " + expenseDTO.isRecurring);
            //another Expense
            expenseDTO.amount += 100;
            expenseDTO.date = DateTime.Now;
            expenseDTO.isRecurring = false;
            expenseDTO.User = userDTO;
            _expenseService.AddExpense(expenseDTO);
            Console.WriteLine(
                $"Expense details");
            Console.WriteLine(expenseDTO.amount + " " + expenseDTO.date);

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
