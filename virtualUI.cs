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
            }
            else
            {
                Console.WriteLine("Invalid email or password. Please try again.");
            }

        }
    }
}
