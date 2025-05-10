using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

        public User login(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Personal_Bugeting;Integrated Security=True"))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE email = @email AND passwordHash = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return new User(reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(0));
                }
            }
            return null;
        }



    }
}
