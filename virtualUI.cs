using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    internal class virtualUI
    {
        static void Main(string[] args)
        {
         
            AuthenticateService authService = new AuthenticateService();
            Console.WriteLine("Welcome to Personal Budgeting App!");
            Console.WriteLine("Please log in to continue.");
            Console.WriteLine("Enter your email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Enter your password: ");
            string password = Console.ReadLine();
            User user=authService.login(email, password);
            if (user != null)
            {
                Console.WriteLine("Login successful!");
                Console.WriteLine($"Welcome, {user.Name}!");
                // Proceed with the application logic
                //Goal test
                //Console.WriteLine("Please enter the goal name: ");
                //string goalName = Console.ReadLine();
                //// Validate goal name
                //while (string.IsNullOrWhiteSpace(goalName))
                //{
                //    Console.WriteLine("Invalid input. Goal name cannot be empty. Please enter a valid goal name: ");
                //    goalName = Console.ReadLine();
                //}

                //decimal targetAmount;
                //Console.WriteLine("Please enter the target amount: ");
                //while (!decimal.TryParse(Console.ReadLine(), out targetAmount))
                //{
                //    Console.WriteLine("Invalid input. Please enter a valid target amount: ");
                //}

                //DateTime targetDate;
                //Console.WriteLine("Please enter the target date (yyyy-mm-dd): ");
                //while (!DateTime.TryParse(Console.ReadLine(), out targetDate))
                //{
                //    Console.WriteLine("Invalid input. Please enter a valid target date (yyyy-mm-dd): ");
                //}

                //Console.WriteLine(
                //    $"Goal Name: {goalName}, Target Amount: {targetAmount}, Target Date: {targetDate.ToShortDateString()}");

                //Goal goal = new Goal
                //{
                //    Name = goalName,
                //    targetAmount = targetAmount,
                //    targetDate = targetDate,
                //    currentAmount = 0
                //};
                //Dictionary<DateTime, decimal> timeline = goal.CalculateTimeLine();
                //Console.WriteLine("Timeline for your goal:");
                //foreach (var entry in timeline)
                //{
                //    Console.WriteLine($"Date: {entry.Key.ToShortDateString()}, Amount: {entry.Value}");
                //}

                //Console.WriteLine(
                //    "Please enter the amount to add to your goal: ");
                //decimal amountToAdd;
                //while (!decimal.TryParse(Console.ReadLine(), out amountToAdd))
                //{
                //    Console.WriteLine("Invalid input. Please enter a valid amount: ");
                //}
                //goal.AddToGoal(amountToAdd);
                //Console.WriteLine(
                //    $"Amount added to goal. Current amount: {goal.currentAmount}");
                // timeline = goal.CalculateTimeLine();
                //Console.WriteLine("Timeline for your goal:");
                //foreach (var entry in timeline)
                //{
                //    Console.WriteLine($"Date: {entry.Key.ToShortDateString()}, Amount: {entry.Value}");
                //}
                //budget class test
                Console.WriteLine(
                    "Please enter the budget name: ");
                string budgetName = Console.ReadLine();
                // Validate budget name 
                while (string.IsNullOrWhiteSpace(budgetName))
                {
                    Console.WriteLine("Invalid input. Budget name cannot be empty. Please enter a valid budget name: ");
                    budgetName = Console.ReadLine();
                }
                decimal budgetLimit;
                Console.WriteLine("Please enter the budget limit: ");
                while (!decimal.TryParse(Console.ReadLine(), out budgetLimit))
                {
                    Console.WriteLine("Invalid input. Please enter a valid budget limit: ");
                }
                DateTime periodStart;
                Console.WriteLine("Please enter the budget period start date (yyyy-mm-dd): ");
                while (!DateTime.TryParse(Console.ReadLine(), out periodStart))
                {
                    Console.WriteLine("Invalid input. Please enter a valid budget period start date (yyyy-mm-dd): ");
                }
                DateTime periodEnd;
                Console.WriteLine(
                    "Please enter the budget period end date (yyyy-mm-dd): ");
                while (!DateTime.TryParse(Console.ReadLine(), out periodEnd))
                {
                    Console.WriteLine("Invalid input. Please enter a valid budget period end date (yyyy-mm-dd): ");
                }
                Console.WriteLine(
                    $"Budget Name: {budgetName}, Budget Limit: {budgetLimit}, Period Start: {periodStart.ToShortDateString()}, Period End: {periodEnd.ToShortDateString()}");
                Budget budget = new Budget
                {
                    category = budgetName,
                    limit = budgetLimit,
                    periodStart = periodStart,
                    periodEnd = periodEnd,
                    spent = 0
                };
                Console.WriteLine("Please enter the amount spent: ");
                decimal amountSpent;
                Console.WriteLine(
                    "Please enter the amount spent: ");
                while (!decimal.TryParse(Console.ReadLine(), out amountSpent))
                {
                    Console.WriteLine("Invalid input. Please enter a valid amount: ");
                }
                Console.WriteLine(
                    $"Amount spent: {amountSpent}");
                budget.spent += amountSpent;
                if (budget.isExceeded())
                {
                    Console.WriteLine("Budget exceeded!");
                    budget.spent =budgetLimit;
                }
                else
                {
                    Console.WriteLine("Budget not exceeded.");


                }
                Console.WriteLine(
                    $"Budget Name: {budget.category}, Budget Limit: {budget.limit}, Amount Spent: {budget.spent}, Period Start: {budget.periodStart.ToShortDateString()}, Period End: {budget.periodEnd.ToShortDateString()}");
                Console.WriteLine($"Budget Progress: {budget.getprogress()}%");
            }
            else
            {
                Console.WriteLine("Invalid email or password. Please try again.");
            }

        }
    }
}
