using System;
using Microsoft.Data.Sqlite;

public class UserService
{
    public bool RegisterUser(string username, string password)
    {
        using (var connection = DatabaseHelper.GetConnection())
        {
            var command = new SqliteCommand("INSERT INTO Users (Username, Password) VALUES (@username, @password)", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (SqliteException)
            {
                Console.WriteLine("Username already exists.");
                return false;
            }
        }
    }

    public bool LoginUser(string username, string password)
    {
        using (var connection = DatabaseHelper.GetConnection())
        {
            var command = new SqliteCommand("SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
    }
}
