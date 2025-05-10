using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; private set; }
        private readonly IUserRepository _userRepository;
        private readonly AuthenticateService _authService;
        public User(IUserRepository userRepository, AuthenticateService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        public User(string name, string email, string passwordHash, int id)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Id = id;
        }
        public User(string email, string password)
        {
            Email = email;
            PasswordHash = _authService.HashPassword(password);
        }
        public bool authenticate()
        {
        
            User authenticatedUser = _authService.login(Email, PasswordHash);
            if (authenticatedUser != null)
            {
                Console.WriteLine("User authenticated successfully.");
                this.Id = authenticatedUser.Id;
                this.Name = authenticatedUser.Name;
                this.Email = authenticatedUser.Email;
                this.PasswordHash = authenticatedUser.PasswordHash;
                return true;
            }
            else
            {
                Console.WriteLine("Authentication failed.");
                return false;
            }
        }

        public void logout()
        {
            // Logic to log out the user
            // This could involve clearing session data, tokens, etc.
            Console.WriteLine("User logged out successfully.");
            Environment.Exit(0);
        }

        public void resetPassword(string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentException("Password cannot be empty.");
            this.PasswordHash = _authService.HashPassword(newPassword);

            _userRepository.Update(this);
        }
      
    }
    public  interface IUserRepository
    {
        void Save(User user);
        void Update(User user);
    }
}
