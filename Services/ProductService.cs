using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite; // Add this using directive

public class ProductService
{
    public List<Product> LoadProducts()
    {
        var products = new List<Product>();

        using (var connection = DatabaseHelper.GetConnection())
        {
            var command = new SqliteCommand("SELECT ProductId, Name, Category, Price, Stock FROM Products", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Category = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                        Stock = reader.GetInt32(4)
                    });
                }
            }
        }

        return products;
    }
}
