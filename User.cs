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
        public UserDTO user;
        private readonly IUserRepository _userRepository;
        private readonly AuthenticateService _authService;

        public User(IUserRepository userRepository, AuthenticateService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        public User(UserDTO user)
        {
            this.user = user ?? new UserDTO(); // Initialize if null
            _authService = new AuthenticateService();
        }
        public User(string email, string password)
        {
            this.user = new UserDTO(); // Initialize the user field
            user.Email = email;
            user.PasswordHash = password;
            _authService = new AuthenticateService();
        }
        public bool authenticate()
        {
        
            User authenticatedUser = _authService.login(user.Email,user.PasswordHash);
            if (authenticatedUser != null)
            {
                user.Id = authenticatedUser.user.Id;
                user.Name = authenticatedUser.user.Name;
                user.Email = authenticatedUser.user.Email;
                user.PasswordHash = user.PasswordHash;
                return true;
            }
            else
            {
                Console.WriteLine("Authentication failed.");
                return false;
            }
        }
      
    }
    
}
