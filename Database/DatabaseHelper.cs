using System;
using Microsoft.Data.Sqlite;

public static class DatabaseHelper
{
    private const string ConnectionString = "Data Source=store.db";

    public static SqliteConnection GetConnection()
    {
        try
        {
            var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            return connection;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing SQLite connection: {ex.Message}");
            throw;
        }
    }
}
