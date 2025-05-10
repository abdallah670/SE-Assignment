using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Bugeting
{
    public interface IAuthenticateService
    {
        void register(string name,string email, string password);
        User login(string email, string password);
    }
    public class AuthenticateService: IAuthenticateService
    {
        public void register(string name, string email, string password)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Personal_Bugeting;Integrated Security=True"))
            {
                connection.Open();
                string query = "INSERT INTO Users (name, email, passwordHash) VALUES (@name, @email, @password)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
               if(command.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine("User registered successfully.");
                }
                else
                {
                    Console.WriteLine("User registration failed.");
                }


            }
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
            using (SqlConnection connection = new SqlConnection("Server=.;Database=Personal_Bugeting;User Id=sa;Password=sa123456;"))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Email = @email AND PasswordHash = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password",HashPassword(password));
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return new User(reader.GetString(3), reader.GetString(1), reader.GetString(2), reader.GetInt32(0));
                }
            }
            return null;
        }



    }
}
